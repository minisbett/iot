// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Represents the Device Status register.
    /// The flags of this read-only register report general device status information.
    /// Address: 0d/00h
    /// Documentation: AN619, Skyworks Solutions Inc.
    /// </summary>
    public class DeviceStatusRegister : DeviceRegister
    {
        /// <summary>
        /// Initializes a new instance of the DeviceStatusRegister class.
        /// </summary>
        public DeviceStatusRegister()
            : base(Address.DeviceStatus)
        {
        }

        /// <summary>
        /// Flag indicates the status of system initialization (false = device ready).
        /// </summary>
        public bool SYS_INIT { get; private set; }

        /// <summary>
        /// Flag indicates the loss of lock for PLL B (false = PLL B is locked).
        /// </summary>
        public bool LOL_B { get; private set; }

        /// <summary>
        /// Flag indicates the loss of lock for PLL A (false = PLL A is locked).
        /// </summary>
        public bool LOL_A { get; private set; }

        /// <summary>
        /// Flag indicates the loss of CLKIN signal (false = valid clock signal at the CLKIN pin).
        /// </summary>
        public bool LOS_CLKIN { get; private set; }

        /// <summary>
        /// Flag indicates the loss of crystal signal (false = valid crystal signal at the XA and XB pins).
        /// </summary>
        public bool LOS_XTAL { get; private set; }

        /// <summary>
        /// Revision number of the device.
        /// </summary>
        public DeviceRevision REVID { get; private set; }

        /// <inheritdoc/>
        public override void Read()
        {
            byte data = ReadData().DataA;
            SYS_INIT = (data & 0b1000_0000) != 0;
            LOL_B = (data & 0b0100_0000) != 0;
            LOL_A = (data & 0b0010_0000) != 0;
            LOS_CLKIN = (data & 0b0001_0000) != 0;
            LOS_XTAL = (data & 0b0000_1000) != 0;
            REVID = (DeviceRevision)(data & 0b0000_0011);
        }

        /// <inheritdoc/>
        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}
