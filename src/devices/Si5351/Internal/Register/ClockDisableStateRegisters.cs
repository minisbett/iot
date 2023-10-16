// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using Iot.Device.Si5351.Common;
using Iot.Device.Si5351.Internal;

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Represents the registers CLK3-0 Disable State and CLK7-4 Disable State.
    /// The settings define the behaviour of the clock outputs (7-0) when it is disabled.
    /// This applies to both, disabling using the Output Enable Control register (3h) or
    /// the OEB pin.
    /// Addresses: 24d/18h, 25d/19h
    /// Documentation: AN619, Skyworks Solutions Inc.
    /// </summary>
     public class ClockDisableStateRegisters : DeviceRegister
     {
        /// <summary>
        /// Initializes a new instance of the ClockDisableStateRegisters class.
        /// </summary>
        public ClockDisableStateRegisters()
            : base(Address.CLK74_DisableState, Address.CLK30_DisableState)
        {
        }

        /// <summary>
        /// Behaviour of clock 7 if disabled
        /// </summary>
        public DisableState CLK7_DIS_STATE { get; set; } = DisableState.LOW;

        /// <summary>
        /// Behaviour of clock 6 if disabled
        /// </summary>
        public DisableState CLK6_DIS_STATE { get; set; } = DisableState.LOW;

        /// <summary>
        /// Behaviour of clock 5 if disabled
        /// </summary>
        public DisableState CLK5_DIS_STATE { get; set; } = DisableState.LOW;

        /// <summary>
        /// Behaviour of clock 4 if disabled
        /// </summary>
        public DisableState CLK4_DIS_STATE { get; set; } = DisableState.LOW;

        /// <summary>
        /// Behaviour of clock 3 if disabled
        /// </summary>
        public DisableState CLK3_DIS_STATE { get; set; } = DisableState.LOW;

        /// <summary>
        /// Behaviour of clock 2 if disabled
        /// </summary>
        public DisableState CLK2_DIS_STATE { get; set; } = DisableState.LOW;

        /// <summary>
        /// Behaviour of clock 1 if disabled
        /// </summary>
        public DisableState CLK1_DIS_STATE { get; set; } = DisableState.LOW;

        /// <summary>
        /// Behaviour of clock 0 if disabled
        /// </summary>
        public DisableState CLK0_DIS_STATE { get; set; } = DisableState.LOW;

        /// <inheritdoc/>
        public override void Read()
        {
            (byte dataA, byte dataB, byte dataC) = ReadData();

            CLK7_DIS_STATE = (DisableState)((dataA & 0b1100_0000) >> 6);
            CLK6_DIS_STATE = (DisableState)((dataA & 0b0011_0000) >> 4);
            CLK5_DIS_STATE = (DisableState)((dataA & 0b0000_1100) >> 2);
            CLK4_DIS_STATE = (DisableState)(dataA & 0b0000_0011);

            CLK3_DIS_STATE = (DisableState)((dataB & 0b1100_0000) >> 6);
            CLK2_DIS_STATE = (DisableState)((dataB & 0b0011_0000) >> 4);
            CLK1_DIS_STATE = (DisableState)((dataB & 0b0000_1100) >> 2);
            CLK0_DIS_STATE = (DisableState)(dataB & 0b0000_0011);
        }

        /// <inheritdoc/>
        public override void Write()
        {
            byte dataA = 0;
            dataA |= (byte)((byte)CLK7_DIS_STATE << 6);
            dataA |= (byte)((byte)CLK6_DIS_STATE << 4);
            dataA |= (byte)((byte)CLK5_DIS_STATE << 2);
            dataA |= (byte)CLK4_DIS_STATE;

            byte dataB = 0;
            dataB |= (byte)((byte)CLK3_DIS_STATE << 6);
            dataB |= (byte)((byte)CLK2_DIS_STATE << 4);
            dataB |= (byte)((byte)CLK1_DIS_STATE << 2);
            dataB |= (byte)CLK0_DIS_STATE;

            WriteData(dataA, dataB);
        }
     }
}
