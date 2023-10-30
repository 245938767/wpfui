// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.IO.Ports;
using System.Windows.Input;
using Wpf.Ui.Demo.Mvvm.Models;
using Wpf.Ui.Demo.Mvvm.ViewModels;

namespace Wpf.Ui.Demo.Mvvm.Views.Pages;

public partial class DevicePortConnectPage
{
    public delegate void OkFunction(DeviceCard deviceCard);

    private OkFunction _okFunction;
    private readonly DeviceCard DeviceCard;
    public DevicePortConnectViewModel ViewModel { get; init; }

    public DevicePortConnectPage(DevicePortConnectViewModel viewModel, DeviceCard deviceCard, OkFunction okFunction)
    {
        _okFunction = okFunction;
        ViewModel = viewModel;
        DataContext = this;
        DeviceCard = deviceCard;

        InitializeComponent();
    }


    private void Cancel_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void OK_OnClick(object sender, RoutedEventArgs e)
    {
        _okFunction.Invoke(DeviceCard);
        Close();
    }

    private void DropDown(object sender, MouseButtonEventArgs e)
    {
        ViewModel.PortList = SerialPort.GetPortNames();
        ViewModel.PortList = new[] { "Com0", "Com2", "Com3", "com4" };
    }
}