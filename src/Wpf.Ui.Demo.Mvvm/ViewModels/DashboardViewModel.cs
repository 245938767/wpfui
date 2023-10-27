// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using Wpf.Ui.Controls;
using Wpf.Ui.Demo.Mvvm.Helpers;
using Wpf.Ui.Demo.Mvvm.Models;
using Wpf.Ui.Demo.Mvvm.Services;
using Wpf.Ui.Demo.Mvvm.Views.Pages;

namespace Wpf.Ui.Demo.Mvvm.ViewModels;

/// <summary>
/// 首页连接和展示
/// </summary>
public partial class DashboardViewModel : ObservableObject, INavigationAware
{
    private readonly WindowsProviderService _windowsProviderService;


    private bool _isInitialized = false;

    [ObservableProperty] private ObservableCollection<DeviceCard> _deviceCards = new ObservableCollection<DeviceCard>();

    public DashboardViewModel(WindowsProviderService windowsProviderService)
    {
        _windowsProviderService = windowsProviderService;
    }

    [RelayCommand]
    private void DeviceConnect(DeviceTypeEnum deviceTypeEnum)
    {
        var deviceCardList = DeviceCards.ToList();
        DeviceCard deviceCard = deviceCardList.First(x => x.Key == deviceTypeEnum);

        Console.WriteLine(
            $"INFO | {nameof(DashboardViewModel)} navigated, ({deviceCard.DeviceName}：{deviceCard.DeviceCardDetail.SerialPortModel.DeviceStatus})",
            "Wpf.Ui.Gallery"
        );

        if (deviceCard.DeviceCardDetail.SerialPortModel.DeviceStatus)
        {
            return;
        }

        // 打开连接配置页面
        var devicePortConnectViewModel =
            new DevicePortConnectViewModel(deviceCard.DeviceCardDetail.SerialPortModel);
        var devicePortConnectPage =
            new DevicePortConnectPage(devicePortConnectViewModel, RunConnection);
        devicePortConnectPage.Show();
    }

    /// <summary>
    /// 运行连接数据
    /// </summary>
    private void RunConnection(DevicePortConnectViewModel devicePortConnectViewModel)
    {
        devicePortConnectViewModel.SerialPortModel.DeviceStatus = true;
        var devices = DeviceCards.ToList();

        // 更新数据
        DeviceCards.Clear();
        foreach (DeviceCard deviceCardf in devices)
        {
            DeviceCards.Add(deviceCardf);
        }
    }

    [RelayCommand]
    private void StartCheck()
    {
    }

    public void OnNavigatedTo()
    {
        Console.WriteLine(_isInitialized);
        if (!_isInitialized)
        {
            InitializeViewModel();

            _isInitialized = true;
        }
    }

    public void OnNavigatedFrom()
    {
    }

    private void InitializeViewModel()
    {
        //获得本地缓存
        //执行初始数据
        var pop = new DeviceCard
        {
            Key = DeviceTypeEnum.Pump,
            DeviceName = "真空泵",
            DeviceCardDetail = new DeviceCardDetail()
            {
                SerialPortModel = new SerialPortModel()
                {
                    PortName = null,
                    StopBit = 1,
                    BaudRate = 9600,
                    DataBit = 8,
                    NetworkAddress = "01",
                    DeviceStatus = false,
                },
            },
            ImageUrl = "pack://application:,,,/Assets/WinUiGallery/Button.png"
        };
        var pressure = new DeviceCard
        {
            Key = DeviceTypeEnum.Pressure,
            DeviceName = "压力源",
            DeviceCardDetail = new DeviceCardDetail()
            {
                SerialPortModel = new SerialPortModel()
                {
                    PortName = null,
                    StopBit = 1,
                    BaudRate = 9600,
                    DataBit = 8,
                    DeviceStatus = false,
                    NetworkAddress = "01"
                },
                CurrentPressure = "100kpa",
                CurrentTemperature = "20C"
            },
            ImageUrl = "pack://application:,,,/Assets/WinUiGallery/Button.png"
        };
        var temperature = new DeviceCard
        {
            Key = DeviceTypeEnum.Temperature,
            DeviceName = "温箱",
            DeviceCardDetail = new DeviceCardDetail()
            {
                SerialPortModel = new SerialPortModel()
                {
                    PortName = null,
                    StopBit = 1,
                    BaudRate = 9600,
                    DataBit = 8,
                    DeviceStatus = false,
                    NetworkAddress = "01"
                },
            },
            ImageUrl = "pack://application:,,,/Assets/WinUiGallery/Flyout.png"
        };
        var work = new DeviceCard
        {
            Key = DeviceTypeEnum.Work,
            DeviceName = "工装",
            DeviceCardDetail = new DeviceCardDetail()
            {
                SerialPortModel = new SerialPortModel()
                {
                    PortName = null,
                    StopBit = 1,
                    BaudRate = 9600,
                    DataBit = 8,
                    DeviceStatus = false,
                    NetworkAddress = "01"
                },
            },
            ImageUrl = "pack://application:,,,/Assets/WinUiGallery/MenuBar.png"
        };
        DeviceCards.Add(pop);
        DeviceCards.Add(pressure);
        DeviceCards.Add(temperature);
        DeviceCards.Add(work);
    }
}