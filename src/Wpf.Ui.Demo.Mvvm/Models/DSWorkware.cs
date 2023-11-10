// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Ui.Demo.Mvvm.Models.DSWorkwareData;
public class DSWorkware
{
    public long Id { get; set; }
    public string DomainKey { get; set; }
    public float Pressure { get; set; }
    public float Temperature { get; set; }
}

public class DSStandardData {
    public int Id { get; set; }
    public StandardEnum Standard { get; set; }
    /// <summary>
    /// 标准值
    /// </summary>
    public float Value { get; set; }
    /// <summary>
    /// 阈值
    /// </summary>
    public float ThresholdValue { get; set; }
}

public enum StandardEnum
{
    Pressure,
    Temperature,
}
