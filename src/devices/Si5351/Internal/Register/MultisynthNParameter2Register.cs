// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Iot.Device.Si5351.Common;

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Represents the (multi-)register of Parameter 2 of the Multisynth NA (PLL A) and NB (PLL B).
    /// Parameter 2 is a 20-bit number representing the numerator for the fractional part of the
    /// PLLA/PLLB Feedback Multisynth divider.
    /// Addresses:
    ///     PLL A: 31d/1Fh[3:0], 32d/20h, 33d/31h
    ///     PLL B: 39d/27h[3:0], 40d/28h, 41d/29h
    /// Documentation: AN619, Skyworks Solutions Inc.
    /// </summary>
    public class MultisynthNParameter2Register : DeviceRegisters20BitsParameter
    {
        /// <summary>
        /// Creates a new instance of the MultisynthNParameter2Register class for the specified PLL component.
        /// </summary>
        /// <param name="pll">Target PLL</param>
        public MultisynthNParameter2Register(Pll pll)
        : base(pll == Pll.A ? Address.MultisynthNAParameters_P3_19_16_P2_19_16 : Address.MultisynthNBParameters_P3_19_16_P2_19_16,
               BitsLocation.Bits_3_0,
               pll == Pll.A ? Address.MultisynthNAParameters_P2_15_8 : Address.MultisynthNBParameters_P2_15_8,
               pll == Pll.A ? Address.MultisynthNAParameters_P2_7_0 : Address.MultisynthNBParameters_P2_7_0)
        {
            if (!Enum.IsDefined(typeof(Pll), pll))
            {
                throw new ArgumentException($"Invalid PLL ({pll})");
            }
        }
    }
}
