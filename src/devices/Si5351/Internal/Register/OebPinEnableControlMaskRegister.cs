// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Represents the OEB Pin Enable Control Mask register.
    /// Address: 9d/09h
    /// Documentation: AN619, Skyworks Solutions Inc.
    /// </summary>
    internal class OebPinEnableControlMaskRegister : DeviceRegister
    {
        /// <summary>
        /// Initializes a new instance of the OebPinEnableControlMaskRegister class.
        /// </summary>
        public OebPinEnableControlMaskRegister()
            : base(Address.OebPinEnableControlMask)
        {
        }

        /// <summary>
        /// Setting controls whether the OEB pin controls the enable/disable state of CLK7 output.
        /// False: OEB pin controls enable/disable state of CLK7 output.
        /// True: OEB pin does not control enable/disable state of CLK7 output.
        /// </summary>
        public bool OEB_MASK7 { get; set; }

        /// <summary>
        /// Setting controls whether the OEB pin controls the enable/disable state of CLK6 output.
        /// False: OEB pin controls enable/disable state of CLK6 output.
        /// True: OEB pin does not control enable/disable state of CLK6 output.
        /// </summary>
        public bool OEB_MASK6 { get; set; }

        /// <summary>
        /// Setting controls whether the OEB pin controls the enable/disable state of CLK5 output.
        /// False: OEB pin controls enable/disable state of CLK5 output.
        /// True: OEB pin does not control enable/disable state of CLK5 output.
        /// </summary>
        public bool OEB_MASK5 { get; set; }

        /// <summary>
        /// Setting controls whether the OEB pin controls the enable/disable state of CLK4 output.
        /// False: OEB pin controls enable/disable state of CLK4 output.
        /// True: OEB pin does not control enable/disable state of CLK4 output.
        /// </summary>
        public bool OEB_MASK4 { get; set; }

        /// <summary>
        /// Setting controls whether the OEB pin controls the enable/disable state of CLK3 output.
        /// False: OEB pin controls enable/disable state of CLK3 output.
        /// True: OEB pin does not control enable/disable state of CLK3 output.
        /// </summary>
        public bool OEB_MASK3 { get; set; }

        /// <summary>
        /// Setting controls whether the OEB pin controls the enable/disable state of CLK2 output.
        /// False: OEB pin controls enable/disable state of CLK2 output.
        /// True: OEB pin does not control enable/disable state of CLK2 output.
        /// </summary>
        public bool OEB_MASK2 { get; set; }

        /// <summary>
        /// Setting controls whether the OEB pin controls the enable/disable state of CLK1 output.
        /// False: OEB pin controls enable/disable state of CLK1 output.
        /// True: OEB pin does not control enable/disable state of CLK1 output.
        /// </summary>
        public bool OEB_MASK1 { get; set; }

        /// <summary>
        /// Setting controls whether the OEB pin controls the enable/disable state of CLK0 output.
        /// False: OEB pin controls enable/disable state of CLK0 output.
        /// True: OEB pin does not control enable/disable state of CLK0 output.
        /// </summary>
        public bool OEB_MASK0 { get; set; }

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
                    0 => OEB_MASK0,
                    1 => OEB_MASK1,
                    2 => OEB_MASK2,
                    3 => OEB_MASK3,
                    4 => OEB_MASK4,
                    5 => OEB_MASK5,
                    6 => OEB_MASK6,
                    7 => OEB_MASK7,
                    _ => throw new IndexOutOfRangeException()
                };
            }

            set
            {
                switch (output)
                {
                    case 0:
                        OEB_MASK0 = value;
                        break;
                    case 1:
                        OEB_MASK1 = value;
                        break;
                    case 2:
                        OEB_MASK2 = value;
                        break;
                    case 3:
                        OEB_MASK3 = value;
                        break;
                    case 4:
                        OEB_MASK4 = value;
                        break;
                    case 5:
                        OEB_MASK5 = value;
                        break;
                    case 6:
                        OEB_MASK6 = value;
                        break;
                    case 7:
                        OEB_MASK7 = value;
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
            OEB_MASK7 = (data & 0b1000_0000) == 1;
            OEB_MASK6 = (data & 0b0100_0000) == 1;
            OEB_MASK5 = (data & 0b0010_0000) == 1;
            OEB_MASK4 = (data & 0b0001_0000) == 1;
            OEB_MASK3 = (data & 0b0000_1000) == 1;
            OEB_MASK2 = (data & 0b0000_0100) == 1;
            OEB_MASK1 = (data & 0b0000_0010) == 1;
            OEB_MASK0 = (data & 0b0000_0001) == 1;
        }

        /// <inheritdoc/>
        public override void Write()
        {
            byte data = 0;
            data |= (byte)(OEB_MASK7 ? 0b_1000_0000 : 0);
            data |= (byte)(OEB_MASK6 ? 0b_0100_0000 : 0);
            data |= (byte)(OEB_MASK5 ? 0b_0010_0000 : 0);
            data |= (byte)(OEB_MASK4 ? 0b_0001_0000 : 0);
            data |= (byte)(OEB_MASK3 ? 0b_0000_1000 : 0);
            data |= (byte)(OEB_MASK2 ? 0b_0000_0100 : 0);
            data |= (byte)(OEB_MASK1 ? 0b_0000_0010 : 0);
            data |= (byte)(OEB_MASK0 ? 0b_0000_0001 : 0);
            WriteData(data);
        }
    }
}
