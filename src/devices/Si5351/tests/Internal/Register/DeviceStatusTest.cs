// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Si5351.Internal.Infrastructure;
using Iot.Device.Si5351.Tests;
using Xunit;

namespace Iot.Device.Si5351.Internal.Register.Tests
{
    public class DeviceStatusTest
    {
        [Theory]
        [InlineData(0b0000_0000, false, false, false, false, false, DeviceRevision.A)]
        [InlineData(0b1000_0000, true, false, false, false, false, DeviceRevision.A)]
        [InlineData(0b0100_0000, false, true, false, false, false, DeviceRevision.A)]
        [InlineData(0b0010_0000, false, false, true, false, false, DeviceRevision.A)]
        [InlineData(0b0001_0000, false, false, false, true, false, DeviceRevision.A)]
        [InlineData(0b0000_1000, false, false, false, false, true, DeviceRevision.A)]
        [InlineData(0b0000_0001, false, false, false, false, false, DeviceRevision.B)]
        [InlineData(0b0000_0010, false, false, false, false, false, DeviceRevision.Reserved1)]
        [InlineData(0b0000_0011, false, false, false, false, false, DeviceRevision.Reserved2)]
        public void Read(byte data, bool sysInit, bool lolB, bool lolA, bool losClkin, bool losXtal, DeviceRevision revision)
        {
            var testDevice = new I2cTestDevice();
            I2cInterface testBus = new(testDevice);
            DeviceRegister.SetBus(testBus);
            testDevice.DataToRead.Enqueue(data);

            var reg = new DeviceStatusRegister();
            reg.Read();

            Assert.Equal(sysInit, reg.SYS_INIT);
            Assert.Equal(lolB, reg.LOL_B);
            Assert.Equal(lolA, reg.LOL_A);
            Assert.Equal(losClkin, reg.LOS_CLKIN);
            Assert.Equal(losXtal, reg.LOS_XTAL);
            Assert.Equal(revision, reg.REVID);
        }
    }
}
