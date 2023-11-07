// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;

namespace Wpf.Ui.Demo.Mvvm.Controls;

/// <summary>
/// PortInfoControl.xaml 的交互逻辑
/// </summary>
public class PortInfoControl : Control
{
    public static readonly DependencyProperty TextNameProperty =
        DependencyProperty.Register(nameof(TextName), typeof(string), typeof(PortInfoControl));

    public static readonly DependencyProperty UnitProperty =
        DependencyProperty.Register(nameof(Unit), typeof(string), typeof(PortInfoControl));

    public static readonly DependencyProperty ValueProperty =
    DependencyProperty.Register(nameof(Value), typeof(string), typeof(PortInfoControl));

    public string? Value
    {
        get { return (string?)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    public string? Unit
    {
        get { return (string?)GetValue(UnitProperty); }
        set { SetValue(UnitProperty, value); }
    }

    public string TextName
    {
        get { return (string)GetValue(TextNameProperty); }
        set { SetValue(TextNameProperty, value); }
    }

}
