// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;

namespace Si5351.Internal.Logic
{
    /// <summary>
    /// Determines parameters P1, P2 and P3 for configuring a MultiSynth component.
    /// </summary>
    public class MultiSynthConfigurator_old
    {
        private readonly int _fVco;
        private readonly int _fOut;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiSynthConfigurator_old"/> class.
        /// </summary>
        /// <param name="fVco">Intermediate VCO frequency [Hz]</param>
        /// <param name="fOut">Clock output frequency [Hz]</param>
        public MultiSynthConfigurator_old(int fVco, int fOut)
        {
            _fVco = fVco;
            _fOut = fOut;

            if (_fOut < 500_000 || _fOut > 150_000_000)
            {
                throw new ArgumentException("Driver supports currently only fOut between 500 kHz and 150 MHz.");
            }
        }

        /*
            private (int p1, int p2, int p3, int r) Calculate()
            {
                // note: below the variables a, b and c represent the respective parameters as
                //       used in the calculation schema in IN619, Skyworks Solutions Inc.
                int a = (int)(_fVco / _fOut);

                if (a < 8 || a > 900)
                {
                    // HIER MUSS ICH NOCH R BERÜCKSICHTIGEN!
                    throw new Exception("Invalid parameter a for fractional mode.");
                }

                // Calculate B and C (fractional part of the divider)
                double fractionalPart = (_fVco / _fOut) - a;
                double tolerance = 1e-9; // a small tolerance for comparing double values
                // double limit = 1.0; // Initial limit for the fraction
                double bestFraction = fractionalPart;
                int bestDenominator = 1;
                int b;
                int c;

                for (int denominator = 1; denominator <= 2048; denominator++)
                {
                    double fraction = fractionalPart * denominator;
                    int roundedFraction = (int)Math.Round(fraction);

                    if (Math.Abs(fraction - roundedFraction) < tolerance)
                    {
                        b = roundedFraction;
                        c = denominator;
                        break;
                    }
                    else if (Math.Abs(fraction - (roundedFraction + 1)) < tolerance)
                    {
                        b = roundedFraction + 1;
                        c = denominator;
                        break;
                    }
                    else
                    {
                        double newFraction = Math.Abs(fraction - roundedFraction);
                        if (newFraction < bestFraction)
                        {
                            bestFraction = newFraction;
                            bestDenominator = denominator;
                        }
                    }
                }

                if (bestFraction < tolerance)
                {
                    b = (int)(fractionalPart * bestDenominator);
                    c = bestDenominator;
                }
                else
                {
                    // No exact match, use the best approximation
                    b = (int)(bestFraction * bestDenominator);
                    c = bestDenominator;
                }
            }
        */
    }
}
