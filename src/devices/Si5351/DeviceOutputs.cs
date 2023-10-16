// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Iot.Device.Si5351
{
    /// <summary>
    /// Defines the set of number of outputs.
    /// </summary>
    public enum DeviceOutputs : byte
    {
        /// <summary>
        /// Device with 3 outputs.
        /// </summary>
        ThreeOutputs = 3,

        /// <summary>
        /// Device with 4 outputs.
        /// </summary>
        FourOutputs = 4,

        /// <summary>
        /// Device with 8 outputs.
        /// </summary>
        EightOutputs = 8,
    }
}
