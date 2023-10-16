// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Iot.Device.Si5351.Common
{
    /// <summary>
    /// Set of available MultiSynth sources.
    /// </summary>
    public enum MultiSynthSource : byte
    {
        /// <summary>
        /// PLL A is source
        /// </summary>
        PllA = 0,

        /// <summary>
        /// PLL B (Si5351A/C only) or VCXO (Si5351B only) is source
        /// </summary>
        PllB_Vcxo = 1
    }
}
