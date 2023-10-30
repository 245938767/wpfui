// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Media.Imaging;
using Wpf.Ui.Demo.Mvvm.Helpers;

namespace Wpf.Ui.Demo.Mvvm.Models;

public class DeviceCard
{
    public DeviceTypeEnum Key { get; set; }

    public string DeviceName { get; set; }

    public string ImageUrl { get; set; }

    public DeviceCardDetail DeviceCardDetail { get; set; }
}