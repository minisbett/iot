// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Iot.Device.Si5351
{
    /// <summary>
    /// Defines the set of device revisions.
    /// [1] Datasheet, revision 1.3, Skyworks Solution Inc., as of 2021-08-27
    /// </summary>
    public enum DeviceRevision
    {
        /// <summary>
        /// Device revision A (deprecated/discontinued)
        /// </summary>
        A = 0,

        /// <summary>
        /// Device version B (current revision)
        /// </summary>
        B = 1,

        /// <summary>
        /// Reserved
        /// </summary>
        Reserved1 = 2,

        /// <summary>
        /// Reserved
        /// </summary>
        Reserved2 = 3
    }
}
