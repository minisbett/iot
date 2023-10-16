// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Si5351.Common;
using Iot.Device.Si5351.Internal;
using Iot.Device.Si5351.Internal.Infrastructure;
using Iot.Device.Si5351.Internal.Register;
using Iot.Device.Si5351.Tests;
using Xunit;

namespace Iot.Device.Si5351.Internal.Register.Tests
{
    public class MultisynthNParameter1RegisterTest
    {
        [Theory]
        [InlineData(0b00, 0b0000_0000, 0b0000_0000, Pll.A, Address.MultisynthNAParameters_P1_17_16, Address.MultisynthNAParameters_P1_15_8, Address.MultisynthNAParameters_P1_7_0, 0)]
        [InlineData(0b00, 0b0011_1111, 0b1111_1111, Pll.A, Address.MultisynthNAParameters_P1_17_16, Address.MultisynthNAParameters_P1_15_8, Address.MultisynthNAParameters_P1_7_0, 16383)]
        [InlineData(0b11, 0b1111_1111, 0b1111_1111, Pll.A, Address.MultisynthNAParameters_P1_17_16, Address.MultisynthNAParameters_P1_15_8, Address.MultisynthNAParameters_P1_7_0, 262143)]
        [InlineData(0b00, 0b0010_1000, 0b0100_0010, Pll.A, Address.MultisynthNAParameters_P1_17_16, Address.MultisynthNAParameters_P1_15_8, Address.MultisynthNAParameters_P1_7_0, 10306)]
        [InlineData(0b00, 0b0000_0000, 0b0000_0000, Pll.B, Address.MultisynthNBParameters_P1_17_16, Address.MultisynthNBParameters_P1_15_8, Address.MultisynthNBParameters_P1_7_0, 0)]
        [InlineData(0b00, 0b0011_1111, 0b1111_1111, Pll.B, Address.MultisynthNBParameters_P1_17_16, Address.MultisynthNBParameters_P1_15_8, Address.MultisynthNBParameters_P1_7_0, 16383)]
        [InlineData(0b11, 0b1111_1111, 0b1111_1111, Pll.B, Address.MultisynthNBParameters_P1_17_16, Address.MultisynthNBParameters_P1_15_8, Address.MultisynthNBParameters_P1_7_0, 262143)]
        [InlineData(0b00, 0b0001_0100, 0b0001_0001, Pll.B, Address.MultisynthNBParameters_P1_17_16, Address.MultisynthNBParameters_P1_15_8, Address.MultisynthNBParameters_P1_7_0, 5137)]
        public void Read(byte bits17_16, byte bits15_8, byte bits7_0, Pll pll, Address addr17_16, Address addr15_8, Address addr7_0, int expectedValue)
        {
            var testDevice = new I2cTestDevice();
            I2cInterface testBus = new(testDevice);
            DeviceRegister.SetBus(testBus);
            testDevice.DataToRead.Enqueue((byte)bits17_16);
            testDevice.DataToRead.Enqueue((byte)bits15_8);
            testDevice.DataToRead.Enqueue((byte)bits7_0);

            var reg = new MultisynthNParameter1Register(pll);
            reg.Read();

            // check addresses
            Assert.Equal((byte)addr17_16, testDevice.DataWritten.Dequeue());
            Assert.Equal((byte)addr15_8, testDevice.DataWritten.Dequeue());
            Assert.Equal((byte)addr7_0, testDevice.DataWritten.Dequeue());

            Assert.Equal(expectedValue, reg.Parameter);
        }
    }
}
