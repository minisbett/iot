// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Represents the PLL Reset register.
    /// Address: 177d/B1h
    /// Documentation: AN619, Skyworks Solutions Inc.
    /// </summary>
    public class PllResetRegister : DeviceRegister
    {
        /// <summary>
        /// Initializes a new instance of the PllResetRegister class.
        /// </summary>
        public PllResetRegister()
            : base(Address.PllReset)
        {
        }

        /// <summary>
        /// Setting this flag to 'true' will reset PLL A when writing the register to the device.
        /// Important: the corresponding bit in the device register is self clearing.
        /// Therefore resulting always in a 'false' state when reading back.
        /// </summary>
        public bool PllARst { get; set; }

        /// <summary>
        /// Setting this flag to 'true' will reset PLL B when writing the register to the device.
        /// Important: the corresponding bit in the device register is self clearing.
        /// Therefore resulting always in a 'false' state when reading back.
        /// </summary>
        public bool PllBRst { get; set; }

        /// <inheritdoc/>
        public override void Read()
        {
            byte data = ReadData().DataA;
            PllARst = (data & 0b0010_0000) != 0;
            PllBRst = (data & 0b1000_0000) != 0;
        }

        /// <inheritdoc/>
        public override void Write()
        {
            WriteDataPreserve((byte)((PllARst ? 0b0010_0000 : 0) | (PllBRst ? 0b1000_0000 : 0)), 0b1010_0000);
        }
    }
}
