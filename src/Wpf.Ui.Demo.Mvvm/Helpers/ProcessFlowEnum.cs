// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Demo.Mvvm.Helpers.Converter;

namespace Wpf.Ui.Demo.Mvvm.Helpers;

[TypeConverter(typeof(EnumDescriptionTypeConverter))]
public enum ProcessFlowEnum
{
    [Description("DS工装测试流程")]
    DSTest,
    [Description("压力传感器测试流程")]
    PressureSensorTest,
    [Description("DS工装测试流程3")]
    DSTest3,
}