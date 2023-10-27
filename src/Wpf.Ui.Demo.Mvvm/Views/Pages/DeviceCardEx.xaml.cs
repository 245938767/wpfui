// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using System.Windows.Input;

namespace Wpf.Ui.Demo.Mvvm.Views.Pages;

public class DeviceCardEx : Control
{
    public static readonly DependencyProperty DeviceCardCommandProperty = DependencyProperty.Register(
        nameof(DeviceCardCommand),
        typeof(ICommand),
        typeof(DeviceCardEx),
        new PropertyMetadata(null)
    );

    public static readonly DependencyProperty DeviceNameProperty = DependencyProperty.Register(
        nameof(DeviceName),
        typeof(string),
        typeof(DeviceCardEx),
        new PropertyMetadata(null)
    );

    public static readonly DependencyProperty DeviceStatusProperty = DependencyProperty.Register(
        nameof(DeviceStatus),
        typeof(Visibility),
        typeof(DeviceCardEx),
        new FrameworkPropertyMetadata(Visibility.Hidden)
    );

    public static readonly DependencyProperty DeviceItemProperty = DependencyProperty.Register(
        nameof(DeviceItem),
        typeof(Dictionary<string, string>),
        typeof(DeviceCardEx),
        new PropertyMetadata(null)
    );

    public static readonly DependencyProperty DeviceImageProperty = DependencyProperty.Register(
        nameof(DeviceImage),
        typeof(string),
        typeof(DeviceCardEx),
        new PropertyMetadata(null)
    );

    public ICommand DeviceCardCommand
    {
        get => (ICommand)GetValue(DeviceCardCommandProperty);
        set => SetValue(DeviceCardCommandProperty, value);
    }

    public string DeviceName
    {
        get => (string)GetValue(DeviceNameProperty);
        set => SetValue(DeviceNameProperty, value);
    }

    public Visibility DeviceStatus
    {
        get => (Visibility)GetValue(DeviceStatusProperty);
        set => SetValue(DeviceStatusProperty, value);
    }

    public Dictionary<string, string> DeviceItem
    {
        get => (Dictionary<string, string>)GetValue(DeviceItemProperty);
        set => SetValue(DeviceItemProperty, value);
    }

    public string DeviceImage
    {
        get => (string)GetValue(DeviceImageProperty);
        set => SetValue(DeviceImageProperty, value);
    }
}