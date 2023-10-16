// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Iot.Device.Si5351.Internal.Common;

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Represents the CLKx Control register for clocks 6 to 7.
    /// It adds the Feedback A/B Multisynth integer mode (FBA_INT/FBB_INT) setting to the common control settings.
    /// This setting is only available for clocks 6 to 7.
    /// Addresses:
    ///     CLK6: 22d/16H
    ///     CLK7: 23d/17H
    /// Documentation: AN619, Skyworks Solutions Inc.
    /// </summary>
    public class ClockControlRegister6To7 : ClockControlRegister
    {
        /// <summary>
        /// Initializes a new instance of a ClockControlRegiser6To7 class for a specific clock number.
        /// </summary>
        /// <param name="number">Clock number 6:7</param>
        public ClockControlRegister6To7(byte number)
            : base(number)
        {
            if (number < 6 || number > 7)
            {
                throw new ArgumentException($"Invalid clock number ({number})");
            }
        }

        /// <summary>
        /// Multisynth operation mode.
        /// </summary>
        public OperationMode FB_INT { get; set; } = OperationMode.Fractional;

        /// <inheritdoc/>
        public override void Read()
        {
            byte data = ReadCommon();
            FB_INT = (data & 0b0100_0000) == 0 ? OperationMode.Fractional : OperationMode.Integer;
        }

        /// <inheritdoc/>
        public override void Write()
        {
            byte data = 0;
            data |= (byte)(FB_INT == OperationMode.Integer ? 0b0100_0000 : 0);
            WriteCommon(data);
        }
    }
}
