// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Iot.Device.Si5351.Common
{
    /// <summary>
    /// Set of available power states for the clock output driver.
    /// </summary>
    public enum PowerState : byte
    {
        /// <summary>
        /// Output driver is powered up
        /// </summary>
        PoweredUp = 0,

        /// <summary>
        /// Output driver is powered down
        /// </summary>
        PoweredDown = 1
    }
}
