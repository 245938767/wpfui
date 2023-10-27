// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Ui.Demo.Mvvm.Models;

public class DeviceCardDetail
{
    public SerialPortModel SerialPortModel { get; set; }
    public string? CurrentTemperature { get; set; }
    public string? CurrentPressure { get; set; }
    public string? SettingTemperature { get; set; }
    public string? SettingPressure { get; set; }
}