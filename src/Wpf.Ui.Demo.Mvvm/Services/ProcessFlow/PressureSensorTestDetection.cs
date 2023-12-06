using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Wpf.Ui.Demo.Mvvm.DeviceItem;
using Wpf.Ui.Demo.Mvvm.Helpers;
using Wpf.Ui.Demo.Mvvm.Helpers.Extension;
using Wpf.Ui.Demo.Mvvm.Models;
using Wpf.Ui.Demo.Mvvm.ViewModels;

namespace Wpf.Ui.Demo.Mvvm.Services.ProcessFlow;

class PressureSensorTestDetection : IProcessFlow
{
    private CancellationTokenSource? _cancellation;
    private readonly PumpDevice pumpDevice;
    private readonly PressureDevice pressureDevice;
    private readonly TemperatureDevice temperatureDevice;
    private readonly PressureSensorWorkwareDevice pressureSensorWorkwareDevice;
    private readonly StandardService standardService;
    private readonly DSWorkwareService dSWorkwareService;
    private readonly ProcessFlowEnum processFlow;

    /// <summary>
    /// 初始化测试流程
    /// </summary>
    /// <param name="processFlow">流程类型</param>
    /// <param name="homePageItemData">显示绑定对象</param>
    public PressureSensorTestDetection(ProcessFlowEnum processFlow, ObservableCollection<object> homePageItemData) : base(
        processFlow, homePageItemData)
    {
        this.processFlow = processFlow;
        dSWorkwareService = App.GetService<DSWorkwareService>()!;
        standardService = App.GetService<StandardService>()!;
        Dictionary<DeviceTypeEnum, IDevice> deviceSerialPorts = GlobalData.Instance.DeviceSerialPorts;
        pressureSensorWorkwareDevice = (PressureSensorWorkwareDevice)deviceSerialPorts[Helpers.DeviceTypeEnum.PressureSensor];
        temperatureDevice = (TemperatureDevice)deviceSerialPorts[Helpers.DeviceTypeEnum.Temperature];
        pressureDevice = (PressureDevice)deviceSerialPorts[Helpers.DeviceTypeEnum.Pressure];
        pumpDevice = (PumpDevice)deviceSerialPorts[Helpers.DeviceTypeEnum.Pump];
    }


    public override bool CheckExecution()
    {
        if (_cancellation == null)
        {
            return false;
        }

        return !_cancellation.IsCancellationRequested;
    }

    public override void Dispose()
    {
        _cancellation?.Cancel();
    }

    public override async Task<bool> ExecutionDetection()
    {
        // 设备检测
        if (!pressureDevice.IsConnection())
        {
            await ShowDeviceConnnectionError("压力源");
            return false;
        }

        if (!pumpDevice.IsConnection())
        {
            await ShowDeviceConnnectionError("真空泵");
            return false;
        }

        if (!temperatureDevice.IsConnection())
        {
            await ShowDeviceConnnectionError("温箱");
            return false;
        }

        if (!pressureSensorWorkwareDevice.IsConnection())
        {
            await ShowDeviceConnnectionError("压力传感器工装");
            return false;
        }

        // TODO 获得配置数据
        _ = await pressureDevice.SetCurrentStatus(150, 0.01f, 60);
        pressureDevice.SetCurrentPressureLook();
        await Task.Delay(30000);
        var check = pressureDevice.CheckAround(150, 0.1f);
        if (!check)
        {
            await ShowDeviceProcessErrorMessages("压力检测失败");
            return check;

        }
        return check;
    }

    public override async Task ExecutionProcess()
    {
        /**   if (!await ExecutionDetection())
           {
               GlobalData.Instance.IsOpenCheck = false;
               return;
           }
        */
        GlobalData.Instance.ProcessBar = 5;
        // 检测当前是否有缓存数据

        // 数据加载缓存的，DoMain ID,加载日志记录

        // 步骤使用X,Y进行记录，并且记录当前的测试数据，再次加载时，根据X，Y的标记点去除之前的数据

        // 加载当前此测用例的缓存步骤数据
        GlobalData.Instance.IsOpenCheck = true;

        _cancellation = new CancellationTokenSource();

        // 获得测试标准数据和阈值
        Standard? standard = standardService.GetStandard(processFlow);
        if (standard == null)
        {
            _cancellation.Cancel();

            // 测试数据为空
            await ShowDeviceProcessErrorMessages("无测试数据");
            GlobalData.Instance.IsOpenCheck = false;
            GlobalData.Instance.ProcessBar = 0;

            return;
        }

        GlobalData.Instance.ProcessBar = 8;
        // 初始化当前测试数据基类对象
        var dSWorkware = new DSWorkware { ProcessFlowEnum = processFlow, CreateTime = DateTime.Now, };
        dSWorkwareService.SaveDSWorkware(dSWorkware);

        // pressure
        var pressureList = standard.StandarDatas.Where(o => o.StandardType == StandardEnum.Pressure).ToList();

        // temperature
        var temperatureList = standard.StandarDatas.Where(o => o.StandardType == StandardEnum.Temperature).ToList();
        GlobalData.Instance.ProcessBar = 10;

        // 80% weight
        var weight = 80 / temperatureList.Count;
        foreach (StandardData temperature in temperatureList)
        {
            // 4 份
            var temWeight = weight / 4;
            pressureDevice.SetCurrentPressureLook();

            if (await temperatureDevice.SetCurrentStatus(temperature.Value, temperature.ThresholdValue))
            {
            }

            GlobalData.Instance.ProcessBar += temWeight;

            if (_cancellation.IsCancellationRequested)
            {
                GlobalData.Instance.ProcessBar = 0;
                GlobalData.Instance.IsOpenCheck = false;

                return;
            }

            // 等待工装达标（2小时）
            try
            {
                // 2小时token 8200 s
                using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(2));

                await await Task.Factory.StartNew(
                  async () =>
                  {
                      while (!_cancellation.IsCancellationRequested && !cancellationTokenSource.IsCancellationRequested)
                      {
                          await Task.Delay(5000);
                      }

                  },
                  cancellationTokenSource.Token);
            }
            catch (TaskCanceledException)
            {
            }

            if (_cancellation.IsCancellationRequested)
            {
                GlobalData.Instance.ProcessBar = 0;
                GlobalData.Instance.IsOpenCheck = false;

                return;
            }

            GlobalData.Instance.ProcessBar += temWeight;

            var pressureWright = (temWeight * 2) / 100;
            foreach (StandardData pressure in pressureList)
            {
                if (_cancellation.IsCancellationRequested)
                {
                    GlobalData.Instance.ProcessBar = 0;
                    GlobalData.Instance.IsOpenCheck = false;

                    return;
                }

                // 测试压力小于105 开启真空泵
                if (pressure.Value < 105)
                {
                    pumpDevice.OpenPump();
                }
                else
                {
                    pumpDevice.ClosePump();
                }

                if (await pressureDevice.SetCurrentStatus(pressure.Value, pressure.ThresholdValue))
                {
                }

                var homePageItem = HomePageItemData.ToList();
                var dSWorkwareItems = new List<DSWorkwareItem>();

                // 获得N次数据
                for (var n = 0; n < 50; n++)
                {
                    // 获得设备数据
                    for (var i = 0; i < homePageItem.Count; i++)
                    {
                        var data = (DSWorkwareGridModel)homePageItem[i];
                 
                        if (n <= 0)
                        {
                            // 初始化数据
                            dSWorkwareItems.Insert(i, new DSWorkwareItem
                            {
                                Equipment = data.SerialNumber.ToString(),
                                StandardPressure = pressure.Value,
                                StandardTemperature = temperature.Value,
                                IsCheck=true,
                            });
                        }

                        DSWorkwareItem dSWorkwareItem = dSWorkwareItems[i];
                        var dataPressure = data.Pressure ?? 0;
                        var dataTemperature = data.Temperature ?? 0;
                        dSWorkwareItem.DSWorkwareAreas.Add(new DSWorkwareArea
                        {
                            Pressure = dataPressure,
                            Temperature = dataTemperature
                        });

                        // 检测数据是否合格
                        if (!(dataPressure.CheckAround(pressure.Value, pressure.ThresholdValue) &&
                              dataTemperature.CheckAround(temperature.Value, temperature.ThresholdValue)))
                        {
                            dSWorkwareItem.IsCheck = false;
                        }

                    }

                    await Task.Delay(1000);
                }

                dSWorkware.DSWorkwareItems.AddRange(dSWorkwareItems);
                _ = dSWorkwareService.UpdateDSWorkware(dSWorkware);
                GlobalData.Instance.ProcessBar += pressureWright;
            }
        }

        // 检测数据校验
        dSWorkware.IsCheck = true;
        _ = dSWorkwareService.UpdateDSWorkware(dSWorkware);
        var dictionary = dSWorkware.DSWorkwareItems.GroupBy(o => o.Equipment)
            .ToDictionary(o => o.Key, o => o.Any(x => !x.IsCheck));

        // 更新页面
        ObservableCollection<object> homePageItemData = HomePageItemData;

        for (var i = 0; i < homePageItemData.Count; i++)
        {
            var v = (DSWorkwareGridModel)homePageItemData[i];

            // 检测是否合格
            v.IsCheck = !dictionary[v.SerialNumber.ToString()];
        }

        GlobalData.Instance.ProcessBar = 100;
        // 压力源进入观测模式
        await pressureDevice.CloseStatus();
        // 温箱关闭
        temperatureDevice.CloseTemperature();
        GlobalData.Instance.IsOpenCheck = false;
    }
}