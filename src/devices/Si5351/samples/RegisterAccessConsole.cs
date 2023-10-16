// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Device.I2c;
using System.Globalization;
using System.Reflection;
using Iot.Device.Si5351.Common;
using Iot.Device.Si5351.Internal.Infrastructure;
using Iot.Device.Si5351.Internal.Register;

namespace Si5351.Samples
{
    /// <summary>
    /// A simple console to access various registers of the device directly.
    /// This allows playing around with settings and learn how the device behaves.
    /// Besides the register classes also a direct memory access by address is available.
    /// </summary>
    public class RegisterAccessConsole
    {
        private readonly I2cDevice _i2cDevice;
        private readonly I2cInterface _i2cInterface;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterAccessConsole"/> class.
        /// </summary>
        public RegisterAccessConsole(I2cDevice i2cDevice)
        {
            _i2cDevice = i2cDevice;
            _i2cInterface = new I2cInterface(_i2cDevice);
        }

        /// <summary>
        /// Main loop of the conosle. The loop continues until the RET (return) command is entered.
        /// </summary>
        public void Loop()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== SI5351 Sample Application - Register Access Console ===\n\n");
                Console.WriteLine("(RET) Return to main console");
                Console.WriteLine("(RDA) Read data from address");
                Console.WriteLine("(WDA) Write data to address");
                Console.WriteLine("(PMS) Read and modify Multisynth parameters for PLL (stage 1)");
                Console.WriteLine("(OMS) Read and modify output Multisynth parameters (stage 2)");
                Console.WriteLine("(RPL) Reset PLLs");
                Console.Write("==> ");

                string? command = Console.ReadLine()?.ToUpper();
                if (command == null)
                {
                    continue;
                }

                switch (command)
                {
                    case "RET":
                        return;

                    case "RDA":
                        ReadWriteDataFromAddress(false);
                        break;

                    case "WDA":
                        ReadWriteDataFromAddress(true);
                        break;

                    case "OMS":
                        ReadOrModifyOutputMultisynthParameters();
                        break;

                    case "PMS":
                        ReadOrModifyPllMultisynthParameters();
                        break;

                    case "RPL":
                        ResetPLLs();
                        break;

                    default:
                        Console.WriteLine($"Unknown command ({command})");
                        break;
                }

                Console.ReadKey();
            }
        }

        private void ResetPLLs()
        {
            PllResetRegister reg = new PllResetRegister();
            reg.PllARst = true;
            reg.PllBRst = true;
            reg.Write();
        }

        private void ReadWriteDataFromAddress(bool write)
        {
            if (!PromptIntegerValue("Address [dec]", out int address))
            {
                return;
            }

            byte data = _i2cInterface.Read(address);
            Console.WriteLine($"==> {address:D3}d / {address:X2}h : {data:D3}d / {data:X2}h / {Convert.ToString(data, 2).PadLeft(8, '0')}b");

            if (write)
            {
                if (!PromptIntegerValue("Data to write [hex]", out int dataToWrite, true))
                {
                    return;
                }

                _i2cInterface.Write((byte)address, (byte)dataToWrite);
            }
        }

        private void ReadOrModifyOutputMultisynthParameters()
        {
            if (!PromptIntegerValue("Mutlisynth [0..5]", out int ms, false, 0, 5))
            {
                return;
            }

            MultisynthParameter1Register regP1 = new(MultiSynth.MS0 + ms);
            MultisynthParameter2Register regP2 = new(MultiSynth.MS0 + ms);
            MultisynthParameter3Register regP3 = new(MultiSynth.MS0 + ms);
            MultisynthOutputDividerRegister regOutputDivider = new(MultiSynth.MS0 + ms);

            regP1.Read();
            regP2.Read();
            regP3.Read();
            regOutputDivider.Read();

            Console.WriteLine($"Parameter 1: {regP1.Parameter}d/{regP1.Parameter:X}h");
            Console.WriteLine($"Parameter 2: {regP2.Parameter}d/{regP2.Parameter:X}h");
            Console.WriteLine($"Parameter 3: {regP3.Parameter}d/{regP3.Parameter:X}h");
            Console.WriteLine($"R divider:   {regOutputDivider.Divider}");
            Console.WriteLine($"Divide by 4: {regOutputDivider.DivideBy4}");

            if (!PromptYesNo("Modify parameters?", out bool modify) || !modify)
            {
                return;
            }

            bool inputOk = true;
            inputOk &= PromptIntegerValue("Parameter 1", out int p1, false, 0);
            inputOk &= PromptIntegerValue("Parameter 2", out int p2, false, 0);
            inputOk &= PromptIntegerValue("Parameter 3", out int p3, false, 0);
            inputOk &= PromptEnum<OutputDivider>("R divider", out OutputDivider divider);
            inputOk &= PromptYesNo("Divide by 4", out bool divideBy4);
            if (!inputOk)
            {
                return;
            }

            regP1.Parameter = p1;
            regP2.Parameter = p2;
            regP3.Parameter = p3;
            regOutputDivider.Divider = divider;
            regOutputDivider.DivideBy4 = divideBy4;
            regP1.Write();
            regP2.Write();
            regP3.Write();
            regOutputDivider.Write();
        }

        private void ReadOrModifyPllMultisynthParameters()
        {
            if (!PromptEnum<Pll>("PLL", out Pll pll))
            {
                return;
            }

            MultisynthNParameter1Register regP1 = new(pll);
            MultisynthNParameter2Register regP2 = new(pll);
            MultisynthNParameter2Register regP3 = new(pll);

            regP1.Read();
            regP2.Read();
            regP3.Read();

            Console.WriteLine($"Parameter 1: {regP1.Parameter}d/{regP1.Parameter:X}h");
            Console.WriteLine($"Parameter 2: {regP2.Parameter}d/{regP2.Parameter:X}h");
            Console.WriteLine($"Parameter 3: {regP3.Parameter}d/{regP3.Parameter:X}h");

            if (!PromptYesNo("Modify parameters?", out bool modify) || !modify)
            {
                return;
            }

            bool inputOk = true;
            inputOk &= PromptIntegerValue("Parameter 1", out int p1, false, 0);
            inputOk &= PromptIntegerValue("Parameter 2", out int p2, false, 0);
            inputOk &= PromptIntegerValue("Parameter 3", out int p3, false, 0);
            if (!inputOk)
            {
                return;
            }

            regP1.Parameter = p1;
            regP2.Parameter = p2;
            regP3.Parameter = p3;
            regP1.Write();
            regP2.Write();
            regP3.Write();
        }

        #region Helper methods
        private bool PromptIntegerValue(string prompt, out int value, bool fromHex = false, int min = int.MinValue, int max = int.MaxValue)
        {
            Console.Write(prompt + ": ");
            string? input = Console.ReadLine();
            bool result = false;
            if (!fromHex)
            {
                result = int.TryParse(input, out value);
            }
            else
            {
                result = int.TryParse(input, NumberStyles.HexNumber, null, out value);
            }

            if (!result)
            {
                Console.WriteLine("Invalid input");
                return false;
            }

            if (value < min || value > max)
            {
                Console.WriteLine($"Input out of range ({min}-{max})");
                return false;
            }

            return true;
        }

        private bool PromptYesNo(string prompt, out bool value)
        {
            Console.Write(prompt + "[y/n]: ");
            string? input = Console.ReadLine();
            if (input?.ToLower() == "y")
            {
                value = true;
                return true;
            }
            else if (input?.ToLower() == "n")
            {
                value = false;
                return true;
            }

            Console.WriteLine("Invalid input");
            value = default;
            return false;
        }

        private bool PromptEnum<T>(string prompt, out T value)
            where T : struct
        {
            Console.WriteLine(prompt);

            foreach (var x in Enum.GetValues(typeof(T)))
            {
                Console.WriteLine("  " + x.ToString());
            }

            Console.Write("=> ");

            string? input = Console.ReadLine();
            if (Enum.TryParse<T>(input, true, out T parsedValue) && Enum.IsDefined(typeof(T), parsedValue))
            {
                value = parsedValue;
                return true;
            }
            else
            {
                Console.WriteLine($"Invalid input ({input})");
                value = default(T);
                return false;
            }
        }
        #endregion
    }
}
