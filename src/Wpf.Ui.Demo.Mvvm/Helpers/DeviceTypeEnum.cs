// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Ui.Demo.Mvvm.Helpers;
public enum DeviceTypeEnum
{
    /**
     * 空气泵
     */
    Pump,

    /**
     * 压力源
     */
    Pressure,

    /**
     * 温箱
     */
    Temperature,

    /**
     * DS工装
     */
    DSWork,
    /**
     * 压力传感器检测工装
     */
    PressureSensor
}