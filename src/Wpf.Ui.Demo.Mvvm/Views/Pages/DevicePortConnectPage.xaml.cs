// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Input;
using Boiido.IO.SerialPort;
using Wpf.Ui.Demo.Mvvm.ViewModels;

namespace Wpf.Ui.Demo.Mvvm.Views.Pages;

public partial class DevicePortConnectPage
{
    public DevicePortConnectViewModel ViewModel { get; init; }

    public DevicePortConnectPage(DevicePortConnectViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }


    private void WindowsClosing()
    {
        Close();
    }

    private void DropDown(object sender, MouseButtonEventArgs e)
    {
        ViewModel.PortList = SerialPort.portArray;
        ViewModel.PortList = new string[] { "Com0", "Com2", "Com3", "com4" };
    }
}