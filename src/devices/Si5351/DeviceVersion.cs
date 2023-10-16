// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Iot.Device.Si5351
{
    /// <summary>
    /// Defines the set of device versions.
    /// [1] Datasheet, revision 1.3, Skyworks Solution Inc., as of 2021-08-27
    /// </summary>
    public enum DeviceVersion
    {
        /// <summary>
        /// Device version A (internal oscillator with XTAL only input, see [1] pg. 1)
        /// </summary>
        A = 0,

        /// <summary>
        /// Device version B (internal oscillator with XTAL input and internal VCXO with control input, see [1] pg. 1)
        /// </summary>
        B = 1,

        /// <summary>
        /// Device version C (like B version but with additional clock synchronization input, see [1] pg. 1)
        /// </summary>
        C = 2
    }
}
