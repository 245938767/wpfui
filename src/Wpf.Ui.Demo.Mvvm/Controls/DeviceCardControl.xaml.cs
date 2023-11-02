// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Ui.Demo.Mvvm.Helpers;
using Wpf.Ui.Demo.Mvvm.Models;

namespace Wpf.Ui.Demo.Mvvm.Controls;

/// <summary>
/// UserControl1.xaml 的交互逻辑
/// </summary>
public class DeviceCardControl : Control
{
    public static readonly DependencyProperty DeviceNameProperty = DependencyProperty.Register(
        nameof(DeviceName),
        typeof(string),
        typeof(DeviceCardControl),
        new PropertyMetadata(null)
    );

    public static readonly DependencyProperty ImageUrlProperty = DependencyProperty.Register(
        nameof(ImageUrl),
        typeof(string),
        typeof(DeviceCardControl),
        new PropertyMetadata(null)
    );

    public static readonly DependencyProperty DeviceCardDetailProperty = DependencyProperty.Register(
        nameof(DeviceCardDetail),
        typeof(DeviceCardDetail),
        typeof(DeviceCardControl),
        new PropertyMetadata(null)
    );

    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        nameof(Command),
        typeof(ICommand),
        typeof(DeviceCardControl),
        new PropertyMetadata(null)
    );

    public static readonly DependencyProperty DeviceTypeProperty = DependencyProperty.Register(
        nameof(DeviceType),
        typeof(DeviceTypeEnum),
        typeof(DeviceCardControl),
        new PropertyMetadata(null)
    );

    public string? DeviceName
    {
        get => (string?)GetValue(DeviceNameProperty);
        set => SetValue(DeviceNameProperty, value);
    }

    public string? ImageUrl
    {
        get => (string?)GetValue(ImageUrlProperty);
        set => SetValue(ImageUrlProperty, value);
    }

    public DeviceCardDetail? DeviceCardDetail
    {
        get => (DeviceCardDetail?)GetValue(DeviceCardDetailProperty);
        set => SetValue(DeviceCardDetailProperty, value);
    }

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public DeviceTypeEnum DeviceType
    {
        get => (DeviceTypeEnum)GetValue(DeviceTypeProperty);
        set => SetValue(DeviceTypeProperty, value);
    }
}