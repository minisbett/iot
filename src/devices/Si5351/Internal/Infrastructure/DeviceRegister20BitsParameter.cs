// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Provides read / write access to a set of registers assembling a 20-bit parameter.
    /// </summary>
    public abstract class DeviceRegister20BitsParameter : DeviceRegister
    {
        /// <summary>
        /// Location for the relevant parameter bits of a shared register to be set by derived class
        /// </summary>
        protected readonly BitsLocation _addrBitsLocation_19_16;

        /// <summary>
        /// Initializes a new instance of a device registers set for a 20-bit parameter
        /// </summary>
        /// <param name="bits_19_16">Address of register with bits 19:16</param>
        /// <param name="bitsLocation_19_16">Location of bits 19:16 in respective register</param>
        /// <param name="bits_15_8">Address of register with bits 15:8</param>
        /// <param name="bits_7_0">Adress of register with bits 7:0</param>
        protected DeviceRegister20BitsParameter(Address bits_19_16,
                                                 BitsLocation bitsLocation_19_16,
                                                 Address bits_15_8,
                                                 Address bits_7_0)
            : base(bits_19_16, bits_15_8, bits_7_0)
        {
            if (!Enum.IsDefined(typeof(BitsLocation), _addrBitsLocation_19_16))
            {
                throw new ArgumentException($"Invalid bit position ({bitsLocation_19_16})");
            }

            _addrBitsLocation_19_16 = bitsLocation_19_16;
        }

        /// <summary>
        /// Parameter value, updated by Read() and used by Write().
        /// </summary>
        public int Parameter { get; set; }

        /// <inheritdoc/>
        public override void Read()
        {
            Parameter = 0;

            (byte dataA, byte dataB, byte dataC) = ReadData();

            Parameter |= _addrBitsLocation_19_16 switch
            {
                BitsLocation.Bits_7_4 => (dataA & 0b1111_0000) << 12,
                BitsLocation.Bits_3_0 => (dataA & 0b0000_1111) << 16,
                _ => throw new InvalidOperationException($"Invalid location ({_addrBitsLocation_19_16})")
            };

            Parameter |= dataB << 8;
            Parameter |= dataC;
        }

        /// <inheritdoc/>
        public override void Write()
        {
            byte dataA;
            byte maskA;
            // preserve other bits when storing parameter bits[19:16] into shared register

            // preserve other bits
            switch (_addrBitsLocation_19_16)
            {
                case BitsLocation.Bits_7_4:
                    maskA = 0b1111_0000;
                    dataA = (byte)((Parameter >> 12) & 0b1111_0000);
                    break;
                case BitsLocation.Bits_3_0:
                    maskA = 0b0000_1111;
                    dataA = (byte)((Parameter >> 16) & 0b0000_1111);
                    break;
                default:
                    throw new InvalidOperationException($"Invalid location ({_addrBitsLocation_19_16})");
            }

            WriteDataPreserve(dataA, maskA,
                              (byte)(Parameter >> 8), 0b1111_1111,
                              (byte)Parameter, 0b1111_1111);

        }

        /// <summary>
        /// Bits where the parameter bits 19:16 are located in the respective register
        /// </summary>
        protected enum BitsLocation
        {
            /// <summary>
            /// Bits 19:16 are located in bits 3:0
            /// </summary>
            Bits_3_0,

            /// <summary>
            /// Bits 19:16 are located in bits 7:4
            /// </summary>
            Bits_7_4
        }
    }
}
