// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using Iot.Device.Si5351.Common;

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Represents the registers of Parameter 1 of the Multisynth 6/7.
    /// The parameter has a size of 8 bits.
    /// Addresses:
    ///     Multisynth 6: 90d/5Ah
    ///     Multisynth 7: 91d/5Bh
    /// Documentation: AN619, Skyworks Solutions Inc.
    /// </summary>
    public class Multisynth67Parameter1Register : DeviceRegister
    {
        /// <summary>
        /// Creates a new instance of the Parameter 1 registers for the specified Multisynth 6/7 component.
        /// </summary>
        /// <param name="multisynth">Target Multisynth (6 or 7)</param>
        public Multisynth67Parameter1Register(MultiSynth multisynth)
            : base(multisynth switch
            {
                MultiSynth.MS6 => Address.Multisynth6Parameter1,
                MultiSynth.MS7 => Address.Multisynth7Parameter1,
                _ => throw new ArgumentException($"Invaliud Multisynth ({multisynth})")
            })
        {
        }

        /// <summary>
        /// Parameter 1 value.
        /// </summary>
        public byte Parameter { get; set; }

        /// <inheritdoc/>
        public override void Read()
        {
            Parameter = ReadData().DataA;
        }

        /// <inheritdoc/>
        public override void Write()
        {
            WriteData(Parameter);
        }
    }
}
