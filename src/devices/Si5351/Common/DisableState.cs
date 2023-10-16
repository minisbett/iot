// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Iot.Device.Si5351.Common
{
    /// <summary>
    /// Defines the set of available disable states.
    /// </summary>
    public enum DisableState : byte
    {
        /// <summary>
        /// CLKx is set to a low state when disabled
        /// </summary>
        LOW = 0,

        /// <summary>
        /// CLKx is set to a high state when disabled
        /// </summary>
        HIGH = 1,

        /// <summary>
        /// CLKx is set to a high impedance state when disabled
        /// </summary>
        HIGH_IMPEDANCE = 2,

        /// <summary>
        /// CLKx is set to be never disabled WAS MEINT DAS EIGENTLICH??? AUSPROBIEREN!!!
        /// </summary>
        NEVER_DISABLED = 0
    }
}
