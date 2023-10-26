// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Iot.Device.Si5351.Common;
using Iot.Device.Si5351.Internal.Common;
using Iot.Device.Si5351.Internal.Register;
using UnitsNet;

namespace Si5351.Internal.Logic
{
    /// <summary>
    /// Determines parameters P1, P2 and P3 for configuring a MultiSynth component.
    /// </summary>
    public class MultiSynthConfigurator
    {
        /// <summary>
        /// The reference frequency, either from the Xtal or CLCKIN.
        /// </summary>
        private readonly int _fRef;

        /// <summary>
        /// The output frequencies of the clocks.
        /// </summary>
        private int[] _fOuts = new int[3]
        {
            10_000,
            10_000,
            10_000
        };

        /// <summary>
        /// The enabled states of the clocks.
        /// </summary>
        private bool[] _enabled = new bool[3]
        {
            false,
            false,
            false
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiSynthConfigurator"/> class with the given reference frequency.
        /// </summary>
        /// <param name="fRef">The Reference frequency, either from the Xtal or CLCKIN.</param>
        public MultiSynthConfigurator(int fRef)
        {
            _fRef = fRef;

            // As per datasheet, the input of the PLLs must be between 10MHz and 40MHz.
            if (_fRef < 10e6 || _fRef > 40e6)
            {
                throw new ArgumentException("The Si5351 only supports reference frequencies between 10MHz and 40MHz.");
            }
        }

        /// <summary>
        /// Sets the enabled state of the given clock by it's id.
        /// </summary>
        /// <param name="clockId">The clock id.</param>
        /// <param name="state">The state.</param>
        public void SetClockState(int clockId, bool state)
        {
            if (clockId >= _enabled.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(clockId));
            }

            _enabled[clockId] = state;
        }

        /// <summary>
        /// Sets the frequency of the given clock by it's id.
        /// </summary>
        /// <param name="clockId">The clock id.</param>
        /// <param name="frequency">The frequency.</param>
        public void SetFrequency(int clockId, int frequency)
        {
            if (clockId >= _fOuts.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(clockId));
            }
            else if (frequency < 2289 || frequency > 200e6)
            {
                throw new ArgumentOutOfRangeException(nameof(frequency), "The allowed range for frequencies is 2289Hz <= frequency <= 200MHz.");
            }

            IEnumerable<int> fOutsBiggerEqual112_5MHz = _fOuts.Where(x => x >= 112.5e6);
            if (fOutsBiggerEqual112_5MHz.Distinct().Count() == 2 && !fOutsBiggerEqual112_5MHz.Contains(frequency))
            {
                throw new InvalidOperationException("The Si5351 only supports two different output frequencies >= 112.5MHz.");
            }

            _fOuts[clockId] = frequency;
        }

        /// <summary>
        /// Performs a readjustment of the MultiSynth parameters to provide the targetted frequencies on the clocks.
        /// </summary>
        public void Apply()
        {
            // If all clocks are disabled, set them to disabled in the registry and return.
            if (!_enabled.Any(x => x))
            {
                OutputEnableControlRegister oecr = new OutputEnableControlRegister();
                for (int i = 0; i < _enabled.Length; i++)
                {
                    oecr[i] = true;
                }

                oecr.Write();
                return;
            }

            // Save the results in a list.
            List<MultiSynthCalculationResult?> results = new List<MultiSynthCalculationResult?>();

            // Calculate initial results for every enabled clock (disabled ones are represented as null in the array),
            // disregarding the fact that there can only be two M values shared for all clocks together.
            for (int i = 0; i < _fOuts.Length; i++)
            {
                if (!_enabled[i])
                {
                    results.Add(null);
                    continue;
                }

                results.Add(Calculate(_fOuts[i])!);
            }

            // If we only have two different M values, perfect! We have our values for the two PLLs. If not, we do adjustments.
            if (results.Where(x => x is not null).Select(x => x!.M).Distinct().Count() > 2)
            {
                // If we have more than 2 different M values, we need to figure out a way to adjust the results.
                // First, we get the two Ms that have the most amount of integer-only results, since those are favoured.
                // If only one or none results are integer-only, it would take the first two results, which is completely fine.
                // If multiple Ms have the same amount of occurences, the higher ones are taken.
                // These two M values will be the two values for our PLLs, now we just need to re-calculate all other results.
                double[] twoMostOccuringMs = results.Where(x => x is not null).Select(x => x!.M).Distinct()
                                            .ToDictionary(x => x, x => results.Count(j => j?.M == x && j.IntegerMode))
                                            .OrderByDescending(x => x.Value).OrderByDescending(x => x.Key).Take(2).Select(x => x.Key).ToArray();

                // Go through all other results and force the first, then the second most occuring M. If the first M gave us an integer-only
                // result, we gladly take that. If the second does, we take that. If none do, we take the higher M. This is because while we do
                // favour integer-only results, if it's not possible we always favor higher PLL frequencies for better resolution.
                for (int i = 0; i < results.Count; i++)
                {
                    // If the output is disabled or we have one of our two PLL values, we can skip it.
                    if (!_enabled[i] || twoMostOccuringMs.Contains(results[i]!.M))
                    {
                        continue;
                    }

                    // Calculate the result while forcing the most occuring M value. If it's an integer, we take it and proceed with the next result.
                    results[i] = Calculate(_fOuts[i], forcedM: twoMostOccuringMs[0]);
                    if (results[i]!.IntegerMode)
                    {
                        continue;
                    }

                    // Otherwise, calculate the result while forcing the second most occuring M value.
                    // If that's an integer, we take it and proceed with the next result.
                    MultiSynthCalculationResult result = Calculate(_fOuts[i], forcedM: twoMostOccuringMs[1])!;
                    if (result.IntegerMode)
                    {
                        results[i] = result;
                        continue;
                    }

                    // If none of them is an integer-only value, we take the one with the higher M value.
                    if (result.M > results[i]!.M)
                    {
                        results[i] = result;
                    }
                }
            }

            // Get the PLL multipliers and write them into the register. There might only be one instead of two different values, meaning a PLL can be fully shared.
            double[] pllMultipliers = results.Where(x => x is not null).Select(x => x!.M).Distinct().ToArray();
            Console.ReadKey();
            for (int i = 0; i < pllMultipliers.Length; i++)
            {
                // Break the decimal number M down into a + b/c. 1_048_575 is the limit for b and c, trimming is automatically performed.
                (int a, int b, int c) = FindABC(pllMultipliers[i], 1_048_575);
                MultisynthNParameter1Register p1 = new MultisynthNParameter1Register((Pll)i);
                MultisynthNParameter2Register p2 = new MultisynthNParameter2Register((Pll)i);
                MultisynthNParameter3Register p3 = new MultisynthNParameter3Register((Pll)i);
                p1.Parameter = 128 * a + (int)Math.Floor(128d * b / c) - 512;
                p2.Parameter = 128 * b - c * (int)Math.Floor(128d * b / c);
                p3.Parameter = c;
                p1.Write();
                p2.Write();
                p3.Write();
            }

            // Write the parameters of the stage 2 synths and outputs into the register.
            for (int i = 0; i < results.Count; i++)
            {
                OutputEnableControlRegister oecr = new OutputEnableControlRegister();
                oecr[i] = !_enabled[i];
                oecr.Write();

                // Skip disabled outputs.
                if (!_enabled[i])
                {
                    continue;
                }

                // Set the source PLL (M) and interger-only mode state.
                ClockControlRegister0To5 clock = new ClockControlRegister0To5((byte)i);
                clock.Read();
                clock.CLK_SRC = OutputClockInputSource.MSx;
                clock.MS_INT = results[i]!.IntegerMode ? OperationMode.Integer : OperationMode.Fractional;
                clock.MS_SRC = pllMultipliers[0] == results[i]!.M ? MultiSynthSource.PllA : MultiSynthSource.PllB_Vcxo;
                clock.Write();

                // Break the decimal number M down into a + b/c. 1_048_575 is the limit for b and c, trimming is automatically performed.
                (int a, int b, int c) = FindABC(results[i]!.N, 1_048_575);

                // Write the divider. (N)
                MultisynthParameter1Register p1 = new MultisynthParameter1Register((MultiSynth)i);
                MultisynthParameter2Register p2 = new MultisynthParameter2Register((MultiSynth)i);
                MultisynthParameter3Register p3 = new MultisynthParameter3Register((MultiSynth)i);
                p1.Parameter = 128 * a + (int)Math.Floor(128d * b / c) - 512;
                p2.Parameter = 128 * b - c * (int)Math.Floor(128d * b / c);
                p3.Parameter = c;
                p1.Write();
                p2.Write();
                p3.Write();

                // Write the output divider (R) and Divide By 4 state.
                MultisynthOutputDividerRegister output = new MultisynthOutputDividerRegister((MultiSynth)i);
                output.Divider = results[i]!.R;
                output.DivideBy4 = results[i]!.DivideBy4;
                output.Write();
            }
        }

        /// <summary>
        /// Calculates the M, N, R and DB4 value for the given output frequency.
        /// If no values could be found, an exception is thrown since this should never happen with valid input.
        /// </summary>
        /// <param name="fOut">The output frequency.</param>
        /// <param name="forcedM">An internal parameter to force an M value.</param>
        /// <returns>The M, N, R and DB4 value for the given output frequency.</returns>
        private MultiSynthCalculationResult Calculate(int fOut, double? forcedM = null)
        {
            Console.WriteLine(fOut);
            // On frequencies >= 150MHz, the normal divisors (stage 2, R divisor) are too inaccurate (we assume?)
            // hence why the Si5351 has an extra "Divide by 4" mode which overrides the behavior of N and R.
            // If we have this mode enable, the need to adjust our M so that it works with the / 4, which is fairly easy.
            if (fOut >= 150e6)
            {
                double m = fOut * 4d / _fRef;
                return new MultiSynthCalculationResult()
                {
                    M = m,
                    N = 0,
                    R = OutputDivider.Div1,
                    DivideBy4 = true,
                    IntegerMode = m % 1 == 0
                };
            }

            // Calculate the minimum possible N we'd need with a Div of 1 to evaluate the minimum R we need.
            // If we have a forced M value, we need to take the resulting fVco of it as reference for it instead.
            // Technically, we could also use higher Divs but no matter which higher we choose, dividing by / 2 again
            // will never give us an integer number, so if we are already working with decimal that won't improve the situation.
            // Also ensure that minNDiv1 is at least 8, since on 100mhz for example it's 6 which would result in a too low Div assumption.
            int minimumFVco = forcedM.HasValue ? (int)(forcedM.Value * _fRef) : (int)600e6;
            double minNDiv1 = Math.Max(8, minimumFVco / fOut);
            int r = Enumerable.Range(1, 7).Select(x => (int)Math.Pow(2, x)).First(x => x >= Math.Ceiling(minNDiv1 / 2048));

            // Save a fallback result in case no integer-only solutions could be found.
            MultiSynthCalculationResult? fallback = null;

            // Get the possible range of M (900MHz / fRef to 600MHz / fRef) to ensure integer values of m.
            // The integer rounding is intentional, ensuring M is always a valid integer value and not going out of range.
            // If a forced M is specified, that value exclusively is taken instead.
            List<double> mValues = forcedM.HasValue ? new List<double>() { forcedM.Value } : new List<double>();
            if (!forcedM.HasValue)
            {
                for (double m = (int)(900e6 / _fRef); m >= 600e6 / _fRef; m--)
                {
                    mValues.Add(m);
                }
            }

            foreach (double m in mValues)
            {
                // Calculate our theoretical N for the given M.
                double n = _fRef * m * 1d / (fOut * r);

                // Make sure N is inside the valid range.
                if (n is not (>= 8 and <= 2048))
                {
                    continue;
                }

                // If we encountered an integer, perfect! We'll use this integer pair.
                if (n % 1 == 0)
                {
                    return new MultiSynthCalculationResult()
                    {
                        M = m,
                        N = n,
                        R = (OutputDivider)Enum.Parse(typeof(OutputDivider), $"Div{r}"),
                        DivideBy4 = false,
                        IntegerMode = true
                    };
                }

                // Otherwise, if we encountered valid values for the first time we save them as callback.
                // This is because we start with an fVco of 900MHz going downwards, so we start with the best case.
                if (fallback is null)
                {
                    fallback = new MultiSynthCalculationResult()
                    {
                        M = m,
                        N = n,
                        R = (OutputDivider)Enum.Parse(typeof(OutputDivider), $"Div{r}"),
                        DivideBy4 = false,
                        IntegerMode = false
                    };
                }
            }

            // If we did not find an integer pair, we go with the fallback values. If none exist, throw an exception. (due to invalid input parameters)
            return fallback ?? throw new Exception("Could not determine a MultiSynth configuration for the specified output frequency.");
        }

        /// <summary>
        /// Finds an A-B-C pair for the given decimal number with the equation a + b/c. A, B and C are all integer values.
        /// The maximum parameter specified the maximum value for C. This results in a possible accuracy loss through disposed decimals.
        /// A is automatically assumed to be the whole-number part of the decimal, b/c represents the decimals.
        /// </summary>
        /// <param name="dec">The decimal number.</param>
        /// <param name="max">The maximum number for C.</param>
        /// <returns>The A-B-C pair for the given decimal number.</returns>
        private (int A, int B, int C) FindABC(double dec, int max)
        {
            // If it's an integer number, return dec + 0/1.
            if (dec % 1 == 0)
            {
                return ((int)dec, 0, 1);
            }

            // Use a for the whole number part of the decimal. The decimals are the rest to be defined by b/c.
            int a = (int)dec;
            decimal rest = (decimal)dec - a;

            // Find B and C by representing the rest as rest*(10^precision)/10^precision. precision the desired amount of precision on the decimal places.
            // e.g. if we have 0.125 with an precision of 3, we have the fraction 125/1000. Decimals further than our precision are simply disposed.
            // We can then find the GCD to simplify the fraction. The greatest common divisor for 125 and 1000 is 125, resulting in the fraction 1/8.
            int x = 9;
            int b = (int)(rest * (decimal)Math.Pow(10, x));
            int c = (int)Math.Pow(10, x);
            int gcd = GCD(b, c);

            // Simply the fraction by diving both sides through their GCD. (e.g. 125/1000 -> 1/8)
            b /= gcd;
            c /= gcd;

            // If no trimming needs to be done, return our A-B-C pair.
            if (c <= max)
            {
                return (a, b, c);
            }

            // Otherwise determine the trimming divisor so that c / disivor ~= max and return our A-B-C pair.
            // Since b is always < c, it can use the same trimming divisor to ensure <= max.
            // This operation results in precision loss through disposed decimals, but is necessary.
            double divisor = c * 1d / max;
            return (a, (int)(b / divisor), (int)(c / divisor));
        }

        /// <summary>
        /// Returns the greatest common divisor of the two given values.
        /// </summary>
        /// <param name="a">The first value.</param>
        /// <param name="b">The second value.</param>
        /// <returns>The greatest common divisor of the two given values.</returns>
        private int GCD(int a, int b) => b == 0 ? a : GCD(b, a % b);
    }
}
