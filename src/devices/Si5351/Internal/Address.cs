// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Iot.Device.Si5351.Internal
{
    /// <summary>
    /// Defines the register addresses of the Si5153A/B/C device with 10-MSOP and 20-QFN housing
    /// according to AN619 ([1]).
    /// Important: this address map, as well as various register layouts, IS NOT compatible with
    ///            a device with 16-QFN housing according to AN1234.
    /// </summary>
    public enum Address : byte
    {
        /// <summary>
        /// Device Status- register 0d/00h
        /// </summary>
        DeviceStatus = 0,

        /// <summary>
        /// Interrrupt Status Sticky - register 1d/01h
        /// </summary>
        InterruptStatusSticky = 1,

        /// <summary>
        /// Interrupt Status Mark - register 2d/02h
        /// </summary>
        InterruptStatusMask = 2,

        /// <summary>
        /// Output Enable Control - register 3d/03h
        /// </summary>
        OutputEnableControl = 3,

        /// <summary>
        /// OEB Pin Enable Control - register 9d/09h
        /// </summary>
        OebPinEnableControlMask = 9,

        /// <summary>
        /// PLL Input Source - register 15
        /// </summary>
        PllInputSource = 15,

        /// <summary>
        /// Base address of CLKx Control registers 0-7
        /// </summary>
        CLKx_Control = 16,

        /// <summary>
        /// CLK0 Control - register 16
        /// </summary>
        CLK0_Control = 16,

        /// <summary>
        /// CLK1 Control - register 17
        /// </summary>
        CLK1_Control = 17,

        /// <summary>
        /// CLK2 Control - register 18
        /// </summary>
        CLK2_Control = 18,

        /// <summary>
        /// CLK3 Control - register 19
        /// </summary>
        CLK3_Control = 19,

        /// <summary>
        /// CLK4 Control - register 20
        /// </summary>
        CLK4_Control = 20,

        /// <summary>
        /// CLK5 Control - register 21
        /// </summary>
        CLK5_Control = 21,

        /// <summary>
        /// CLK6 Control - register 22
        /// </summary>
        CLK6_Control = 22,

        /// <summary>
        /// CLK7 Control - register 23
        /// </summary>
        CLK7_Control = 23,

        /// <summary>
        /// CLK3-0 Disable State - register 24
        /// </summary>
        CLK30_DisableState = 24,

        /// <summary>
        /// CLK7-4 Disable State - register 25
        /// </summary>
        CLK74_DisableState = 25,

        /// <summary>
        /// Multisynth NA Parameter 3, bits 15:8
        /// </summary>
        MultisynthNAParameters_P3_15_8 = 26,

        /// <summary>
        /// Multisynth NA Parameter 3, bits 7:0
        /// </summary>
        MultisynthNAParameters_P3_7_0 = 27,

        /// <summary>
        /// Multisynth NA Parameter 1, bits 17:16 @ bits 1:0
        /// </summary>
        MultisynthNAParameters_P1_17_16 = 28,

        /// <summary>
        /// Multisynth NA Parameter 1, bits 15:8
        /// </summary>
        MultisynthNAParameters_P1_15_8 = 29,

        /// <summary>
        /// Multisynth NA Parameter 1, bits 7:0
        /// </summary>
        MultisynthNAParameters_P1_7_0 = 30,

        /// <summary>
        /// Multisynth NA Parameter 3, bits 19:16
        /// Multisynth NA Parameter 2, bits 19:16
        /// </summary>
        MultisynthNAParameters_P3_19_16_P2_19_16 = 31,

        /// <summary>
        /// Multisynth NA Parameter 2, bits 15:8
        /// </summary>
        MultisynthNAParameters_P2_15_8 = 32,

        /// <summary>
        /// Multisynth NA Parameter 2, bits 7:0
        /// </summary>
        MultisynthNAParameters_P2_7_0 = 33,

        /// <summary>
        /// Multisynth NB Parameter 3, bits 15:8
        /// </summary>
        MultisynthNBParameters_P3_15_8 = 34,

        /// <summary>
        /// Multisynth NB Parameter 3, bits 7:0
        /// </summary>
        MultisynthNBParameters_P3_7_0 = 35,

        /// <summary>
        /// Multisynth NB Parameter 1, bits 17:16 @ bits 1:0
        /// </summary>
        MultisynthNBParameters_P1_17_16 = 36,

        /// <summary>
        /// Multisynth NB Parameter 1, bits 15:8
        /// </summary>
        MultisynthNBParameters_P1_15_8 = 37,

        /// <summary>
        /// Multisynth NB Parameter 1, bits 7:0
        /// </summary>
        MultisynthNBParameters_P1_7_0 = 38,

        /// <summary>
        /// Multisynth NB Parameter 3, bits 19:16
        /// Multisynth NB Parameter 2, bits 19:16
        /// </summary>
        MultisynthNBParameters_P3_19_16_P2_19_16 = 39,

        /// <summary>
        /// Multisynth NB Parameter 2, bits 15:8
        /// </summary>
        MultisynthNBParameters_P2_15_8 = 40,

        /// <summary>
        /// Multisynth NB Parameter 2, bits 7:0
        /// </summary>
        MultisynthNBParameters_P2_7_0 = 41,

        /// <summary>
        /// Multisynth X Parameter 3, bit 15:8
        /// X is 0..5 with addr = base + x * 8
        /// </summary>
        MultisynthXParametersBase_P3_15_8 = 42,

        /// <summary>
        /// Multisynth X Parameter 3, bit 7:0
        /// X is 0..5 with addr = base + x * 8
        /// </summary>
        MultisynthXParametersBase_P3_7_0 = 43,

        /// <summary>
        /// Multisynth X parameter 1 bits 17:16
        /// X is 0..5 with addr = base + x * 8
        /// </summary>
        MultisynthXParametersBase_R_4_P1_17_16 = 44,

        /// <summary>
        /// Multisynth X output divider, divide by 4 enable
        /// X is 0..5 with addr = base + x * 8
        /// </summary>
        ClockOutputDivider_0_5 = 44,

        /// <summary>
        /// Multisynth X Parameter 1, bit 15:8
        /// X is 0..5 with addr = base + x * 8
        /// </summary>
        MultisynthXParametersBase_P1_15_8 = 45,

        /// <summary>
        /// Multisynth X Parameter 1, bit 7:0
        /// X is 0..5 with addr = base + x * 8
        /// </summary>
        MultisynthXParametersBase_P1_7_0 = 46,

        /// <summary>
        /// Multisynth X Parameter 3, bits 19:16
        /// Multisynth X Parameter 2, bits 19:16
        /// X is 0..5 with addr = base + x * 8
        /// </summary>
        MultisyntXParametersBase_P3_19_16_P2_19_16 = 47,

        /// <summary>
        /// Multisynth X Parameter 2, bit 15:8
        /// X is 0..5 with addr = base + x * 8
        /// </summary>
        MultisynthXParametersBase_P2_15_8 = 48,

        /// <summary>
        /// Multisynth X Parameter 2, bit 7:0
        /// X is 0..5 with addr = base + x * 8
        /// </summary>
        MultisynthXParametersBase_P2_7_0 = 49,

        /// <summary>
        /// Multisynth 6 Parameter 1, bit 7:0
        /// </summary>
        Multisynth6Parameter1 = 90,

        /// <summary>
        /// Multisynth 7 Parameter 1, bit 7:0
        /// </summary>
        Multisynth7Parameter1 = 91,

        /// <summary>
        /// Clock 6 and 7 Output Divider
        /// </summary>
        ClockOutputDivider_6_7 = 92,

        /// <summary>
        /// PLL Reset register
        /// </summary>
        PllReset = 177,

        /// <summary>
        /// Crystal Internal Load Capacitance
        /// </summary>
        CrystalInternalLoadCapacitance = 183,

        /// <summary>
        /// Fanout Enable
        /// </summary>
        FanoutEnable = 187,

        /// <summary>
        /// For driver internal purpose only
        /// </summary>
        NoAddress = 255
    }
}
