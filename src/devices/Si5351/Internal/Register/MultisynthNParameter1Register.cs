// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using Iot.Device.Si5351.Common;

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Represents the (multi-)register of Parameter 1 of the Multisynth NA (PLL A) and NB (PLL B).
    /// Parameter 1 is a 18-bit number representing the integer part of the PLLA/PLLB Feedback Multisynth divider.
    /// Addresses:
    ///     PLL A: 28d/1Ch[1:0], 29d/1Eh, 30d/1Fh
    ///     PLL B: 36d/22h[1:0], 37d/23h, 38d/24h
    /// Documentation: AN619, Skyworks Solutions Inc.
    /// </summary>
    public class MultisynthNParameter1Register : DeviceRegister18BitsParameter
    {
        /// <summary>
        /// Creates a new instance of the MultisynthNParameter1Register class for the specified PLL component.
        /// </summary>
        /// <param name="pll">Target PLL</param>
        public MultisynthNParameter1Register(Pll pll)
        : base(pll == Pll.A ? Address.MultisynthNAParameters_P1_17_16 : Address.MultisynthNBParameters_P1_17_16,
               pll == Pll.A ? Address.MultisynthNAParameters_P1_15_8 : Address.MultisynthNBParameters_P1_15_8,
               pll == Pll.A ? Address.MultisynthNAParameters_P1_7_0 : Address.MultisynthNBParameters_P1_7_0)
        {
            if (!Enum.IsDefined(typeof(Pll), pll))
            {
                throw new ArgumentException($"Invalid PLL ({pll})");
            }
        }
    }
}
