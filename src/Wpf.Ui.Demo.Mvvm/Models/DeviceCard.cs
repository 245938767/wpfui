// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Demo.Mvvm.Helpers;

namespace Wpf.Ui.Demo.Mvvm.Models;

public partial class DeviceCard : ObservableObject
{
    [ObservableProperty]
    private DeviceTypeEnum _key;
    [ObservableProperty]
    private string _deviceName;
    [ObservableProperty]

    private string _imageUrl;
    [ObservableProperty]
    private SerialPortModel _serialPortModel;
    [ObservableProperty]

    private float? _currentTemperature;
    [ObservableProperty]

    private float? _currentPressure;
    [ObservableProperty]

    private float? _settingTemperature;
    [ObservableProperty]

    private float? _settingPressure;

    /// <summary>
    /// 单价
    /// </summary>
    [ObservableProperty]

    private string _unit;
}