// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Iot.Device.Si5351.Common
{
    /// <summary>
    /// Set of defined clock output input sources.
    /// </summary>
    public enum OutputClockInputSource : byte
    {
        /// <summary>
        /// XTAL is clock output source
        /// </summary>
        XTAL = 0b00,

        /// <summary>
        /// CLKIN is clock output source (by-passes both synthesis stages - PLL/VCXO and MultiSynth).
        /// </summary>
        CLKIN = 0b01,

        /// <summary>
        /// Reserved bits (only clock output 0 and 4).
        /// </summary>
        Reserved = 0b10,

        /// <summary>
        /// Only: MS0 as clock output source for clock output 1, 2 and 3.
        /// </summary>
        MS0 = 0b10,

        /// <summary>
        /// Only: MS4 as clock output source for clock output 5, 6 and 7.
        /// </summary>
        MS4 = 0b10,

        /// <summary>
        /// Corresponding Multisynth is clock output source.
        /// </summary>
        MSx = 0b11
    }
}
