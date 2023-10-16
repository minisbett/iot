// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using Iot.Device.Si5351.Common;

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Represents the PLL Input Source register.
    /// Address: 15d/0Fh
    /// Documentation: AN619, Skyworks Solutions Inc.
    /// </summary>
    public class PllInputSourceRegister : DeviceRegister
    {
        /// <summary>
        /// Initializes a new instance of the PllInputSourceRegister class.
        /// </summary>
        public PllInputSourceRegister()
            : base(Address.PllInputSource)
        {
        }

        /// <summary>
        /// Input divider for CLKIN signal.
        /// </summary>
        public ClkinInputDivider CLKIN_DIV { get; set; } = ClkinInputDivider.Div1;

        /// <summary>
        /// Input source select for PLL B.
        /// </summary>
        public InputSource PLLB_SRC { get; set; } = InputSource.XTAL;

        /// <summary>
        /// Input source select for PLL A.
        /// </summary>
        public InputSource PLLA_SRC { get; set; } = InputSource.XTAL;

        /// <inheritdoc/>
        public override void Read()
        {
            byte data = ReadData().DataA;
            CLKIN_DIV = (ClkinInputDivider)(data >> 6);
            PLLB_SRC = (InputSource)((data & 0b0000_1000) >> 3);
            PLLA_SRC = (InputSource)((data & 0b0000_0100) >> 2);
        }

        /// <inheritdoc/>
        public override void Write()
        {
            byte data = 0;
            data |= (byte)((byte)CLKIN_DIV << 6);
            data |= (byte)((byte)PLLB_SRC << 3);
            data |= (byte)((byte)PLLA_SRC << 2);
            WriteDataPreserve(data, 0b11001100);
        }
    }
}
