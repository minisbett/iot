// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Represents the output enable register.
    /// Address: 3d/03h
    /// Documentation: AN619, Skyworks Solutions Inc.
    /// </summary>
    public class OutputEnableControlRegister : DeviceRegister
    {
        /// <summary>
        /// Initializes a new instance of the OutputEnableControlRegister class.
        /// </summary>
        public OutputEnableControlRegister()
            : base(Address.OutputEnableControl)
        {
        }

        /// <summary>
        /// State of output driver for clock 7 (false=enabled, true=disabled)
        /// </summary>
        public bool CLK7_OEB { get; set; }

        /// <summary>
        /// State of output driver for clock 6 (false=enabled, true=disabled)
        /// </summary>
        public bool CLK6_OEB { get; set; }

        /// <summary>
        /// State of output driver for clock 5 (false=enabled, true=disabled)
        /// </summary>
        public bool CLK5_OEB { get; set; }

        /// <summary>
        /// State of output driver for clock 4 (false=enabled, true=disabled)
        /// </summary>
        public bool CLK4_OEB { get; set; }

        /// <summary>
        /// State of output driver for clock 3 (false=enabled, true=disabled)
        /// </summary>
        public bool CLK3_OEB { get; set; }

        /// <summary>
        /// State of output driver for clock 2 (false=enabled, true=disabled)
        /// </summary>
        public bool CLK2_OEB { get; set; }

        /// <summary>
        /// State of output driver for clock1 (false=enabled, true=disabled)
        /// </summary>
        public bool CLK1_OEB { get; set; }

        /// <summary>
        /// State of output driver for clock 0 (false=enabled, true=disabled)
        /// </summary>
        public bool CLK0_OEB { get; set; }

        /// <summary>
        /// Gets or sets the state of the output selected by the index
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">Thrown if index not with the range of 0..7</exception>
        public bool this[int output]
        {
            get
            {
                return output switch
                {
                    0 => CLK0_OEB,
                    1 => CLK1_OEB,
                    2 => CLK2_OEB,
                    3 => CLK3_OEB,
                    4 => CLK4_OEB,
                    5 => CLK5_OEB,
                    6 => CLK6_OEB,
                    7 => CLK7_OEB,
                    _ => throw new IndexOutOfRangeException()
                };
            }

            set
            {
                switch (output)
                {
                    case 0:
                        CLK0_OEB = value;
                        break;
                    case 1:
                        CLK1_OEB = value;
                        break;
                    case 2:
                        CLK2_OEB = value;
                        break;
                    case 3:
                        CLK3_OEB = value;
                        break;
                    case 4:
                        CLK4_OEB = value;
                        break;
                    case 5:
                        CLK5_OEB = value;
                        break;
                    case 6:
                        CLK6_OEB = value;
                        break;
                    case 7:
                        CLK7_OEB = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        /// <inheritdoc/>
        public override void Read()
        {
            byte data = ReadData().DataA;
            CLK7_OEB = (data & 0b1000_0000) == 1;
            CLK6_OEB = (data & 0b0100_0000) == 1;
            CLK5_OEB = (data & 0b0010_0000) == 1;
            CLK4_OEB = (data & 0b0001_0000) == 1;
            CLK3_OEB = (data & 0b0000_1000) == 1;
            CLK2_OEB = (data & 0b0000_0100) == 1;
            CLK1_OEB = (data & 0b0000_0010) == 1;
            CLK0_OEB = (data & 0b0000_0001) == 1;
        }

        /// <inheritdoc/>
        public override void Write()
        {
            byte data = 0;
            data |= (byte)(CLK7_OEB ? 0b_1000_0000 : 0);
            data |= (byte)(CLK6_OEB ? 0b_0100_0000 : 0);
            data |= (byte)(CLK5_OEB ? 0b_0010_0000 : 0);
            data |= (byte)(CLK4_OEB ? 0b_0001_0000 : 0);
            data |= (byte)(CLK3_OEB ? 0b_0000_1000 : 0);
            data |= (byte)(CLK2_OEB ? 0b_0000_0100 : 0);
            data |= (byte)(CLK1_OEB ? 0b_0000_0010 : 0);
            data |= (byte)(CLK0_OEB ? 0b_0000_0001 : 0);
            WriteData(data);
        }
    }
}
