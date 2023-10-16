// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Iot.Device.Si5351.Common;

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Represents the (multi-)register of Parameter 3 of the Multisynth NA (PLL A) and NB (PLL B).
    /// Parameter 3 is a 20-bit number representing the denominator for the fractional part of the
    /// PLLA/PLLB Feedback Multisynth divider.
    /// Addresses:
    ///     PLL A: 26d/1Ah, 27d/1Bh, 31d/1Fh[7:4]
    ///     PLL B: 34d/22h, 35d/23h, 39d/27h[7:4]
    /// Documentation: AN619, Skyworks Solutions Inc.
    /// </summary>
    public class MultisynthNParameter3Register : DeviceRegisters20BitsParameter
    {
        /// <summary>
        /// Creates a new instance of the MultisynthNParameter3Register class for the specified PLL component.
        /// </summary>
        /// <param name="pll">Target PLL</param>
        public MultisynthNParameter3Register(Pll pll)
        : base(pll == Pll.A ? Address.MultisynthNAParameters_P3_19_16_P2_19_16 : Address.MultisynthNBParameters_P3_19_16_P2_19_16,
               BitsLocation.Bits_7_4,
               pll == Pll.A ? Address.MultisynthNAParameters_P3_15_8 : Address.MultisynthNBParameters_P3_15_8,
               pll == Pll.A ? Address.MultisynthNAParameters_P3_7_0 : Address.MultisynthNBParameters_P3_7_0)
        {
            if (!Enum.IsDefined(typeof(Pll), pll))
            {
                throw new ArgumentException($"Invalid PLL ({pll})");
            }
        }
    }
}
