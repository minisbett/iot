// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Drawing;
using Iot.Device.Si5351.Internal.Infrastructure;

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Device register interface for registers with a width of 1, 2 and 3 bytes.
    /// </summary>
    public abstract class DeviceRegister
    {
        /// <summary>
        /// I2C bus instance to be used for reading/writing register from/to device.
        /// </summary>
        private static I2cInterface? _bus;

        private readonly Address _addressA = Address.NoAddress;
        private readonly Address _addressB = Address.NoAddress;
        private readonly Address _addressC = Address.NoAddress;

        // <summary>
        // Initializes the base class for any derived register.
        // </summary>
        // <param name="address">Register address</param>
        // protected DeviceRegister(Address address)
        // {
        //     _addressA = address;
        // }

        /// <summary>
        /// Initializes the base class for any derived register with up to 3 addresses.
        /// </summary>
        /// <param name="addressA">First register address</param>
        /// <param name="addressB">Second register address</param>
        /// <param name="addressC">Third register address</param>
        protected DeviceRegister(Address addressA, Address addressB = Address.NoAddress, Address addressC = Address.NoAddress)
        {
            _addressA = addressA;
            _addressB = addressB;
            _addressC = addressC;
        }

        /// <summary>
        /// Reads the register data from the device.
        /// </summary>
        public abstract void Read();

        /// <summary>
        /// Writes the register data to the device.
        /// </summary>
        public abstract void Write();

        /// <summary>
        /// Performs a read operation.
        /// </summary>
        protected (byte DataA, byte DataB, byte DataC) ReadData()
        {
            if (_bus == null)
            {
                throw new Exception($"Property {nameof(_bus)} must be set to a valid instance of I2cBus");
            }

            byte dataA = _bus.Read(_addressA);
            byte dataB = _addressB != Address.NoAddress ? _bus.Read(_addressB) : (byte)0;
            byte dataC = _addressC != Address.NoAddress ? _bus.Read(_addressC) : (byte)0;
            return (dataA, dataB, dataC);
        }

        /// <summary>
        /// Sets the reference to the I2cBus instance to be used for any read or write operation.
        /// </summary>
        public static void SetBus(I2cInterface? bus)
        {
            _bus = bus;
        }

        /// <summary>
        /// Writes 'data' to the specified address, first applying a mask to the current content
        /// of the address before the write access is performed.
        /// </summary>
        protected void WriteData(byte data)
        {
            WriteInternal(data);
        }

        /// <summary>
        /// Writes 'data' to the specified address, first applying a mask to the current content
        /// of the address before the write access is performed.
        /// </summary>
        protected void WriteData(byte dataA, byte dataB)
        {
            WriteInternal(dataA, null, dataB);
        }

        /// <summary>
        /// Writes 'data' to the specified address, first applying a mask to the current content
        /// of the address before the write access is performed.
        /// Mask: only bits with '1' are modified. All other bits a preserved.
        /// </summary>
        protected void WriteDataPreserve(byte data, byte mask) => WriteInternal(data, mask);

        /// <summary>
        /// Writes 'data' to the specified address, first applying a mask to the current content
        /// of the address before the write access is performed.
        /// Mask: only bits with '1' are modified. All other bits a preserved.
        /// </summary>
        protected void WriteDataPreserve(byte dataA, byte maskA,
                                         byte dataB, byte maskB,
                                         byte dataC, byte maskC)
            => WriteInternal(dataA, maskA, dataB, maskB, dataC, maskC);

        private void WriteInternal(byte dataA, byte? maskA = null,
                                   byte? dataB = null, byte? maskB = null,
                                   byte? dataC = null, byte? maskC = null)
        {
            if (_bus == null)
            {
                throw new Exception($"Property {nameof(_bus)} must be set to a valid instance of I2cBus");
            }

            if (maskA != null)
            {
                byte regData = _bus.Read(_addressA);

                // set bits to be modified to 0
                regData &= (byte)~maskA.Value;
                regData |= (byte)(dataA & maskA.Value);
                _bus.Write(_addressA, regData);
            }
            else
            {
                _bus.Write(_addressA, dataA);
            }

            // Intentionally leave here if B is not valid, even if C would be (which would be weired anyway)
            if (dataB == null || _addressB == Address.NoAddress)
            {
                return;
            }

            if (maskB != null)
            {
                byte regData = _bus.Read(_addressB);
                regData &= (byte)~maskB.Value;
                regData |= (byte)(dataB.Value & maskB.Value);
                _bus.Write(_addressB, regData);
            }
            else
            {
                _bus.Write(_addressB, dataB.Value);
            }

            if (dataC == null || _addressC == Address.NoAddress)
            {
                return;
            }

            if (maskC != null)
            {
                byte regData = _bus.Read(_addressC);
                regData &= (byte)~maskC.Value;
                regData |= (byte)(dataC.Value & maskC.Value);
                _bus.Write(_addressC, regData);
            }
            else
            {
                _bus.Write(_addressC, dataC.Value);
            }
        }
    }
}
