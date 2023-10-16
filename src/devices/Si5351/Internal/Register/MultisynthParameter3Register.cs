// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Iot.Device.Si5351.Common;

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Represents the registers of Parameter 3 of the Multisynth 0..5
    /// Parameter 3 is a 20-bit number representing the denominator for the fractional part of
    /// the Multisynth 0..5 divider.
    /// Addresses:
    ///     MS0: 42d/2Ah, 43d/2Bh, 47d/2Fh[7:4]
    ///     MS1: 50d/32h, 51d/33h, 55d/37h[7:4]
    ///     MS2: 58d/3Ah, 59d/3Bh, 63d/3Fh[7:4]
    ///     MS3: 66d/42h, 67d/43h, 71d/47h[7:4]
    ///     MS4: 74d/4Ah, 75d/4Bh, 79d/4Fh[7:4]
    ///     MS5: 82d/52h, 83d/53h, 87d/57h[7:4]
    /// Documentation: AN619, Skyworks Solutions Inc.
    /// The memory layout for the Multisynth 0 to 5 is a sequence of consecutive register sets
    /// (P1 to P3 and auxiliary registers), each consisting of 8 bytes, starting from the base address.
    /// Therefore the specific addresses are calculated by multiplying the size with the number of the MS.
    /// </summary>
    public class MultisynthParameter3Register : DeviceRegisters20BitsParameter
    {
        /// <summary>
        /// Creates a new instance of the Parameter 3 registers for the specified Multisynth circuit.
        /// </summary>
        /// <param name="multisynth">Target Multisynth (0 to 5)</param>
        public MultisynthParameter3Register(MultiSynth multisynth)
        : base((Address)((int)Address.MultisyntXParametersBase_P3_19_16_P2_19_16 + (int)(multisynth) * 8),
               BitsLocation.Bits_7_4,
               (Address)((int)Address.MultisynthXParametersBase_P3_15_8 + (int)(multisynth) * 8),
               (Address)((int)Address.MultisynthXParametersBase_P3_7_0 + (int)(multisynth) * 8))
        {
            if (!Enum.IsDefined(typeof(MultiSynth), multisynth) || multisynth == MultiSynth.MS6 || multisynth == MultiSynth.MS7)
            {
                throw new ArgumentException($"Invaliud Multisynth ({multisynth})");
            }
        }
    }
}
