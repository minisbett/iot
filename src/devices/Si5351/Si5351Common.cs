// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Device.I2c;
using Iot.Device.Si5351.Internal;
using Iot.Device.Si5351.Internal.Register;

namespace Iot.Device.Si5351
{
    /// <summary>
    /// Binding for the Si5351 device family.
    /// </summary>
    public class Si5351Common : IDisposable
    {
        private readonly OutputEnableControlRegister _outputEnableControlRegister = new();
        private readonly OebPinEnableControlMaskRegister _oebPinEnableControlMaskRegister = new();
        private readonly ClockControlRegister0To5[] _clockControlRegisters;

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
        /// Gets the number of available outputs
        /// </summary>
        public int Outputs { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Si5351"/> binding and
        /// assembles it for the specified device version (A/B/C) and number of outputs.
        /// </summary>
        public Si5351Common(I2cDevice i2cDevice, DeviceVersion version, DeviceOutputs outputs)
        {
            I2cDevice dev = i2cDevice ?? throw new ArgumentNullException(nameof(i2cDevice));
            _i2cBus = new(dev);

            DeviceRegister.SetBus(_i2cBus);

            // verify that the parameters specify a valid device
            if (version == DeviceVersion.A && (outputs == DeviceOutputs.FourOutputs || outputs == DeviceOutputs.EightOutputs))
            {
                throw new ArgumentException($"The combination of device version ({version}) and number of outputs ({outputs}) is not valid.");
            }

            DeviceStatus status = GetDeviceStatus();
            if (status.Revision != DeviceRevision.B)
            {
                throw new NotImplementedException("The driver supports only revision B of Si5351A/B/C");
            }

            _version = version;
            _outputs = outputs;
            Outputs = (int)outputs;

            _clockControlRegisters = new ClockControlRegister0To5[(byte)outputs];
            for (byte i = 0; i < (byte)outputs; i++)
            {
                _clockControlRegisters[i] = new ClockControlRegister0To5(i);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            DeviceRegister.SetBus(null);

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
            _initialized = true;
        }

        /// <summary>
        /// Gets the current device status
        /// </summary>
        /// <returns>Device status</returns>
        public DeviceStatus GetDeviceStatus()
        {
            DeviceStatusRegister register = new();
            register.Read();
            return new DeviceStatus(register);
        }

        /// <summary>
        /// Clears all interrupt status flags
        /// </summary>
        public void ClearInterruptStatus()
        {
            InterruptStatusStickyRegister register = new();
            register.Write();
        }

        /// <summary>
        /// Gets the interrupt status flags
        /// </summary>
        /// <returns>Current interrupt status flags</returns>
        public InterruptStatus GetInterruptStatus()
        {
            InterruptStatusStickyRegister register = new();
            register.Read();
            return new InterruptStatus(register);
        }

        /// <summary>
        /// Gets the interrupt masks
        /// </summary>
        public InterruptMask GetInterruptMask()
        {
            InterruptStatusMaskRegister register = new();
            register.Read();
            return new InterruptMask(register);
        }

        /// <summary>
        /// Sets the interrupt masks
        /// </summary>
        public void SetInterruptMask(InterruptMask mask)
        {
            mask.ToRegister().Write();
        }

        /// <summary>
        /// Gets the enabled state of the provided output number.
        /// Note: the max. number of outputs depends on the respective device type.
        /// An exception is thrown if an invalid number is provided.
        /// </summary>
        /// <param name="number">Number of the output</param>
        /// <exception cref="ArgumentException">Invalid output number</exception>
        public bool GetOutputState(int number)
        {
            if (number < 0 || number >= (byte)_outputs)
            {
                throw new ArgumentException("Invalid output number", nameof(number));
            }

            _outputEnableControlRegister.Read();

            // Reverse state UM ES INTUITIVER ZU MACHEN
            return !_outputEnableControlRegister[number];
        }

        /// <summary>
        /// Enables an clock output of the device.
        /// Note: the max. number of outputs depends on the respective device type.
        /// An exception is thrown if an invalid number is provided.
        /// </summary>
        /// <param name="number">Number of the output to be enabled</param>
        /// <exception cref="ArgumentException">Invalid output number</exception>
        public void EnableOutput(int number)
        {
            EnableDisableOutput(number, false);
        }

        /// <summary>
        /// Disables an clock output of the device.
        /// </summary>
        /// <param name="number">Number of the output to be enabled</param>
        /// <exception cref="ArgumentException">Invalid output number</exception>
        public void DisableOutput(int number)
        {
            EnableDisableOutput(number, true);
        }

        /// <summary>
        /// t.b.d.
        /// </summary>
        /// <param name="number">t.b.d.</param>
        /// <returns></returns>
        public OutputConfiguration GetOutputConfiguration(byte number)
        {
            if (number < 0 || number >= (byte)_outputs)
            {
                throw new ArgumentException("Invalid output number", nameof(number));
            }

            _oebPinEnableControlMaskRegister.Read();
            OutputConfiguration configuration = new(!_oebPinEnableControlMaskRegister[number]);
            return configuration;
        }

        /// <summary>
        /// t.b.d.
        /// </summary>
        /// <param name="number">t.b.d.</param>
        /// <param name="configuration">t.b.d.</param>
        public void SetOutputConfigure(byte number, OutputConfiguration configuration)
        {
            if (number < 0 || number >= (byte)_outputs)
            {
                throw new ArgumentException("Invalid output number", nameof(number));
            }

            _oebPinEnableControlMaskRegister.Read();
            _oebPinEnableControlMaskRegister[number] = !configuration.ControlledByOutputEnablePin;
            _oebPinEnableControlMaskRegister.Write();
        }

        private void EnableDisableOutput(int number, bool state)
        {
            if (number < 0 || number >= (byte)_outputs)
            {
                throw new ArgumentException("Invalid output number", nameof(number));
            }

            _outputEnableControlRegister.Read();
            // Note: A state of 'true' is enabling the output (c.f. [1], Output Enable Control register)
            _outputEnableControlRegister[number] = state;
            _outputEnableControlRegister.Write();
        }

        /// <summary>
        /// Writes the settings, as generated by Clock Builder Pro, to the registers
        /// of the device.
        /// It parses the generated settings file and issues the related write operations
        /// to the device.
        /// Note: any setting will be applied. This includes also the configuration
        /// of the clock outputs, so there is e.g. no option to prevent from enabling
        /// any configured output.
        /// </summary>
        /// <param name="settingsFile">Line-wise content of settings file</param>
        public void WriteClockBuilderSettings(string[] settingsFile)
        {
            foreach (string line in settingsFile)
            {
                if (line.Trim().StartsWith("#"))
                {
                    continue;
                }

                string[] setting = line.Split(',');
                byte addr = byte.Parse(setting[0]);
                setting[1] = setting[1].TrimEnd('h');
                byte data = byte.Parse(setting[1], System.Globalization.NumberStyles.HexNumber);

                _i2cBus.Write(addr, data);
            }
        }
    }
}
