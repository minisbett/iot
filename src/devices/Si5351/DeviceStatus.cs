// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Si5351.Internal.Register;

namespace Iot.Device.Si5351
{
    /// <summary>
    /// Represents the Device Status.
    /// </summary>
    public class DeviceStatus
    {
        /// <summary>
        /// Device revision
        /// </summary>
        public DeviceRevision Revision { get; }

        /// <summary>
        /// Clock signel (CLKIN pin) validity (only Si5351C)
        /// </summary>
        /// <value>True, if the signal is valid</value>
        public bool ClockSignalValid { get; }

        /// <summary>
        /// Crystal signal validitiy
        /// </summary>
        /// <value>True, if the signal is valid</value>
        public bool CrystalSignalValid { get; }

        /// <summary>
        /// Locked state of PLL A.
        /// </summary>
        /// <value>True, if the PLL has been locked on a valid reference from either CLKIN or XTAL</value>
        public bool PllALocked { get; }

        /// <summary>
        /// Locked state of PLL B.
        /// </summary>
        /// <value>True, if the PLL has been locked on a valid reference from either CLKIN or XTAL</value>
        public bool PllBLocked { get; }

        /// <summary>
        /// System initialization status.
        /// IMPORTANT: do not perform any write operations (to NVRAM) before initialization has been completed.
        /// </summary>
        /// <value>True, if the device initialization has been completed</value>
        public bool Initialized { get; }

        /// <summary>
        /// Initializes a new intancec from a given register value
        /// </summary>
        public DeviceStatus(DeviceStatusRegister register)
        {
            Revision = (DeviceRevision)register.REVID;
            CrystalSignalValid = !register.LOS_XTAL;
            ClockSignalValid = !register.LOS_CLKIN;
            PllALocked = !register.LOL_A;
            PllBLocked = !register.LOL_B;
            Initialized = !register.SYS_INIT;
        }

        /// <inheritdoc/>
        public override string ToString() =>
            $"{nameof(Revision)}:{Revision}, {nameof(Initialized)}:{Initialized}, {nameof(ClockSignalValid)}:{ClockSignalValid}, {nameof(CrystalSignalValid)}:{CrystalSignalValid}, {nameof(PllALocked)}:{PllALocked}, {nameof(PllBLocked)}:{PllBLocked}";
    }
}
