// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Iot.Device.Si5351.Common;
using Iot.Device.Si5351.Internal.Common;

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Base class for CLKx Control registers covering parameters applicable to all registers of this type.
    /// Note: some registers vary regarding the valid options for OutputClockInputSource depending on the
    ///       clock output number. This class does not check the validity if setting the CLKx_SRC property.
    /// </summary>
    public abstract class ClockControlRegister : DeviceRegister
    {
        private readonly byte _number = 0;

        /// <summary>
        /// Initializes a new instance of a ClockControlRegiser class for a specific clock number.
        /// </summary>
        /// <param name="number">Clock number 0:7</param>
        public ClockControlRegister(byte number)
            : base(Address.CLKx_Control + number)
        {
            if (number < 0 || number > 7)
            {
                throw new ArgumentException($"Invalid clock number ({number})");
            }

            _number = number;
        }

        /// <summary>
        /// Power down state of clock 0:7 output driver.
        /// </summary>
        public PowerState CLK_PDN { get; set; } = PowerState.PoweredUp;

        /// <summary>
        /// Source select of MultiSynth X.
        /// </summary>
        public MultiSynthSource MS_SRC { get; set; } = MultiSynthSource.PllA;

        /// <summary>
        /// Clock output inverted (true, if inverted).
        /// </summary>
        public ClockInversionState CLK_INV { get; set; } = ClockInversionState.NotInverted;

        /// <summary>
        /// Input source of clock output.
        /// </summary>
        public OutputClockInputSource CLK_SRC { get; set; } = OutputClockInputSource.XTAL;

        /// <summary>
        /// Drive strength of clock output.
        /// </summary>
        public DriveStrength CLK_IDRV { get; set; } = DriveStrength.Strength2mA;

        /// <summary>
        /// Reads the common register data and populates respective properties.
        /// This is to be called from a specific register implementation.
        /// </summary>
        /// <returns>Data read from register</returns>
        protected byte ReadCommon()
        {
            byte data = ReadData().DataA;
            CLK_PDN = (data & 0b1000_0000) == 1 ? PowerState.PoweredDown : PowerState.PoweredUp;
            MS_SRC = (data & 0b0010_0000) == 1 ? MultiSynthSource.PllB_Vcxo : MultiSynthSource.PllA;
            CLK_INV = (data & 0b0001_0000) == 1 ? ClockInversionState.Inverted : ClockInversionState.NotInverted;
            CLK_SRC = (data & 0b0000_1100, _number) switch
            {
                (0b0000_0000, _) => OutputClockInputSource.XTAL,
                (0b0000_0100, _) => OutputClockInputSource.CLKIN,
                (0b0000_1000, 0 or 4) => OutputClockInputSource.Reserved,
                (0b0000_1000, 1 and <= 3) => OutputClockInputSource.MS0,
                (0b0000_1000, >= 5 and <= 7) => OutputClockInputSource.MS4,
                (0b0000_1100, _) => OutputClockInputSource.MSx,
                 _ => throw new Exception("Invalid data or number for clock source")
            };
            CLK_IDRV = (DriveStrength)(data & 0b0000_0011);
            return data;
        }

        /// <summary>
        /// Combines the common and specific register data and writes it to the register.
        /// This is to be called from a specific register implementation.
        /// Important: the parameter 'data' must be populated with the specific bits only.
        ///            All other bits must be set to 0.
        /// </summary>
        /// <param name="data">Data from specific implementation</param>
        protected void WriteCommon(byte data)
        {
            data |= (byte)(CLK_PDN == PowerState.PoweredDown ? 0b1000_0000 : 0);
            data |= (byte)(MS_SRC == MultiSynthSource.PllB_Vcxo ? 0b0010_0000 : 0);
            data |= (byte)(CLK_INV == ClockInversionState.Inverted ? 0b0001_0000 : 0);
            data |= CLK_SRC switch
            {
                OutputClockInputSource.XTAL => 0b0000_0000,
                OutputClockInputSource.CLKIN => 0b0000_1000,
                OutputClockInputSource.Reserved => 0b0000_0000,
                OutputClockInputSource.MSx or OutputClockInputSource.MS0 or OutputClockInputSource.MS4 => 0b0000_110000,
                _ => throw new Exception("Invalid value for clock source")
            };
            data |= CLK_IDRV switch
            {
                DriveStrength.Strength2mA => 0b00,
                DriveStrength.Strength4mA => 0b01,
                DriveStrength.Strength6mA => 0b10,
                DriveStrength.Strength8mA => 0b11,
                _ => throw new Exception("Invalid value for drive strength")
            };

            WriteData(data);
        }
    }
}
