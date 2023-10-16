// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Represents the Fanout Enable register.
    /// Address: 187d/BBh
    /// Documentation: AN619, Skyworks Solutions Inc.
    /// </summary>
    public class FanoutEnableRegister : DeviceRegister
    {
        /// <summary>
        /// Initializes a new instance of the FanoutEnableRegister class.
        /// </summary>
        public FanoutEnableRegister()
            : base(Address.FanoutEnable)
        {
        }

        /// <summary>
        /// Set CLKIN_FANOUT_EN to 'true' to enable fanout of CLKIN to clock output multiplexers.
        /// </summary>
        public bool ClkinFanoutEn { get; set; }

        /// <summary>
        /// Set XO_FANOUT_EN to 'true' to enable fanout of XO to clock output multiplexers.
        /// </summary>
        public bool XoFanoutEn { get; set; }

        /// <summary>
        /// Set MS_FANOUT_EN to 'true' to enable fanout of Multisynth0 and Multisynth4 to all output multiplexers.
        /// </summary>
        public bool MsFanoutEn { get; set; }

        /// <inheritdoc/>
        public override void Read()
        {
            byte data = ReadData().DataA;
            ClkinFanoutEn = (data & 0b1000_0000) != 0;
            XoFanoutEn = (data & 0b0100_0000) != 0;
            MsFanoutEn = (data & 0b0001_0000) != 0;
        }

        /// <inheritdoc/>
        public override void Write()
        {
            byte data = 0;
            data |= (byte)(ClkinFanoutEn ? 0b1000_0000 : 0);
            data |= (byte)(XoFanoutEn ? 0b0100_0000 : 0);
            data |= (byte)(MsFanoutEn ? 0b0001_0000 : 0);
            WriteDataPreserve(data, 0b1101_0000);
        }
    }
}
