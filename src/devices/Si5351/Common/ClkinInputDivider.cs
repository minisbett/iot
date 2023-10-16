// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Iot.Device.Si5351.Common
{
    /// <summary>
    /// Defines the set of available input dividers for CLKIN signal.
    /// </summary>
    public enum ClkinInputDivider : byte
    {
        /// <summary>
        /// Divide by 1
        /// </summary>
        Div1 = 0b00,

        /// <summary>
        /// Divide by 2
        /// </summary>
        Div2 = 0b01,

        /// <summary>
        /// Divide by 4
        /// </summary>
        Div4 = 0b10,

        /// <summary>
        /// Divide by 8
        /// </summary>
        Div8 = 0b11
    }
}
