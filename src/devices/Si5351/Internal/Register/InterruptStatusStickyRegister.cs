// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Represents the Interrupt Status Sticky register.
    /// The flags in this register correspond to the state of occurred interrupts.
    /// By writing to this register, past occurrences can be reset.
    /// Address: 1d/01h
    /// Documentation: AN619, Skyworks Solutions Inc.
    /// </summary>
    public class InterruptStatusStickyRegister : DeviceRegister
    {
        /// <summary>
        /// Initializes a new instance of the InterruptStatusStickyRegister class.
        /// </summary>
        public InterruptStatusStickyRegister()
            : base(Address.InterruptStatusSticky)
        {
        }

        /// <summary>
        /// The SYS_INIT_STKY bit is triggered when the SYS_INIT bit in the device status register is set high.
        /// </summary>
        public bool SYS_INIT_STKY { get; set; } = false;

        /// <summary>
        /// The LOL_B_STKY bit is triggered when the LOL_B bit in the device status register is set high.
        /// </summary>
        public bool LOL_B_STKY { get; set; } = false;

        /// <summary>
        /// The LOL_A_STKY bit is triggered when the LOL_A bit in the device status register is set high.
        /// </summary>
        public bool LOL_A_STKY { get; set; } = false;

        /// <summary>
        /// The LOS_CLKIN_STKY bit is triggered when the LOS_CLKIN bit in the device status register is set high.
        /// </summary>
        public bool LOS_CLKIN_STKY { get; set; } = false;

        /// <summary>
        /// The LOS_XTASL_STKY bit is triggered when the LOS_XTAL bit in the device status register is set high.
        /// </summary>
        public bool LOS_XTAL_STKY { get; set; } = false;

        /// <inheritdoc/>
        public override void Read()
        {
            byte data = ReadData().DataA;
            SYS_INIT_STKY = (data & 0b1000_0000) == 1;
            LOL_B_STKY = (data & 0b0100_0000) == 1;
            LOL_A_STKY = (data & 0b0010_0000) == 1;
            LOS_CLKIN_STKY = (data & 0b0001_0000) == 1;
            LOS_XTAL_STKY = (data & 0b0000_1000) == 1;
        }

        /// <inheritdoc/>
        public override void Write()
        {
            byte data = 0;
            data |= (byte)(SYS_INIT_STKY ? 0b1000_0000 : 0);
            data |= (byte)(LOL_B_STKY ? 0b0100_0000 : 0);
            data |= (byte)(LOL_A_STKY ? 0b0010_0000 : 0);
            data |= (byte)(LOS_CLKIN_STKY ? 0b0001_0000 : 0);
            data |= (byte)(LOS_XTAL_STKY ? 0b0000_1000 : 0);
            WriteDataPreserve(data, 0b1111_1000);
        }
    }
}
