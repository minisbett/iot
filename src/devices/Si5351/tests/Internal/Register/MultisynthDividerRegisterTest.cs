// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Si5351.Common;
using Iot.Device.Si5351.Internal.Infrastructure;
using Iot.Device.Si5351.Tests;
using Xunit;

namespace Iot.Device.Si5351.Internal.Register.Tests
{
    public class MultisynthDividerRegisterTest
    {
        [Theory]
        [InlineData(0b0000_0000, OutputDivider.Div1, false, MultiSynth.MS0, Address.ClockOutputDivider_0_5)]
        [InlineData(0b0001_1100, OutputDivider.Div2, true, MultiSynth.MS1, Address.ClockOutputDivider_0_5 + 8)]
        [InlineData(0b0010_0000, OutputDivider.Div4, false, MultiSynth.MS2, Address.ClockOutputDivider_0_5 + 16)]
        [InlineData(0b0011_0000, OutputDivider.Div8, false, MultiSynth.MS3, Address.ClockOutputDivider_0_5 + 24)]
        [InlineData(0b0100_1100, OutputDivider.Div16, true, MultiSynth.MS4, Address.ClockOutputDivider_0_5 + 32)]
        [InlineData(0b0101_0000, OutputDivider.Div32, false, MultiSynth.MS5, Address.ClockOutputDivider_0_5 + 40)]
        [InlineData(0b0110_0000, OutputDivider.Div64, false, MultiSynth.MS0, Address.ClockOutputDivider_0_5)]
        [InlineData(0b0111_1100, OutputDivider.Div128, true, MultiSynth.MS0, Address.ClockOutputDivider_0_5)]
        [InlineData(0b0000_0000, OutputDivider.Div1, false, MultiSynth.MS6, Address.ClockOutputDivider_6_7)]
        [InlineData(0b0000_0001, OutputDivider.Div2, false, MultiSynth.MS6, Address.ClockOutputDivider_6_7)]
        [InlineData(0b0000_0010, OutputDivider.Div4, false, MultiSynth.MS6, Address.ClockOutputDivider_6_7)]
        [InlineData(0b0000_0011, OutputDivider.Div8, false, MultiSynth.MS6, Address.ClockOutputDivider_6_7)]
        [InlineData(0b0000_0100, OutputDivider.Div16, false, MultiSynth.MS6, Address.ClockOutputDivider_6_7)]
        [InlineData(0b0000_0101, OutputDivider.Div32, false, MultiSynth.MS6, Address.ClockOutputDivider_6_7)]
        [InlineData(0b0000_0110, OutputDivider.Div64, false, MultiSynth.MS6, Address.ClockOutputDivider_6_7)]
        [InlineData(0b0000_0111, OutputDivider.Div128, false, MultiSynth.MS6, Address.ClockOutputDivider_6_7)]
        [InlineData(0b0000_0000, OutputDivider.Div1, false, MultiSynth.MS7, Address.ClockOutputDivider_6_7)]
        [InlineData(0b0001_0000, OutputDivider.Div2, false, MultiSynth.MS7, Address.ClockOutputDivider_6_7)]
        [InlineData(0b0010_0000, OutputDivider.Div4, false, MultiSynth.MS7, Address.ClockOutputDivider_6_7)]
        [InlineData(0b0011_0000, OutputDivider.Div8, false, MultiSynth.MS7, Address.ClockOutputDivider_6_7)]
        [InlineData(0b0100_0000, OutputDivider.Div16, false, MultiSynth.MS7, Address.ClockOutputDivider_6_7)]
        [InlineData(0b0101_0000, OutputDivider.Div32, false, MultiSynth.MS7, Address.ClockOutputDivider_6_7)]
        [InlineData(0b0110_0000, OutputDivider.Div64, false, MultiSynth.MS7, Address.ClockOutputDivider_6_7)]
        [InlineData(0b0111_0000, OutputDivider.Div128, false, MultiSynth.MS7, Address.ClockOutputDivider_6_7)]
        // check whether reserved bits are ignored
        // [InlineData(0b0111_1100, OutputDivider.Div128, true, MultiSynth.MS7, Address.ClockOutputDivider_6_7 + 8)]
        public void Read(byte data, OutputDivider divider, bool divideBy4, MultiSynth multiSynth, Address address)
        {
            var testDevice = new I2cTestDevice();
            I2cInterface testBus = new(testDevice);
            DeviceRegister.SetBus(testBus);
            testDevice.DataToRead.Enqueue(data);

            var reg = new MultisynthOutputDividerRegister(multiSynth);
            reg.Read();

            Assert.Equal((byte)address, testDevice.DataWritten.Dequeue());

            Assert.Equal(divider, reg.Divider);
            Assert.Equal(divideBy4, reg.DivideBy4);
        }
    }
}
