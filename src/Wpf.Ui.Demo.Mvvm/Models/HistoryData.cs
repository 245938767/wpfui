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

namespace Wpf.Ui.Demo.Mvvm.Models;
public class HistoryData
{
    public long Id { get; set; }

    [Description("是否检测完成")] public bool IsCheck { get; set; } = false;
    public DateTime CreateTime { get; set; }
    public int DataCount { get; set; }
    public string TemperatureList { get; set; }
    public string PressureList { get; set; }

}
