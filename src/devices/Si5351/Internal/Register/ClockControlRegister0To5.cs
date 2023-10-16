// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Iot.Device.Si5351.Internal.Common;

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Represents the CLKx Control register for clocks 0 to 5.
    /// It adds the Multisynth integer mode (MSx_INT) setting to the common control settings.
    /// This setting is only available for clocks 0 to 5.
    /// Addresses:
    ///     CLK0: 16d/10H
    ///     CLK1: 17d/11H
    ///     CLK2: 18d/12H
    ///     CLK3: 19d/13H
    ///     CLK4: 20d/14H
    ///     CLK5: 21d/15H
    /// Documentation: AN619, Skyworks Solutions Inc.
    /// </summary>
    public class ClockControlRegister0To5 : ClockControlRegister
    {
        /// <summary>
        /// Initializes a new instance of a ClockControlRegiser0To5 class for a specific clock number.
        /// </summary>
        /// <param name="number">Clock number 0:5</param>
        public ClockControlRegister0To5(byte number)
            : base(number)
        {
            if (number < 0 || number > 5)
            {
                throw new ArgumentException($"Invalid clock number ({number})");
            }
        }

        /// <summary>
        /// Multisynth operation mode.
        /// </summary>
        public OperationMode MS_INT { get; set; } = OperationMode.Fractional;

        /// <inheritdoc/>
        public override void Read()
        {
            byte data = ReadCommon();
            MS_INT = (data & 0b0100_0000) == 0 ? OperationMode.Fractional : OperationMode.Integer;
        }

        /// <inheritdoc/>
        public override void Write()
        {
            byte data = 0;
            data |= (byte)(MS_INT == OperationMode.Integer ? 0b0100_0000 : 0);
            WriteCommon(data);
        }
    }
}
