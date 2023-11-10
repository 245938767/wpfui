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
    public delegate Task<bool> OkFunction(DeviceCard deviceCard);

    private readonly OkFunction _okFunction;
    private readonly DeviceCard _deviceCard;

    public DevicePortConnectViewModel ViewModel { get; init; }

    public DevicePortConnectPage(DevicePortConnectViewModel viewModel, DeviceCard deviceCard, OkFunction okFunction)
    {
        _okFunction = okFunction;
        ViewModel = viewModel;
        DataContext = this;
        _deviceCard = deviceCard;
        InitializeComponent();
    }

    private void Cancel_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private async void OK_OnClickAsync(object sender, RoutedEventArgs e)
    {
        var ok = await _okFunction.Invoke(_deviceCard);
        if (ok)
        {
            Close();
        }
    }

    private void DropDown(object sender, MouseButtonEventArgs e)
    {
        ViewModel.PortList = SerialPort.GetPortNames();
    }
}