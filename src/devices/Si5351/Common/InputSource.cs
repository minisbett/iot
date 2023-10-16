// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Iot.Device.Si5351.Common
{
    /// <summary>
    /// Defines the set of available input sources for PLLx.
    /// </summary>
    public enum InputSource : byte
    {
        /// <summary>
        /// XTAL input
        /// </summary>
        XTAL = 0,

        /// <summary>
        /// CLKIN input
        /// </summary>
        CLKIN = 1
    }
}
