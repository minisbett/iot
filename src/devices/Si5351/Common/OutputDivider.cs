// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Iot.Device.Si5351.Common
{
    /// <summary>
    /// Defines the set of Multisynth dividers
    /// </summary>
    public enum OutputDivider : byte
    {
        /// <summary>
        /// Divide by 1
        /// </summary>
        Div1 = 0b000,

        /// <summary>
        /// Divide by 2
        /// </summary>
        Div2 = 0b001,

        /// <summary>
        /// Divide by 4
        /// </summary>
        Div4 = 0b010,

        /// <summary>
        /// Divide by 8
        /// </summary>
        Div8 = 0b011,

        /// <summary>
        /// Divide by 16
        /// </summary>
        Div16 = 0b100,

        /// <summary>
        /// Divide by 32
        /// </summary>
        Div32 = 0b101,

        /// <summary>
        /// Divide by 64
        /// </summary>
        Div64 = 0b110,

        /// <summary>
        /// Divide by 128
        /// </summary>
        Div128 = 0b111
    }
}
