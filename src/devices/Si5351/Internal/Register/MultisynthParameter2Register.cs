// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Iot.Device.Si5351.Common;

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Represents the registers of Parameter 2 of the Multisynth 0..5
    /// Parameter 2 is a 20-bit number representing the nominator for the fractional part of
    /// the Multisynth 0..5 divider.
    /// Addresses:
    ///     MS0: 48d/30h, 49d/31h, 47d/2Fh[3:0]
    ///     MS1: 56d/38h, 57d/39h, 55d/37h[3:0]
    ///     MS2: 64d/40h, 65d/41h, 63d/3Fh[3:0]
    ///     MS3: 72d/48h, 73d/49h, 71d/47h[3:0]
    ///     MS4: 80d/50h, 81d/51h, 79d/4Fh[3:0]
    ///     MS5: 88d/58h, 89d/59h, 87d/57h[3:0]
    /// Documentation: AN619, Skyworks Solutions Inc.
    /// The memory layout for the Multisynth 0 to 5 is a sequence of consecutive register sets
    /// (P1 to P3 and auxiliary registers), each consisting of 8 bytes, starting from the base address.
    /// Therefore the specific addresses are calculated by multiplying the size with the number of the MS.
    /// </summary>
    public class MultisynthParameter2Register : DeviceRegisters20BitsParameter
    {
        /// <summary>
        /// Creates a new instance of the Parameter 2 registers for the specified Multisynth circuit.
        /// </summary>
        /// <param name="multisynth">Target Multisynth (0 to 5)</param>
        public MultisynthParameter2Register(MultiSynth multisynth)
            : base((Address)((int)Address.MultisyntXParametersBase_P3_19_16_P2_19_16 + (int)(multisynth) * 8),
                    BitsLocation.Bits_3_0,
                    (Address)((int)Address.MultisynthXParametersBase_P2_15_8 + (int)(multisynth) * 8),
                    (Address)((int)Address.MultisynthXParametersBase_P2_7_0 + (int)(multisynth) * 8))
        {
            if (!Enum.IsDefined(typeof(MultiSynth), multisynth) || multisynth == MultiSynth.MS6 || multisynth == MultiSynth.MS7)
            {
                throw new ArgumentException($"Invaliud Multisynth ({multisynth})");
            }
        }
    }
}
