// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Si5351.Internal;
using Iot.Device.Si5351.Internal.Infrastructure;
using Iot.Device.Si5351.Internal.Register;
using Iot.Device.Si5351.Tests;
using Xunit;

namespace Iot.Device.Si5351.Internal.Register.Tests
{
    public class FanoutEnableRegisterTest
    {
        [Theory]
        [InlineData(0b0000_0000, false, false, false)]
        [InlineData(0b1000_0000, true, false, false)]
        [InlineData(0b0100_0000, false, true, false)]
        [InlineData(0b0001_0000, false, false, true)]
        // check whether reserved bits are ignored
        [InlineData(0b0000_0101, false, false, false)]
        [InlineData(0b0010_1010, false, false, false)]
        [InlineData(0b1111_0101, true, true, true)]
        [InlineData(0b1111_1010, true, true, true)]
        public void Read(byte data, bool clkinFanout, bool xoFanout, bool msFanout)
        {
            var testDevice = new I2cTestDevice();
            I2cInterface testBus = new(testDevice);
            DeviceRegister.SetBus(testBus);
            testDevice.DataToRead.Enqueue(data);

            var reg = new FanoutEnableRegister();
            reg.Read();

            Assert.Equal((byte)Address.FanoutEnable, testDevice.DataWritten.Dequeue());

            Assert.Equal(clkinFanout, reg.ClkinFanoutEn);
            Assert.Equal(xoFanout, reg.XoFanoutEn);
            Assert.Equal(msFanout, reg.MsFanoutEn);
        }

        [Theory]
        [InlineData(false, false, false, 0b0000_0000, 0b0000_0000)]
        [InlineData(true, false, false, 0b0000_0000, 0b1000_0000)]
        [InlineData(false, true, false, 0b0000_0000, 0b0100_0000)]
        [InlineData(false, false, true, 0b0000_0000, 0b0001_0000)]
        // check if other bits are preserved
        [InlineData(false, false, false, 0b0000_0101, 0b0000_0101)]
        [InlineData(false, false, false, 0b0010_1010, 0b0010_1010)]
        [InlineData(true, true, true, 0b0011_0101, 0b1111_0101)]
        [InlineData(true, true, true, 0b0000_1010, 0b1101_1010)]
        public void Write(bool clkinFanout, bool xoFanout, bool msFanout, byte dataBefore, byte dataAfter)
        {
            var testDevice = new I2cTestDevice();
            I2cInterface testBus = new(testDevice);
            DeviceRegister.SetBus(testBus);

            var reg = new FanoutEnableRegister();
            reg.ClkinFanoutEn = clkinFanout;
            reg.XoFanoutEn = xoFanout;
            reg.MsFanoutEn = msFanout;

            testDevice.DataToRead.Enqueue(dataBefore);
            reg.Write();

            var addrReadFrom = testDevice.DataWritten.Dequeue();
            Assert.Equal((byte)Address.FanoutEnable, addrReadFrom);
            var addrWrittenTo = testDevice.DataWritten.Dequeue();
            Assert.Equal((byte)Address.FanoutEnable, addrWrittenTo);
            var dataWritten = testDevice.DataWritten.Dequeue();
            Assert.Equal(dataAfter, dataWritten);
        }
    }
}
