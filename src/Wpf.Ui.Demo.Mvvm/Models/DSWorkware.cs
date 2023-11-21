// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using Wpf.Ui.Demo.Mvvm.Helpers;

namespace Wpf.Ui.Demo.Mvvm.Models;

public class DSWorkware
{
    public long id { get; set; }

    [Description("是否检测完成")] public bool IsCheck { get; set; } = false;
    public ProcessFlowEnum ProcessFlowEnum { get; set; }
    public DateTime CreateTime { get; set; }
    public List<DSWorkwareItem> DSWorkwareItems { get; set; } = new List<DSWorkwareItem>();
}

public class DSWorkwareItem
{
    public long Id { get; set; }

    public long WorkwareId { get; set; }
    public DSWorkware Dsworkware { get; set; }

    /// <summary>
    /// 设备ID
    /// </summary>
    public string? Equipment { get; set; }

    [Description("重复数据集")] public List<DSWorkwareArea> DSWorkwareAreas { get; set; } = new List<DSWorkwareArea>();

    [Description("标准压力数值")] public float StandardPressure { get; set; }
    [Description("标准温度数值")] public float StandardTemperature { get; set; }

    public bool IsCheck { get; set; } = true;
}

/// <summary>
/// Ds 数据域
/// </summary>
public class DSWorkwareArea
{
    public long Id { get; set; }
    public long DSWorkwareItemId { get; set; }

    /// <summary>
    /// 工装压力数据
    /// </summary>
    public float Pressure { get; set; }

    /// <summary>
    /// 工装温度数据
    /// </summary>
    public float Temperature { get; set; }

    public DSWorkwareItem DSWorkwareItem { get; set; }
}

/// <summary>
/// 页面显示Model
/// </summary>
public partial class DSWorkwareGridModel : ObservableObject
{
    [ObservableProperty] private int _serialNumber;

    /// <summary>
    /// 设备ID
    /// </summary>
    [ObservableProperty] private string? _equipment;

    /// <summary>
    /// 工装压力数据
    /// </summary>
    [ObservableProperty] private float? _pressure;

    /// <summary>
    /// 工装温度数据
    /// </summary>
    [ObservableProperty] private float? _temperature;

    [ObservableProperty] private bool _isCheck;
}