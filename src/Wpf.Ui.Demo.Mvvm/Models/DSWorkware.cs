// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;

namespace Wpf.Ui.Demo.Mvvm.Models;

public class DSWorkware {
    public long Id { get; set; }
    /// 标准对象外键
    /// </summary>
    public int StandardId { get; set; }
    //标准数据
    [Description("标准数据")]
    public Standard? Standard { get; set; }
    [Description("数据集")]
    public List<DSWorkwareArea>? DSWorkwareAreas { get; set; }
    [Description("是否检测完成")]
    public bool IsCheck { get; set; }
}

/// <summary>
/// Ds 数据域
/// </summary>
public  class DSWorkwareArea
{
    public long Id { get; set; }
    public long DSWorkwareId { get; set; }
    /// <summary>
    /// 设备ID
    /// </summary>
    public string? Equipment { get; set; }

    /// <summary>
    /// 工装压力数据
    /// </summary>
    public float? Pressure { get; set; }

    /// <summary>
    /// 工装温度数据
    /// </summary>
    public float? Temperature { get; set; }

}

/// <summary>
/// 页面显示Model
/// </summary>
public partial class DSWorkwareGridModel : ObservableObject
{
    [ObservableProperty]
    private int _serialNumber;
    /// <summary>
    /// 设备ID
    /// </summary>
    [ObservableProperty]
    private string? _equipment;

    /// <summary>
    /// 工装压力数据
    /// </summary>
    [ObservableProperty]
    private float? _pressure;

    /// <summary>
    /// 工装温度数据
    /// </summary>
    [ObservableProperty]
    private float? _temperature;
    [ObservableProperty]
    private bool _isCheck;
}
