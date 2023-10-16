// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Device.I2c;
using System.IO;
using Iot.Device.Si5351;
using Iot.Device.Si5351.Internal.Register;
using Si5351.Samples;

internal class Program
{
    private static I2cDevice? _i2cDevice;
    private static Si5351Common? _device;

    private static void Main(string[] args)
    {
        const int I2cBus = 1;
        I2cConnectionSettings i2cSettings = new(I2cBus, Si5351Device.DefaultI2cAddress);
        I2cDevice i2cDevice = I2cDevice.Create(i2cSettings);

        _device = new(i2cDevice, DeviceVersion.A, DeviceOutputs.ThreeOutputs);
        _i2cDevice = i2cDevice;

        RegisterAccessConsole registerAccessConsole = new(_i2cDevice);

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== SI5351 Sample Application ===\n\n");
            Console.WriteLine("(SST) Show Status");
            Console.WriteLine("(CBS) Load Clock Builder settings file");
            Console.WriteLine("(OES) Output enable status");
            Console.WriteLine("(PLG) Playground");
            Console.WriteLine("(RAC) Register access console");
            Console.WriteLine("(DUM) Dump all registers");
            Console.WriteLine("(QQQ) Quit");
            Console.Write("==> ");

            string? choice = Console.ReadLine()?.ToUpper();
            if (choice == null)
            {
                continue;
            }

            switch (choice)
            {
                case "SST":
                    ShowStatus();
                    break;

                case "CBS":
                    LoadClockBuilderFile();
                    break;

                case "OES":
                    OutputEnableState();
                    break;

                case "PLG":
                    Playground();
                    break;

                case "RAC":
                    registerAccessConsole.Loop();
                    break;

                case "DUM":
                    DumpRegisters();
                    break;

                case "QQQ":
                    return;
            }
        }

        // device.ClearInterruptStatus();
        // DeviceStatus status = device.GetDeviceStatus();
        // Console.WriteLine(status);
        // InterruptStatus interruptStatus = device.GetInterruptStatus();
        // Console.WriteLine(interruptStatus);

        // Console.WriteLine("Reading register file:" + args[0]);
        // device.WriteClockBuilderSettings(File.ReadAllLines(args[0]));

        // status = device.GetDeviceStatus();
        // Console.WriteLine(status);
        // interruptStatus = device.GetInterruptStatus();
        // Console.WriteLine(interruptStatus);
    }

    private static void ShowStatus()
    {
        DeviceStatus status = _device!.GetDeviceStatus();
        Console.WriteLine(status);
        Console.ReadKey();
    }

    private static void OutputEnableState()
    {
        Console.WriteLine("Output state:");
        for (int i = 0; i < _device!.Outputs; i++)
        {
            Console.WriteLine($"  {i}: {_device.GetOutputState(i)}");
        }

        int? output = GetNumber("Output to change (enter for none): ");
        if (output == null)
        {
            return;
        }

        int? state = GetNumber("New state (0: disable, 1: enable): ");
        if (state == null)
        {
            return;
        }

        if (state == 0)
        {
            _device.DisableOutput((int)output);
        }
        else
        {
            _device.EnableOutput((int)output);
        }
    }

    private static void LoadClockBuilderFile()
    {
        ListFilesWithExtension("txt");
        Console.Write("File: ");
        string? file = Console.ReadLine();
        if (file == null)
        {
            return;
        }

        try
        {
            _device!.WriteClockBuilderSettings(File.ReadAllLines(file));
            Console.WriteLine("Settings file loaded to device");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Loading settings file failed.");
            Console.WriteLine(ex.Message);
            Console.ReadKey();
        }
    }

    private static int? GetNumber(string prompt)
    {
        Console.Write(prompt);
        string? s = Console.ReadLine();

        if (string.IsNullOrEmpty(s) || !int.TryParse(s, out int v))
        {
            return int.MinValue;
        }

        return v;
    }

    private static void Playground()
    {
        // synthesis stage 2
        int a = 56;
        int b = 0;
        int c = 1;

        int p1 = (int)(128d * a + Math.Floor(128d * b / c) - 512d);
        int p2 = (int)(128 * b - c * Math.Floor(120d * b / c));
        int p3 = c;

        MultisynthParameter1Register p1Reg = new(Iot.Device.Si5351.Common.MultiSynth.MS0);
        MultisynthParameter2Register p2Reg = new(Iot.Device.Si5351.Common.MultiSynth.MS0);
        MultisynthParameter3Register p3Reg = new(Iot.Device.Si5351.Common.MultiSynth.MS0);
        p1Reg.Parameter = p1;
        p2Reg.Parameter = p2;
        p3Reg.Parameter = p3;
        p1Reg.Write();
        p2Reg.Write();
        p3Reg.Write();

        MultisynthOutputDividerRegister divReg = new(Iot.Device.Si5351.Common.MultiSynth.MS0);
        divReg.Read();
        Console.WriteLine($"R0 Output Divider: {divReg.Divider}");
        Console.WriteLine($"DivBy4: {divReg.DivideBy4}");
        divReg.Divider = Iot.Device.Si5351.Common.OutputDivider.Div1;
        divReg.DivideBy4 = false;
        divReg.Write();

        divReg.Read();
        Console.WriteLine($"R0 Output Divider: {divReg.Divider}");
        Console.WriteLine($"DivBy4: {divReg.DivideBy4}");
        Console.ReadKey();
    }

    private static void DumpRegisters()
    {
        Span<byte> output = stackalloc byte[2];
        output[0] = 0x00;
        output[1] = 0x00;
        _i2cDevice!.Write(output);

        int registerMapSize = 188;
        byte[] data = new byte[registerMapSize];
        _i2cDevice!.Read(data);

        Console.Write("     ");
        for (int c = 0; c < 0x10; c++)
        {
            Console.Write($"{c:X2}h  ");
        }

        Console.WriteLine();

        int x = 0;
        int y = 0;
        for (int i = 0; i < data.Length; i++)
        {
            if (x == 0)
            {
                Console.Write($"{y * 0x10:X2}h  ");
            }

            Console.Write($"{y * 0x10 + x:X2}h  ");

            x++;
            if (x == 0x10)
            {
                x = 0;
                y++;
                Console.WriteLine();
            }
        }

        Console.ReadKey();
    }

    public static void ListFilesWithExtension(string extension)
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        Console.WriteLine($"Current directory: {currentDirectory}");
        var files = Directory.EnumerateFiles(currentDirectory, $"*.{extension}");
        foreach (string file in files)
        {
            Console.WriteLine(file);
        }
    }
}
