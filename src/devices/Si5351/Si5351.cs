// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Device.I2c;
using System.Threading;
using Iot.Device.Si5351.Internal;
namespace Iot.Device.Si5351
{
    /// <summary>
    /// Binding for the Si5351 device family.
    /// </summary>
    public class Si5351Device : IDisposable
    {
        /// <summary>
        /// The device slave base address (0x60) consist of a 6-bit fixed address plus a user selectable LSB bit.
        /// The LSB bit is selectable as 0 or 1 using the optional A0 pin which is useful for applications that
        /// require more than one Si5351 on a single I2C bus.
        /// If a part does not have the A0 pin, the default address is 0x60 with the A0 bit set to 0.
        /// </summary>
        public static int DefaultI2cAddress = 0x60;
        private readonly DeviceOutputs _outputs;
        private readonly DeviceVersion _version;
        private bool _initialized = false;
        private Internal.Infrastructure.I2cInterface _i2cBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="Si5351"/> binding and
        /// assembles it for the specified device version (A/B/C) and number of outputs.
        /// </summary>
        public Si5351Device(I2cDevice i2cDevice, DeviceVersion version, DeviceOutputs outputs)
        {
            I2cDevice dev = i2cDevice ?? throw new ArgumentNullException(nameof(i2cDevice));
            _i2cBus = new(dev);

            // verify that the parameters specify a valid device
            if (version == DeviceVersion.A && (outputs == DeviceOutputs.FourOutputs || outputs == DeviceOutputs.EightOutputs))
            {
                throw new ArgumentException($"The combination of device version ({version}) and number of outputs ({outputs}) is not valid.");
            }

            _version = version;
            _outputs = outputs;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_i2cBus != null)
            {
                _i2cBus?.Dispose();
                _i2cBus = null!;
            }
        }

        /// <summary>
        /// t.b.d.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void Init()
        {
            if (_initialized)
            {
                throw new InvalidOperationException("Device is already initialized");
            }

            /* Disable all outputs setting CLKx_DIS high */
            // ASSERT_STATUS(write8(SI5351_REGISTER_3_OUTPUT_ENABLE_CONTROL, 0xFF));
            _initialized = true;
        }

        // private DeviceStatus GetSetting(Setting setting)
        // {
        //     SettingAddress set = SettingMap.GetRegisterAddress(setting);
        //     int len = ((set.Length - 1) / sizeof(byte)) + 1;
        //     Span<byte> buf = stackalloc byte[len];
        //     _i2cBus.Read(0x0, buf);
        // }

        /// <summary>
        /// Just for testings
        /// </summary>
        public void Test()
        {
            // clear interrupt status sticky bits
            // _i2cBus.Write(0x01, 0x00);

            // disable outputs by setting CLKx_OEB to high in Output Enable Control register (0x03)
            // _i2cBus.Write(0x03, 0xff);

            // power-down all output drivers by setting CLKx_PDN to high (CLKx Control Registers 0x10 - 0x17)
            // _i2cBus.Write(0x10, 0x80);
            // _i2cBus.Write(0x11, 0x80);
            // _i2cBus.Write(0x12, 0x80);
            // _i2cBus.Write(0x13, 0x80);
            // _i2cBus.Write(0x14, 0x80);
            // _i2cBus.Write(0x15, 0x80);
            // _i2cBus.Write(0x16, 0x80);
            // _i2cBus.Write(0x17, 0x80);

            // set all interrupt masks to prevent INTR pin from going low (availability depends on actual device variant)
            // _i2cBus.Write(0x02, 0xf8);

            // disable spread spectrum
            // _i2cBus.Write(0x00, 0x00);

            // set internal load capacitance for crystal (0xB7 / 183)
            // _i2cBus.Write(0xb7, 0b11010010);

            // set PLL input source (register 15 / 0x0f)
            // _i2cBus.Write(0x0f, 0x00);
            // _i2cBus.Write(0x02, 0b0101_0011);
            // _i2cBus.Write(0x04, 0b0010_0000);
            // _i2cBus.Write(0x07, 0b0000_0000);
            // _i2cBus.Write(0x0f, 0b0000_0000);
            // _i2cBus.Write(0x10, 0b0000_1111);
            // _i2cBus.Write(0x11, 0b1000_1100);
            // _i2cBus.Write(0x12, 0b1000_1100);
            // _i2cBus.Write(0x13, 0b1000_1100);
            // _i2cBus.Write(0x14, 0b1000_1100);
            // _i2cBus.Write(0x15, 0b1000_1100);
            // _i2cBus.Write(0x16, 0b1000_1100);
            // _i2cBus.Write(0x17, 0b1000_1100);

            // _i2cBus.Write(0x1a, 0b0000_0000);
            // _i2cBus.Write(0x1b, 0b0000_0001);

            // _i2cBus.Write(0x1c, 0b0000_0000);
            // _i2cBus.Write(0x1d, 0b0001_0000);
            // _i2cBus.Write(0x1e, 0b0000_0000);
            // _i2cBus.Write(0x1f, 0b0000_0000);
            // _i2cBus.Write(0x20, 0b0000_0000);
            // _i2cBus.Write(0x21, 0b0000_0000);

            // _i2cBus.Write(0x2a, 0b0000_0000);
            // _i2cBus.Write(0x2b, 0b0000_0100);
            // _i2cBus.Write(0x2c, 0b0110_0010);
            // _i2cBus.Write(0x2d, 0b1011_1101);
            // _i2cBus.Write(0x2e, 0b0010_0000);
            // _i2cBus.Write(0x2f, 0b0000_0000);
            // _i2cBus.Write(0x30, 0b0000_0000);
            // _i2cBus.Write(0x31, 0b0000_0000);

            // _i2cBus.Write(90, 00);
            // _i2cBus.Write(91, 00);

            // _i2cBus.Write(149, 0);
            // _i2cBus.Write(150, 0);
            // _i2cBus.Write(151, 0);
            // _i2cBus.Write(152, 0);
            // _i2cBus.Write(153, 0);
            // _i2cBus.Write(154, 0);
            // _i2cBus.Write(155, 0);

            // _i2cBus.Write(162, 0);
            // _i2cBus.Write(163, 0);
            // _i2cBus.Write(164, 0);
            // _i2cBus.Write(165, 0);

            // _i2cBus.Write(183, 0xd2);

            // _i2cBus.Write(177, 0xac);

            // // enable CLK0 output
            // _i2cBus.Write(0x03, 0b00000000);

            // _i2cBus.Write(0x0, 0x11);
            // _i2cBus.Write(0x1, 0xf9);
            // _i2cBus.Write(0x2, 0x3);
            // _i2cBus.Write(0x3, 0x0);
            // _i2cBus.Write(0x3, 0xff);
            // _i2cBus.Write(0x4, 0x0);
            // _i2cBus.Write(0x5, 0xff);
            // _i2cBus.Write(0x6, 0x0);

            // _i2cBus.Write(0x7, 0x0);
            // _i2cBus.Write(0x8, 0x0);
            // _i2cBus.Write(0x9, 0x0);
            // _i2cBus.Write(0xa, 0x0);
            // _i2cBus.Write(0xb, 0x0);
            // _i2cBus.Write(0xc, 0x0);
            // _i2cBus.Write(0xd, 0x0);
            // _i2cBus.Write(0xe, 0x90);
            // _i2cBus.Write(0xf, 0x0);
            // _i2cBus.Write(0x10, 0x4f);
            // _i2cBus.Write(0x11, 0x2f);
            // _i2cBus.Write(0x12, 0x6f);
            // _i2cBus.Write(0x13, 0x80);
            // _i2cBus.Write(0x14, 0x80);
            // _i2cBus.Write(0x15, 0x80);
            // _i2cBus.Write(0x16, 0x80);
            // _i2cBus.Write(0x17, 0x80);
            // _i2cBus.Write(0x18, 0x0);
            // _i2cBus.Write(0x19, 0x0);
            // _i2cBus.Write(0x1a, 0x0);
            // _i2cBus.Write(0x1b, 0x1);
            // _i2cBus.Write(0x1c, 0x0);
            // _i2cBus.Write(0x1d, 0x10);
            // _i2cBus.Write(0x1e, 0x0);
            // _i2cBus.Write(0x1f, 0x0);
            // _i2cBus.Write(0x20, 0x0);
            // _i2cBus.Write(0x21, 0x0);
            // _i2cBus.Write(0x22, 0x0);
            // _i2cBus.Write(0x23, 0x3);
            // _i2cBus.Write(0x24, 0x0);
            // _i2cBus.Write(0x25, 0xa);
            // _i2cBus.Write(0x26, 0x55);
            // _i2cBus.Write(0x27, 0x0);
            // _i2cBus.Write(0x28, 0x0);
            // _i2cBus.Write(0x29, 0x1);
            // _i2cBus.Write(0x2a, 0x0);
            // _i2cBus.Write(0x2b, 0x1);
            // _i2cBus.Write(0x2c, 0x0);
            // _i2cBus.Write(0x2d, 0x2);
            // _i2cBus.Write(0x2e, 0x0);
            // _i2cBus.Write(0x2f, 0x0);
            // _i2cBus.Write(0x30, 0x0);
            // _i2cBus.Write(0x31, 0x0);
            // _i2cBus.Write(0x32, 0x0);
            // _i2cBus.Write(0x33, 0x2);
            // _i2cBus.Write(0x34, 0x0);
            // _i2cBus.Write(0x35, 0x14);
            // _i2cBus.Write(0x36, 0xc0);
            // _i2cBus.Write(0x37, 0x0);
            // _i2cBus.Write(0x38, 0x0);
            // _i2cBus.Write(0x39, 0x0);
            // _i2cBus.Write(0x3a, 0x0);
            // _i2cBus.Write(0x3b, 0x1);
            // _i2cBus.Write(0x3c, 0x61);
            // _i2cBus.Write(0x3d, 0xc0);
            // _i2cBus.Write(0x3e, 0x0);
            // _i2cBus.Write(0x3f, 0x0);
            // _i2cBus.Write(0x40, 0x0);
            // _i2cBus.Write(0x41, 0x0);
            // _i2cBus.Write(0x42, 0x0);
            // _i2cBus.Write(0x43, 0x0);
            // _i2cBus.Write(0x44, 0x0);
            // _i2cBus.Write(0x45, 0x0);
            // _i2cBus.Write(0x46, 0x0);
            // _i2cBus.Write(0x47, 0x0);
            // _i2cBus.Write(0x48, 0x0);
            // _i2cBus.Write(0x49, 0x0);
            // _i2cBus.Write(0x4a, 0x0);
            // _i2cBus.Write(0x4b, 0x0);
            // _i2cBus.Write(0x4c, 0x0);
            // _i2cBus.Write(0x4d, 0x0);
            // _i2cBus.Write(0x4e, 0x0);
            // _i2cBus.Write(0x4f, 0x0);
            // _i2cBus.Write(0x50, 0x0);
            // _i2cBus.Write(0x51, 0x0);
            // _i2cBus.Write(0x52, 0x0);
            // _i2cBus.Write(0x53, 0x0);
            // _i2cBus.Write(0x54, 0x0);
            // _i2cBus.Write(0x55, 0x0);
            // _i2cBus.Write(0x56, 0x0);
            // _i2cBus.Write(0x57, 0x0);
            // _i2cBus.Write(0x58, 0x0);
            // _i2cBus.Write(0x59, 0x0);
            // _i2cBus.Write(0x5a, 0x0);
            // _i2cBus.Write(0x5b, 0x0);
            // _i2cBus.Write(0x5c, 0x0);
            // _i2cBus.Write(0x95, 0x0);
            // _i2cBus.Write(0x96, 0x0);
            // _i2cBus.Write(0x97, 0x0);
            // _i2cBus.Write(0x98, 0x0);
            // _i2cBus.Write(0x99, 0x0);
            // _i2cBus.Write(0x9a, 0x0);
            // _i2cBus.Write(0x9b, 0x0);
            // _i2cBus.Write(0x9c, 0x0);
            // _i2cBus.Write(0x9d, 0x0);
            // _i2cBus.Write(0x9e, 0x0);
            // _i2cBus.Write(0x9f, 0x0);
            // _i2cBus.Write(0xa0, 0x0);
            // _i2cBus.Write(0xa1, 0x0);
            // _i2cBus.Write(0xa2, 0x0);
            // _i2cBus.Write(0xa3, 0x0);
            // _i2cBus.Write(0xa4, 0x0);
            // _i2cBus.Write(0xa5, 0x0);
            // _i2cBus.Write(0xa6, 0x0);
            // _i2cBus.Write(0xa7, 0x0);
            // _i2cBus.Write(0xa8, 0x0);
            // _i2cBus.Write(0xa9, 0x0);
            // _i2cBus.Write(0xaa, 0x0);
            // _i2cBus.Write(0xb1, 0x0);
            // _i2cBus.Write(0xb7, 0xc0);
            // _i2cBus.Write(0xbb, 0x2);
            // _i2cBus.Write(0xbc, 0x0);

            // ----------------
            // Interrupt Status Mask
            // - all interrupt sources masked
            _i2cBus.Write(0x02, 0b0000_0000);

            // PLL Input Source
            // - clock input divider: 1
            // - XTAL input for PLL A and B
            _i2cBus.Write(0x0f, 0b0000_0000);

            // CLK0 Control (0x10/16)
            // - power up
            // - fractional division mode
            // - PLL A as the source for MultiSynth0
            // - Output is not inverted
            // - MultiSynth0 is source for CLK0
            // - Drive strength of CLK 0 is 8 mA
            _i2cBus.Write(0x10, 0b0000_1111);

            // CLK1-7 Control (0x11-0x17/17-23)
            // - power down
            _i2cBus.Write(0x11, 0b1000_0000);
            _i2cBus.Write(0x12, 0b1000_0000);
            _i2cBus.Write(0x13, 0b1000_0000);
            _i2cBus.Write(0x14, 0b1000_0000);
            _i2cBus.Write(0x15, 0b1000_0000);
            _i2cBus.Write(0x16, 0b1000_0000);
            _i2cBus.Write(0x17, 0b1000_0000);

            // CLK3-0 Disable State (0x18/24)
            // - CLK3-0 is set to a LOW state when disabled
            _i2cBus.Write(0x18, 0x00);

            // CLK7-4 Disable State (0x19/25)
            // - CLK7-4 is set to a LOW state when disabled
            _i2cBus.Write(0x19, 0x00);

            // Stage 1
            // Parameter 1 = 4096
            // Parameter 2 = 0
            // Parameter 3 = 1
            // Internal PLLA VCO = 900 MHz

            // Multisynth NA Parameters - Parameter 3 [15:0] (0x1a/26 - 0x1b/27)
            _i2cBus.Write(0x1a, 0b0000_0000);
            _i2cBus.Write(0x1b, 0b0000_0001);

            // Multisynth NA Parameters - Parameter 1 (0x1c/28 - 0x1e/30)
            _i2cBus.Write(0x1c, 0b0000_0000);
            _i2cBus.Write(0x1d, 0b0001_0000);
            _i2cBus.Write(0x1e, 0b0000_0000);

            // Multisynth NA Parameters - Parameter 3 [19:16], Parameter 2 [19:16] (0x1f/31)
            _i2cBus.Write(0x1f, 0b0000_0000);

            // Multisynth NA Parameters - Parameter 2 [15:0] (0x20/32 -0x21/33)
            _i2cBus.Write(0x20, 0b0000_0000);
            _i2cBus.Write(0x21, 0b0000_0000);

            // Stage 2
            // - Parameter 1 = 179.488
            // - Parameter 2 = 0
            // - Parameter 3 = 4

            // MultiSynth0 Parameters - Parameter 3 [15:0] (0x2a/42 - 0x2b/43)
            _i2cBus.Write(0x2a, 0b0000_0000);
            _i2cBus.Write(0x2b, 0b0000_0100);

            // MultiSynth0 Parameters - R0, MS0, Parameter 1 [17:16] (0x2c/44)
            // - R = 64
            // - MS0 = divide by number other than 4
            // - Parameter 1 [17:16] = 0x10;
            _i2cBus.Write(0x2c, 0b0110_0010);

            // MultiSynth0 Parameters - Parameter 1 [15:0] (0x2d/45 - 0x2e/46)
            _i2cBus.Write(0x2d, 0b1011_1101);
            _i2cBus.Write(0x2e, 0b0010_0000);

            // MultiSynth0 Parameters - Parameter 2 [19:16], Parameter 3 [19:16] (0x2f/47)
            _i2cBus.Write(0x2f, 0b0000_0000);

            // MultiSynth0 Parameters - Parameter 2 [15:0] (0x30/48 - 0x31/49)
            _i2cBus.Write(0x30, 0b0000_0000);
            _i2cBus.Write(0x31, 0b0000_0000);

            _i2cBus.Write(90, 00);
            _i2cBus.Write(91, 00);

            _i2cBus.Write(149, 0);
            _i2cBus.Write(150, 0);
            _i2cBus.Write(151, 0);
            _i2cBus.Write(152, 0);
            _i2cBus.Write(153, 0);
            _i2cBus.Write(154, 0);
            _i2cBus.Write(155, 0);

            _i2cBus.Write(162, 0);
            _i2cBus.Write(163, 0);
            _i2cBus.Write(164, 0);
            _i2cBus.Write(165, 0);

            _i2cBus.Write(183, 0xd2);

            _i2cBus.Write(177, 0xac);

            // _i2cBus.Write(187, 64);

            // enable CLK0 output
            _i2cBus.Write(0x03, 0b00000000);
            // ----------

            // Span<byte> status = stackalloc byte[1];
            // Span<byte> stickyBits = stackalloc byte[1];
            // while (true)
            // {
            //     Thread.Sleep(1000);
            //     _i2cBus.Write(0x2c, 0b0110_0010);
            //     Thread.Sleep(1000);
            //     _i2cBus.Write(0x2c, 0b0111_0010);
            // }
        }
    }
}
