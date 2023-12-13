// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using ClosedXML.Excel;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Threading;
using Wpf.Ui.Controls;
using Wpf.Ui.Demo.Mvvm.DeviceItem;
using Wpf.Ui.Demo.Mvvm.Helpers;
using Wpf.Ui.Demo.Mvvm.Models;
using Wpf.Ui.Demo.Mvvm.Services;
using Wpf.Ui.Demo.Mvvm.Services.ProcessFlow;
using Wpf.Ui.Demo.Mvvm.Views.Pages;

namespace Wpf.Ui.Demo.Mvvm.ViewModels;

/// <summary>
/// 首页连接和展示
/// </summary>
public partial class DashboardViewModel : ObservableObject, INavigationAware
{
    private readonly IContentDialogService _contentDialogService;
    private readonly DeviceService _deviceService;
    private readonly DSWorkwareService dSWorkwareService;
    private bool _isInitialized = false;

    [ObservableProperty] private List<DeviceCard> _deviceCards = new();

    [ObservableProperty]
    private ProcessFlowEnum _processFlow = ProcessFlowEnum.DSTest;
    /// <summary>
    /// 首页表格数据
    /// </summary>
    [ObservableProperty] private ObservableCollection<object> _homePageItemData = new ObservableCollection<object>();

    public DashboardViewModel(IContentDialogService contentDialogService, DeviceService deviceService, DSWorkwareService dSWorkwareService)
    {
        _contentDialogService = contentDialogService;
        _deviceService = deviceService;
        this.dSWorkwareService = dSWorkwareService;

    }

    [RelayCommand]
    private async Task DeviceConnectAsync(DeviceCard deviceCard)
    {
        Console.WriteLine(
            $"INFO | {nameof(DashboardViewModel)} navigated, ({deviceCard.DeviceName}：{deviceCard.SerialPortModel.DeviceStatus})",
            "Wpf.Ui.Gallery"
        );

        if (deviceCard.SerialPortModel.DeviceStatus)
        {
            ContentDialogResult result = await _contentDialogService.ShowSimpleDialogAsync(
                new SimpleContentDialogCreateOptions()
                {
                    Title = "关闭设备提醒",
                    Content = "是否关闭设备连接",
                    PrimaryButtonText = "确定",
                    CloseButtonText = "取消",
                }
            );
            switch (result)
            {
                case ContentDialogResult.Primary:
                    // 关闭IDevice的连接状态
                    await CloseConnection(deviceCard);
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
            new DevicePortConnectViewModel(deviceCard.SerialPortModel);
        var devicePortConnectPage =
            new DevicePortConnectPage(devicePortConnectViewModel, deviceCard, RunConnection);
        devicePortConnectPage.Show();
    }

    /// <summary>
    /// 运行设备连接数据
    /// </summary>
    private async Task<bool> RunConnection(DeviceCard deviceCard)
    {
        // TODO 根据设备实例化对应对象
        if (!GlobalData.Instance.DeviceSerialPorts.ContainsKey(deviceCard.Key))
        {
            _ = await new Ui.Controls.MessageBox
            {
                Title = "设备连接失败警告",
                Content =
                    "目前未对此设备支持敬请期待...",
            }.ShowDialogAsync();
            return false;
        }

        IDevice instanceDeviceSerialPort = GlobalData.Instance.DeviceSerialPorts[deviceCard.Key];

        // 开启对应的设备线程对象 open
        var open = await instanceDeviceSerialPort.Open();
        if (open == false)
        {
            var uiMessageBox = new Wpf.Ui.Controls.MessageBox
            {
                Title = "设备连接失败警告",
                Content =
                    $"请选择正确的端口，并且检查{deviceCard.SerialPortModel.PortName}端口未被占用",
            };

            _ = await uiMessageBox.ShowDialogAsync();
            return false;
        }

        deviceCard.SerialPortModel.DeviceStatus = true;
        _deviceService.UpdateLocaltionData(deviceCard);
        return true;
    }

    /// <summary>
    /// 关闭设备连接
    /// </summary>
    /// <param name="deviceCard">关闭设备连接</param>
    private async Task CloseConnection(DeviceCard deviceCard)
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
            deviceCard.SerialPortModel.DeviceStatus = false;
        }
        _deviceService.UpdateLocaltionData(deviceCard);


    }

    /// <summary>
    /// 开始检测按钮
    /// </summary>
    [RelayCommand]
    private async Task StartCheck()
    {
        if (!GlobalData.Instance.ProcessFlow.ContainsKey(ProcessFlow))
        {
            var uiMessageBox = new Wpf.Ui.Controls.MessageBox
            {
                Title = "功能未开通提醒",
                Content =
             $"此测试功能未开通敬请期待...",
            };
            _ = await uiMessageBox.ShowDialogAsync();
            return;
        }
        IProcessFlow processFlow = GlobalData.Instance.ProcessFlow[ProcessFlow];

        // 检查主程序是否在运行（在运行不允许再次连接）
        if (processFlow.CheckExecution())
        {
            var uiMessageBox = new Wpf.Ui.Controls.MessageBox
            {
                Title = "执行测试错误",
                Content =
                 $"测试正在执行",
            };
            _ = await uiMessageBox.ShowDialogAsync();
            return;
        }

        ContentDialogResult result = await _contentDialogService.ShowSimpleDialogAsync(
            new SimpleContentDialogCreateOptions()
            {
                Title = "开始检测提醒",
                Content = $"是否开始测试: {ProcessFlow.ToDescription()}",
                PrimaryButtonText = "确定",
                CloseButtonText = "取消",
            }
        );
        if (result == ContentDialogResult.Primary)
        {
            LoggerHelper.Instance.Log($"开始进行检测 {ProcessFlow.ToDescription()}");
            // TODO 根据流程实例化对应的决策
            await processFlow.ExecutionProcess();

        }
        else
        {
            return;
        }
    }
    /// <summary>
    /// 取消检测（中断检测）
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private async Task ColseCheck()
    {
        ContentDialogResult result = await _contentDialogService.ShowSimpleDialogAsync(
         new SimpleContentDialogCreateOptions()
         {
             Title = "中断检测提醒",
             Content = $"是否中断测试: {ProcessFlow.ToDescription()}",
             PrimaryButtonText = "确定",
             CloseButtonText = "取消",
         }
     );
        if (result == ContentDialogResult.Primary)
        {
            GlobalData.Instance.IsOpenCheck = false;
            IProcessFlow processFlow = GlobalData.Instance.ProcessFlow[ProcessFlow];
            processFlow.Dispose();
        }
    }

    [RelayCommand]
    private async Task ExportExcel()
    {
        // check history data
        DSWorkware? dSWorkware = dSWorkwareService.GetNewsData();
        if (dSWorkware == null)
        {
            return;
        }

        // 提示用户History信息和时间
        ContentDialogResult result = await _contentDialogService.ShowSimpleDialogAsync(
          new SimpleContentDialogCreateOptions()
          {
              Title = "开始生成提醒",
              Content = $"是开始生成数据: 测试流程:{dSWorkware.ProcessFlowEnum.ToDescription()}, 创建时间为:{dSWorkware.CreateTime} ,是否检测完成:{dSWorkware.IsCheck}",
              PrimaryButtonText = "确定",
              CloseButtonText = "取消",
          }
      );
        if (result == ContentDialogResult.Primary)
        {
            // 继续生成Excel文件
            //循环设备信息
            var dsWorkwareItems = dSWorkware.DSWorkwareItems.GroupBy(o => o.Equipment)
                                                            .ToDictionary(o => o.Key, o => o.ToList());
            DateTime now = DateTime.Now;
            var times = now.Year.ToString() + now.Month.ToString() + now.Day.ToString() + now.Hour.ToString() + now.Minute.ToString() + now.Second.ToString();
            foreach (KeyValuePair<string, List<DSWorkwareItem>> item in dsWorkwareItems)
            {

                // 根据设备的循环生成对应的设备Excel报告
                using var xlWorkBook = new XLWorkbook();

                // 根据温度生成Sheet
                var temperatureGroup = item.Value.GroupBy(o => o.StandardTemperature).ToDictionary(o => o.Key, o => o.ToList());
                foreach (KeyValuePair<float, List<DSWorkwareItem>> temperatureItem in temperatureGroup)
                {
                    var xml = xlWorkBook.AddWorksheet(temperatureItem.Key.ToString());

                    // 循环压力数据
                    // 行标记
                    var pressureCount = 1;
                    foreach (DSWorkwareItem pressureItem in temperatureItem.Value)
                    {
                        // 列标记
                        var count = 1;
                        xml.Cell(count++, pressureCount).Value = pressureItem.StandardPressure;
                        foreach (DSWorkwareArea relaData in pressureItem.DSWorkwareAreas)
                        {
                            xml.Cell(count++, pressureCount).Value = ((relaData.Pressure - pressureItem.StandardPressure) / pressureItem.StandardPressure).ToString("#0.0000");
                        }

                        pressureCount++;
                    }
                }

                // save xmal
                var fileName = $"C:\\xml\\{times}\\{item.Key}.xlsx";
                xlWorkBook.SaveAs(fileName);
            }
            _ = _contentDialogService.ShowSimpleDialogAsync(
    new SimpleContentDialogCreateOptions()
    {
        Title = "已生成提醒",
        Content = $"生成路径：C:\\xml\\{times}",
        PrimaryButtonText = "确定",
        CloseButtonText = "取消",
    }
);
        }
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
        // 获得本地设备缓存
        List<DeviceCard> deviceCards = _deviceService.GetLocaltionData();
        DeviceCards = deviceCards;

        // 初始化设数据
        Dictionary<DeviceTypeEnum, IDevice> instanceDeviceSerialPorts = GlobalData.Instance.DeviceSerialPorts;

        instanceDeviceSerialPorts.Add(DeviceTypeEnum.Pressure, new PressureDevice(deviceCards.First(x => x.Key == DeviceTypeEnum.Pressure)));
        instanceDeviceSerialPorts.Add(DeviceTypeEnum.Pump, new PumpDevice(deviceCards.First(x => x.Key == DeviceTypeEnum.Pump)));
        instanceDeviceSerialPorts.Add(DeviceTypeEnum.Temperature, new TemperatureDevice(deviceCards.First(x => x.Key == DeviceTypeEnum.Temperature)));
        instanceDeviceSerialPorts.Add(DeviceTypeEnum.DSWork, new DSWorkwareDevice(deviceCards.First(x => x.Key == DeviceTypeEnum.DSWork), HomePageItemData));
        instanceDeviceSerialPorts.Add(DeviceTypeEnum.PressureSensor, new PressureSensorWorkwareDevice(deviceCards.First(x => x.Key == DeviceTypeEnum.PressureSensor), HomePageItemData));

        // TODO 初始化 流程逻辑类
        GlobalData.Instance.ProcessFlow.Add(ProcessFlowEnum.DSTest, new DSTestDetection(ProcessFlowEnum.DSTest, HomePageItemData));
        GlobalData.Instance.ProcessFlow.Add(ProcessFlowEnum.PressureSensorTest, new PressureSensorTestDetection(ProcessFlowEnum.PressureSensorTest, HomePageItemData));
    }


}