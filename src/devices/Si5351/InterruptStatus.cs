// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Si5351.Internal.Register;

namespace Iot.Device.Si5351
{
    /// <summary>
    /// Represents the stauts of the interrupt (sticky) bits
    /// </summary>
    public class InterruptStatus
    {
        /// <summary>
        /// System initialization interrupt (SYS_INIT) status bit
        /// </summary>
        /// <value>True, if an interrupt has occured since it was last cleared.</value>
        public bool SystemInitialization { get; }

        /// <summary>
        /// Loss of clock signal (CLKIN) interrupt status bit
        /// </summary>
        /// <value>True, if an interrupt has occured since it was last cleared.</value>
        public bool ClockSignalLoss { get; }

        /// <summary>
        /// Loss of crystal signal (XTAL) interrupt status bit
        /// </summary>
        /// <value>True, if an interrupt has occured since it was last cleared.</value>
        public bool CrystalSignalLoss { get; }

        /// <summary>
        /// PLL A loss of lock interrupt status
        /// </summary>
        /// <value>True, if the PLL has been locked on a valid reference from either CLKIN or XTAL</value>
        public bool PllALockLoss { get; }

        /// <summary>
        /// PLL A loss of lock interrupt status
        /// </summary>
        /// <value>True, if the PLL has been locked on a valid reference from either CLKIN or XTAL</value>
        public bool PllBLockLoss { get; }

        /// <summary>
        /// Initializes a new intance from a given register value
        /// </summary>
        public InterruptStatus(InterruptStatusStickyRegister register)
        {
            SystemInitialization = register.SYS_INIT_STKY;
            PllBLockLoss = register.LOL_B_STKY;
            PllALockLoss = register.LOL_A_STKY;
            ClockSignalLoss = register.LOS_CLKIN_STKY;
            CrystalSignalLoss = register.LOS_XTAL_STKY;
        }

        /// <inheritdoc/>
        public override string ToString() =>
            $"{nameof(SystemInitialization)}:{SystemInitialization}, {nameof(ClockSignalLoss)}:{ClockSignalLoss}, {nameof(CrystalSignalLoss)}:{CrystalSignalLoss}, {nameof(PllALockLoss)}:{PllALockLoss}, {nameof(PllBLockLoss)}:{PllBLockLoss}";
    }
}
