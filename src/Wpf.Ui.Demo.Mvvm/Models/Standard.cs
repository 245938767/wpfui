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

public class Standard
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<StandardData>? StandarDatas { get; set; }
}
/// <summary>
/// DS 工装 标准对象
/// </summary>
public class StandardData
{
    public int Id { get; set; }
    public int StandardId { get; set; }

    public StandardEnum StandardType { get; set; }

    /// <summary>
    /// 标准值
    /// </summary>
    public float Value { get; set; }

    /// <summary>
    /// 阈值
    /// </summary>
    public float ThresholdValue { get; set; }
    public Standard Standard { get; set; }
}

/// <summary>
/// 标准类型
/// </summary>
public enum StandardEnum
{
    Pressure,
    Temperature,
}