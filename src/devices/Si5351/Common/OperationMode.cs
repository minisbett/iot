// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Iot.Device.Si5351.Internal.Common
{
    /// <summary>
    /// Set of operation modes for Multisynths 0 to 5.
    /// </summary>
    public enum OperationMode : byte
    {
        /// <summary>
        /// Multisynth operates in fractional division mode.
        /// </summary>
        Fractional = 0,

        /// <summary>
        /// Multisynth operates in integer division mode.
        /// </summary>
        Integer = 1
    }
}
