// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using Iot.Device.Si5351.Common;

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Represents the Output Divider register for clocks 0:5 and 6:/7.
    /// Addresses:
    ///     Output divider clock 0: 44d/2Ch
    ///     Output divider clock 1: 52d/34h
    ///     Output divider clock 2: 60d/3Ch
    ///     Output divider clock 3: 68d/44h
    ///     Output divider clock 4: 76d/4Ch
    ///     Output divider clock 5: 84d/54h
    /// Documentation: AN619, Skyworks Solutions Inc.
    /// The output dividers for clock 0 to 5 are stored in registers starting from the address of the Output Divider register for Clock 0,
    /// with each subsequent register at intervals of 8 bytes. Each of these registers follows the same layout.
    /// In addition to the divider settings, these registers also contain a portion of another Multisynth parameter (P1 17:16), which is, however,
    /// ignored when reading and writing the divider parameters.
    ///
    /// Output dividers for Multisynth 6 and 7 are stored in a different layout, hence they are handled differently.
    /// Address:
    ///     Output dividier clock 6/7: 92d/5Ch
    ///
    /// </summary>
    public class MultisynthOutputDividerRegister : DeviceRegister
    {
        private readonly MultiSynth _multisynth;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultisynthOutputDividerRegister"/> class.
        /// </summary>
        /// <param name="multisynth">Target Multisynth (0 to 7)</param>
        public MultisynthOutputDividerRegister(MultiSynth multisynth)
            : base(
                multisynth switch
                {
                    >= MultiSynth.MS0 and <= MultiSynth.MS5 => (Address)((byte)Address.ClockOutputDivider_0_5 + (byte)multisynth * 8),
                    MultiSynth.MS6 or MultiSynth.MS7 => Address.ClockOutputDivider_6_7,
                    _ => throw new ArgumentException($"Invalid Multisynth ({multisynth})")
                })
        {
            _multisynth = multisynth;
        }

        /// <summary>
        /// Output divider of Multisynth
        /// </summary>
        public OutputDivider Divider { get; set; }

        /// <summary>
        /// Divide by 4 enabled for Multisynth (only Multisynth 0:5, otherwise always false)
        /// </summary>
        public bool DivideBy4 { get; set; } = false;

        /// <inheritdoc/>
        public override void Read()
        {
            byte data = ReadData().DataA;

            Divider = _multisynth switch
            {
                (>= MultiSynth.MS0 and <= MultiSynth.MS5) or MultiSynth.MS7 => (OutputDivider)((data & 0b0111_0000) >> 4),
                MultiSynth.MS6 => (OutputDivider)(data & 0b0000_0111),
                _ => throw new ArgumentException($"Invalid Multisynth ({_multisynth})")
            };

            // Clock 6 and 7 do not have the parameter "Divide by 4"
            if (!(_multisynth == MultiSynth.MS6 || _multisynth == MultiSynth.MS7))
            {
                DivideBy4 = (data & 0b0000_1100) != 0;
            }
            else
            {
                DivideBy4 = false;
            }
        }

        /// <inheritdoc/>
        public override void Write()
        {
            // Divider clock 0:5 - keep bits for MSx_P1[1:0] and reserved bit [7]
            // Divider clock 6:7 - keep reserved bits [7, 3]
            byte mask = _multisynth switch
            {
                >= MultiSynth.MS0 and <= MultiSynth.MS5 => 0b0111_1100,
                MultiSynth.MS6 or MultiSynth.MS7 => 0b0111_0111,
                _ => throw new NotImplementedException()
            };

            byte data = _multisynth switch
            {
                >= MultiSynth.MS0 and <= MultiSynth.MS5 => (byte)((byte)((byte)Divider << 4) | (byte)(DivideBy4 ? 0b0000_1100 : 0)),
                MultiSynth.MS6 => (byte)Divider,
                MultiSynth.MS7 => (byte)((byte)Divider << 4),
                _ => throw new NotImplementedException()
            };

            WriteDataPreserve(data, mask);
        }
    }
}
