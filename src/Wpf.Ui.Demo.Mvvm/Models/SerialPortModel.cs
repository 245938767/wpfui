// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.IO.Ports;

namespace Wpf.Ui.Demo.Mvvm.Models;
[Owned]
public partial class SerialPortModel : ObservableObject
{

    [ObservableProperty]
    private string? _portName;
    [ObservableProperty]
    private int _baudRate;
    [ObservableProperty]
    private int _dataBit;
    [ObservableProperty]
    private StopBits _stopBit;
    [ObservableProperty]
    private string? _networkAddress;
    [ObservableProperty]
    private bool _deviceStatus;
}