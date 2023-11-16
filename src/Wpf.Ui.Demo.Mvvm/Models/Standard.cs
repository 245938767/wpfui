// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Demo.Mvvm.Helpers;

namespace Wpf.Ui.Demo.Mvvm.Models;

/// <summary>
/// 测试数据对象集
/// </summary>
public partial class Standard: ObservableObject
{
    public int Id { get; set; }
    [Description("数据名称")]
    public string Name { get; set; }
    [Description("流程类型")]
    public ProcessFlowEnum ProcessFlow { get; set; }

    [Description("数据")]
    public ObservableCollection<StandardData> StandarDatas { get; set; } = new ObservableCollection<StandardData>();

}
/// <summary>
/// DS 工装 标准对象
/// </summary>
public partial class StandardData: ObservableObject
{
    public int Id { get; set; }

    [Description("父Id")]
    public int StandardId { get; set; }

    [ObservableProperty]
    private StandardEnum _standardType;

    /// <summary>
    /// 标准值
    /// </summary>
    [ObservableProperty]
    private float _value;

    /// <summary>
    /// 阈值
    /// </summary>
    [ObservableProperty]
    private float _thresholdValue;
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