// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xunit;
using Iot.Device.Si5351;

namespace Iot.Device.Si5351.Tests
{
    public class Si5351Tests
    {
        [Theory]
        [InlineData(DeviceVersion.A, DeviceOutputs.ThreeOutputs, true)]
        [InlineData(DeviceVersion.A, DeviceOutputs.FourOutputs, false)]
        [InlineData(DeviceVersion.A, DeviceOutputs.EightOutputs, false)]
        [InlineData(DeviceVersion.B, DeviceOutputs.ThreeOutputs, true)]
        [InlineData(DeviceVersion.B, DeviceOutputs.FourOutputs, true)]
        [InlineData(DeviceVersion.B, DeviceOutputs.EightOutputs, true)]
        [InlineData(DeviceVersion.C, DeviceOutputs.ThreeOutputs, true)]
        [InlineData(DeviceVersion.C, DeviceOutputs.FourOutputs, true)]
        [InlineData(DeviceVersion.C, DeviceOutputs.EightOutputs, true)]
        public void Constructor_CheckValidDeviceVerification(DeviceVersion version, DeviceOutputs outputs, bool valid)
        {
            if (valid)
            {
                _ = new Si5351Device(new I2cTestDevice(), version, outputs);
            }
            else
            {
                Assert.Throws<ArgumentException>(() => new Si5351Device(new I2cTestDevice(), version, outputs));
            }
        }
    }
}
