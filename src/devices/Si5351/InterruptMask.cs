// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Si5351.Internal.Register;

namespace Iot.Device.Si5351
{
    /// <summary>
    /// Represents the masks for signaling interrupts at the INTR pin (only Si5351C).
    /// </summary>
    public class InterruptMask
    {
        /// <summary>
        /// System initialization interrupt mask
        /// </summary>
        /// <value>True prevents from setting INTR pin low if the interrupt occurs.</value>
        public bool SystemInitializationMask { get; }

        /// <summary>
        /// Loss of clock signal (CLKIN) interrupt mask
        /// </summary>
        /// <value>True prevents from setting INTR pin low if the interrupt occurs.</value>
        public bool ClockSignalLossMask { get; }

        /// <summary>
        /// Loss of crystal signal (XTAL) interrupt mask
        /// </summary>
        /// <value>True prevents from setting INTR pin low if the interrupt occurs.</value>
        public bool CrystalSignalLossMask { get; }

        /// <summary>
        /// PLL A loss of lock interrupt mask
        /// </summary>
        /// <value>True prevents from setting INTR pin low if the interrupt occurs.</value>
        public bool PllALockLossMask { get; }

        /// <summary>
        /// PLL A loss of lock interrupt mask
        /// </summary>
        /// <value>True prevents from setting INTR pin low if the interrupt occurs.</value>
        public bool PllBLockLossMask { get; }

        /// <summary>
        /// Initializes a new intance from a given register value
        /// </summary>
        public InterruptMask(InterruptStatusMaskRegister register)
        {
            SystemInitializationMask = register.SYS_INIT_MASK;
            PllBLockLossMask = register.LOL_B_MASK;
            PllALockLossMask = register.LOL_A_MASK;
            ClockSignalLossMask = register.LOS_CLKIN_MASK;
            CrystalSignalLossMask = register.LOS_XTAL_MASK;
        }

        /// <summary>
        /// Create a new register instance initialized with the interrupt mask settings
        /// </summary>
        /// <returns></returns>
        public InterruptStatusMaskRegister ToRegister()
        {
            InterruptStatusMaskRegister register = new()
            {
                SYS_INIT_MASK = SystemInitializationMask,
                LOL_B_MASK = PllBLockLossMask,
                LOL_A_MASK = PllALockLossMask,
                LOS_CLKIN_MASK = ClockSignalLossMask,
                LOS_XTAL_MASK = CrystalSignalLossMask
            };

            return register;
        }

        /// <inheritdoc/>
        public override string ToString() =>
            $"{nameof(SystemInitializationMask)}:{SystemInitializationMask}, {nameof(PllBLockLossMask)}:{PllBLockLossMask}, {nameof(PllALockLossMask)}:{PllALockLossMask}, {nameof(ClockSignalLossMask)}:{ClockSignalLossMask}, {nameof(CrystalSignalLossMask)}:{CrystalSignalLossMask}";
    }
}
