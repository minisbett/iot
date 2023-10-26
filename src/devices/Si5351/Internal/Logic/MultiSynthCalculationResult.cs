// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using Iot.Device.Si5351.Common;

namespace Si5351.Internal.Logic
{
    /// <summary>
    /// Represents the result of a <see cref="MultiSynthConfigurator.Calculate(int, double?)"/> operation,
    /// containing the resulting M, N, R and DB4 value, as well as whether we can use integer-only mode.
    /// </summary>
    internal class MultiSynthCalculationResult
    {
        /// <summary>
        /// The multiplier for the PLL.
        /// </summary>
        public double M { get; init; }

        /// <summary>
        /// The divider for the stage 2 synth.
        /// </summary>
        public double N { get; init; }

        /// <summary>
        /// The output divider for the clock.
        /// </summary>
        public OutputDivider R { get; init; }

        /// <summary>
        /// Bool whether the divide by 4 mode is enabled.
        /// </summary>
        public bool DivideBy4 { get; init; }

        /// <summary>
        /// Bool whether all values are integer, meaning the integer-only mode can be enabled.
        /// </summary>
        public bool IntegerMode { get; init; }
    }
}
