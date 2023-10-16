// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Iot.Device.Si5351.Common
{
    /// <summary>
    /// Set of available internal load capacitances for the crystal.
    /// </summary>
    public enum CrystalLoadCapacitance : byte
    {
        /// <summary>
        /// Internal capicatance load = 0 pF
        /// AN619 recommends "Do not select this option."
        /// </summary>
        Cl0pF = 0b00,

        /// <summary>
        /// Internal capacitance load = 6 pF
        /// </summary>
        Cl6pF = 0b01,

        /// <summary>
        /// Internal capacitance load = 8 pF
        /// </summary>
        Cl8pF = 0b10,

        /// <summary>
        /// Internal capacitance load = 10 pF
        /// </summary>
        Cl10pF = 0b11
    }
}
