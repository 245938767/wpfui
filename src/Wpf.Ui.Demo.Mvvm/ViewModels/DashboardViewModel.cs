// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using System.IO.Ports;
using Wpf.Ui.Controls;
using Wpf.Ui.Demo.Mvvm.DeviceItem;
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
    private readonly IContentDialogService _contentDialogService;


    private bool _isInitialized = false;

    [ObservableProperty] private ObservableCollection<DeviceCard> _deviceCards = new();

    public DashboardViewModel(IContentDialogService contentDialogService)
    {
        _contentDialogService = contentDialogService;
    }


    [RelayCommand]
    private async void DeviceConnect(DeviceTypeEnum deviceTypeEnum)
    {
        var deviceCardList = DeviceCards.ToList();
        DeviceCard deviceCard = deviceCardList.First(x => x.Key == deviceTypeEnum);

        Console.WriteLine(
            $"INFO | {nameof(DashboardViewModel)} navigated, ({deviceCard.DeviceName}：{deviceCard.DeviceCardDetail.SerialPortModel.DeviceStatus})",
            "Wpf.Ui.Gallery"
        );

        if (deviceCard.DeviceCardDetail.SerialPortModel.DeviceStatus)
        {
            // TODO 检查主程序是否在运行（在运行不允许取消连接）

            ContentDialogResult result = await _contentDialogService.ShowSimpleDialogAsync(
                new SimpleContentDialogCreateOptions()
                {
                    Title = "关闭黑色版提醒", Content = "是否关闭设备连接", PrimaryButtonText = "确定", CloseButtonText = "取消",
                }
            );
            switch (result)
            {
                case ContentDialogResult.Primary:
                    // 关闭IDevice的连接状态
                    CloseConnection(deviceCard);
                    break;
                case ContentDialogResult.None:
                    break;
                case ContentDialogResult.Secondary:
                    break;
                default:
                    break;
            }

            return;
        }

        // 打开连接配置页面
        var devicePortConnectViewModel =
            new DevicePortConnectViewModel(deviceCard.DeviceCardDetail.SerialPortModel);
        var devicePortConnectPage =
            new DevicePortConnectPage(devicePortConnectViewModel, deviceCard, RunConnection);
        devicePortConnectPage.Show();
    }

    /// <summary>
    /// 运行连接数据
    /// </summary>
    private async void RunConnection(DeviceCard deviceCard)
    {
        // TODO 根据设备实例化对应对象
        IDevice instanceDeviceSerialPort = GlobalData.Instance.DeviceSerialPorts[deviceCard.Key];
        if (instanceDeviceSerialPort == null)
        {
            return;
        }

        // 开启对应的设备线程对象 open
        var open = await instanceDeviceSerialPort.Open();
        if (open == false)
        {
            var uiMessageBox = new Wpf.Ui.Controls.MessageBox
            {
                Title = "设备连接警告",
                Content =
                    $"请选择正确的端口，并且检查{deviceCard.DeviceCardDetail.SerialPortModel.PortName}端口不被占用",
            };

            await uiMessageBox.ShowDialogAsync();
            return;
        }

        deviceCard.DeviceCardDetail.SerialPortModel.DeviceStatus = true;
        var devices = DeviceCards.ToList();

        // 更新数据
        DeviceCards.Clear();
        foreach (DeviceCard deviceCardf in devices)
        {
            DeviceCards.Add(deviceCardf);
        }
    }

    /// <summary>
    /// 关闭连接
    /// </summary>
    /// <param name="deviceCard">关闭设备连接</param>
    private async void CloseConnection(DeviceCard deviceCard)
    {
        // TODO 关闭设备连接

        // TODO 根据设备实例化对应对象
        IDevice instanceDeviceSerialPort = GlobalData.Instance.DeviceSerialPorts[deviceCard.Key];
        if (instanceDeviceSerialPort == null)
        {
            return;
        }

        var closeConnect = await instanceDeviceSerialPort.CloseConnect();
        if (closeConnect)
        {
            deviceCard.DeviceCardDetail.SerialPortModel.DeviceStatus = false;
        }

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
        // TODO 获得本地缓存
        var pop = new DeviceCard
        {
            Key = DeviceTypeEnum.Pump,
            DeviceName = "真空泵",
            DeviceCardDetail = new DeviceCardDetail()
            {
                SerialPortModel = new SerialPortModel()
                {
                    PortName = null,
                    StopBit = StopBits.One,
                    BaudRate = 9600,
                    DataBit = 8,
                    NetworkAddress = "01",
                    DeviceStatus = false,
                },
            },
            ImageUrl = "pack://application:,,,/Assets/WinUiGallery/Pump.png"
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
                    StopBit = StopBits.One,
                    BaudRate = 9600,
                    DataBit = 8,
                    DeviceStatus = false,
                    NetworkAddress = "01"
                },
                CurrentPressure = 100f,
                CurrentTemperature = 20f
            },
            ImageUrl = "pack://application:,,,/Assets/WinUiGallery/Pressure.png"
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
                    StopBit = StopBits.One,
                    BaudRate = 9600,
                    DataBit = 8,
                    DeviceStatus = false,
                    NetworkAddress = "01"
                },
            },
            ImageUrl = "pack://application:,,,/Assets/WinUiGallery/Temperature.png"
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
                    StopBit = StopBits.One,
                    BaudRate = 9600,
                    DataBit = 8,
                    DeviceStatus = false,
                    NetworkAddress = "01"
                },
            },
            ImageUrl = "pack://application:,,,/Assets/WinUiGallery/Working.png"
        };
        DeviceCards.Add(pop);
        DeviceCards.Add(pressure);
        DeviceCards.Add(temperature);
        DeviceCards.Add(work);

        // 初始化设数据
        Dictionary<DeviceTypeEnum, IDevice> instanceDeviceSerialPorts = GlobalData.Instance.DeviceSerialPorts;
        instanceDeviceSerialPorts.Add(DeviceTypeEnum.Pressure, new PressureDevice(pressure));
        instanceDeviceSerialPorts.Add(DeviceTypeEnum.Pump, new PumpDevice(pop));
        instanceDeviceSerialPorts.Add(DeviceTypeEnum.Temperature, new TemperatureDevice(temperature));
        // TODO 初始化流程信息
    }
}