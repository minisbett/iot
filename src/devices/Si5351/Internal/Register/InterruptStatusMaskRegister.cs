// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Si5351.Internal.Register;

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Represents the interrupt status mask register.
    /// Address: 2d/02h
    /// Documentation: AN619, Skyworks Solutions Inc.
    /// </summary>
    public class InterruptStatusMaskRegister : DeviceRegister
    {
        /// <summary>
        /// Initializes a new instance of the InterruptStatusMaskRegister class.
        /// </summary>
        public InterruptStatusMaskRegister()
            : base(Address.InterruptStatusMask)
        {
        }

        /// <summary>
        /// Setting the SYS_INIT_MASK bit prevents the INTR pin (Si5351C only) from going low when SYS_INIT interrupt occurs.
        /// </summary>
        public bool SYS_INIT_MASK { get; set; } = false;

        /// <summary>
        /// Setting the LOL_B_MASK bit prevents the INTR pin (Si5351C only) from going low when LOL_B interrupt occurs.
        /// </summary>
        public bool LOL_B_MASK { get; set; } = false;

        /// <summary>
        /// Setting the LOL_A_MASK bit prevents the INTR pin (Si5351C only) from going low when LOL_A interrupt occurs.
        /// </summary>
        public bool LOL_A_MASK { get; set; } = false;

        /// <summary>
        /// Setting the LOS_CLKIN_MASK bit prevents the INTR pin (Si5351C only) from going low when LOS_CLKIN interrupt occurs.
        /// </summary>
        public bool LOS_CLKIN_MASK { get; set; } = false;

        /// <summary>
        /// Setting the LOS_XTAL_MASK bit prevents the INTR pin (Si5351C only) from going low when LOS_TAL interrupt occurs.
        /// </summary>
        public bool LOS_XTAL_MASK { get; set; } = false;

        /// <inheritdoc/>
        public override void Read()
        {
            byte data = ReadData().DataA;
            SYS_INIT_MASK = (data & 0b1000_0000) == 1;
            LOL_B_MASK = (data & 0b0100_0000) == 1;
            LOL_A_MASK = (data & 0b0010_0000) == 1;
            LOS_CLKIN_MASK = (data & 0b0001_0000) == 1;
            LOS_XTAL_MASK = (data & 0b0000_1000) == 1;
        }

        /// <inheritdoc/>
        public override void Write()
        {
            byte data = 0;
            data |= (byte)(SYS_INIT_MASK ? 0b1000_0000 : 0);
            data |= (byte)(LOL_B_MASK ? 0b0100_0000 : 0);
            data |= (byte)(LOL_A_MASK ? 0b0010_0000 : 0);
            data |= (byte)(LOS_CLKIN_MASK ? 0b0001_0000 : 0);
            data |= (byte)(LOS_XTAL_MASK ? 0b0000_1000 : 0);
            WriteDataPreserve(data, 0b1111_1000);
        }
    }
}
