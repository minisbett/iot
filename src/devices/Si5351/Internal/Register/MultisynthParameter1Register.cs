// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Iot.Device.Si5351.Common;

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Represents the registers of Parameter 1 of the Multisynth 0..5
    /// Parameter 1 is a 18-bit number representing the integer part of the Multisynth 0..5 divider.
    /// Addresses:
    ///     MS0: 45d/2Dh, 46d/2Eh, 44d/2Ch[1:0]
    ///     MS1: 53d/35h, 54d/36h, 52d/34h[1:0]
    ///     MS2: 61d/3Dh, 62d/3Eh, 60d/3Ch[1:0]
    ///     MS3: 59d/45h, 60d/46h, 68d/44h[1:0]
    ///     MS4: 77d/4Dh, 78d/4Eh, 76d/4Ch[1:0]
    ///     MS5: 85d/55h, 86d/56h, 84d/54h[1:0]
    /// Documentation: AN619, Skyworks Solutions Inc.
    /// The memory layout for the Multisynth 0 to 5 is a sequence of consecutive register sets
    /// (P1 to P3 and auxiliary registers), each consisting of 8 bytes, starting from the base address.
    /// Therefore the specific addresses are calculated by multiplying the size with the number of the MS.
    /// </summary>
    public class MultisynthParameter1Register : DeviceRegister18BitsParameter
    {
        /// <summary>
        /// Creates a new instance of the Parameter 1 registers for the specified Multisynth circuit.
        /// </summary>
        /// <param name="multisynth">Target Multisynth (0 to 5)</param>
        public MultisynthParameter1Register(MultiSynth multisynth)
        : base((Address)((int)Address.MultisynthXParametersBase_R_4_P1_17_16 + (int)(multisynth) * 8),
               (Address)((int)Address.MultisynthXParametersBase_P1_15_8 + (int)(multisynth) * 8),
               (Address)((int)Address.MultisynthXParametersBase_P1_7_0 + (int)(multisynth) * 8))
        {
            if (!Enum.IsDefined(typeof(MultiSynth), multisynth) || multisynth == MultiSynth.MS6 || multisynth == MultiSynth.MS7)
            {
                throw new ArgumentException($"Invaliud Multisynth ({multisynth})");
            }
        }
    }
}
