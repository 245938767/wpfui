// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Microsoft.EntityFrameworkCore;
using Wpf.Ui.Demo.Mvvm.Helpers;

namespace Wpf.Ui.Demo.Mvvm.Models;

public partial class DeviceCard : ObservableObject
{
    public int Id { get; set; }
     public int ForeignKey { get; set; }

    [ObservableProperty]
    private DeviceTypeEnum _key;
    [ObservableProperty]
    private string _deviceName;
    [ObservableProperty]

    private string _imageUrl;
    [ObservableProperty]
    private SerialPortModel? _serialPortModel;
    [ObservableProperty]

    private float? _currentTemperature;
    [ObservableProperty]

    private float? _currentPressure;
    [ObservableProperty]

    private float? _settingTemperature;
    [ObservableProperty]

    private float? _settingPressure;

    /// <summary>
    /// 温度单价
    /// </summary>
    [ObservableProperty]
    private string? _unitT;
    /// <summary>
    /// 压力单价
    /// </summary>
    [ObservableProperty]
    private string? _unitP;

    /// <summary>
    /// Gets or sets 数据版本（设备更新字段时本地数据也需要更新）
    /// </summary>
    public float Version { get; set; }

    /// <summary>
    /// 清理有状态数据
    /// </summary>
    public void ClearHasStatusData() {
        CurrentPressure = null;
        CurrentTemperature = null;
        SettingPressure = null;
        SettingTemperature = null;
        if (SerialPortModel != null)
        {
            SerialPortModel.DeviceStatus = false;
        }
    }
}