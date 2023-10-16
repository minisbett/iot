// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Si5351.Common;

namespace Iot.Device.Si5351.Internal.Register
{
    /// <summary>
    /// Represents the Crystal Internal Load Capacitance register.
    /// Address: 183d/B7h
    /// Documentation: AN619, Skyworks Solutions Inc.
    /// </summary>
    public class CrystalInternalLoadCapacitanceRegister : DeviceRegister
    {
        /// <summary>
        /// Initializes a new instance of the CrystalInternalLoadCapacitanceRegister class.
        /// </summary>
        public CrystalInternalLoadCapacitanceRegister()
            : base(Address.CrystalInternalLoadCapacitance)
        {
        }

        /// <summary>
        /// Internal load capacitance value for the crystal.
        /// </summary>
        public CrystalLoadCapacitance XtalCl { get; set; } = CrystalLoadCapacitance.Cl10pF;

        /// <inheritdoc/>
        public override void Read()
        {
            byte data = ReadData().DataA;
            XtalCl = (CrystalLoadCapacitance)((data & 0b1100_0000) >> 6);
        }

        /// <inheritdoc/>
        public override void Write()
        {
            WriteData((byte)(((byte)XtalCl << 6) | 0b0001_0010));
        }
    }
}
