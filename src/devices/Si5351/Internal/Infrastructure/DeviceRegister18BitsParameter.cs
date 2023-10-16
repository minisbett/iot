// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Provides read / write access to a set of registers assembling a 18-bit parameter.
    /// </summary>
    public abstract class DeviceRegister18BitsParameter : DeviceRegister
    {
        /// <summary>
        /// Parameter value, updated by Read() and used by Write().
        /// </summary>
        public int Parameter { get; set; }

        /// <summary>
        /// Initializes a new instance of a device registers set for a 20-bit parameter
        /// </summary>
        /// <param name="bits_17_16">Address of register with bits 17:16</param>
        /// <param name="bits_15_8">Address of register with bits 15:8</param>
        /// <param name="bits_7_0">Adress of register with bits 7:0</param>
        protected DeviceRegister18BitsParameter(Address bits_17_16, Address bits_15_8, Address bits_7_0)
           : base(bits_17_16, bits_15_8, bits_7_0)
        {
        }

        /// <inheritdoc/>
        public override void Read()
        {
            Parameter = 0;

            (byte dataA, byte dataB, byte dataC) = ReadData();
            Parameter |= (dataA & 0b0000_0011) << 16;
            Parameter |= dataB << 8;
            Parameter |= dataC;
        }

        /// <inheritdoc/>
        public override void Write()
        {
            // preserve other bits when storing parameter bits[17:16] into shared register
            WriteDataPreserve((byte)(Parameter >> 16), 0b0000_0011,
                              (byte)(Parameter >> 8), 0b1111_1111,
                              (byte)Parameter, 0b1111_1111);
        }
    }
}
