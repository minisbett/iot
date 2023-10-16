// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Iot.Device.Si5351.Internal.Common
{
    /// <summary>
    /// Set of defined inversion states for clock outputs.
    /// </summary>
    public enum ClockInversionState : byte
    {
        /// <summary>
        /// Clock output is not inverted.
        /// </summary>
        NotInverted = 0,

        /// <summary>
        /// Clock output is inverted.
        /// </summary>
        Inverted = 1
    }
}
