// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.IO.Ports;

namespace Wpf.Ui.Demo.Mvvm.Models;

public class SerialPortModel
{
    public string? PortName { get; set; }
    public int BaudRate { get; set; }
    public int DataBit { get; set; }
    public StopBits StopBit { get; set; }
    public string NetworkAddress { get; set; }

    public bool DeviceStatus { get; set; }
}