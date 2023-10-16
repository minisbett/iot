// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Iot.Device.Si5351.Common
{
    /// <summary>
    /// Set of available drive strengths.
    /// </summary>
    public enum DriveStrength : byte
    {
        /// <summary>
        /// Drive strength 2 mA
        /// </summary>
        Strength2mA = 0b00,

        /// <summary>
        /// Drive strength 4 mA
        /// </summary>
        Strength4mA = 0b01,

        /// <summary>
        /// Drive strength 6 mA
        /// </summary>
        Strength6mA = 0b10,

        /// <summary>
        /// Drive strength 8 mA
        /// </summary>
        Strength8mA = 0b11
    }
}
