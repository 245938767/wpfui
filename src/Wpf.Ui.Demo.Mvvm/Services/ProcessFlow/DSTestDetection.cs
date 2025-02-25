using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Services.Maps;
using Wpf.Ui.Demo.Mvvm.DeviceItem;
using Wpf.Ui.Demo.Mvvm.Helpers;
using Wpf.Ui.Demo.Mvvm.Helpers.Extension;
using Wpf.Ui.Demo.Mvvm.Models;
using Wpf.Ui.Demo.Mvvm.ViewModels;

namespace Wpf.Ui.Demo.Mvvm.Services.ProcessFlow;

class DSTestDetection : IProcessFlow
{
    private CancellationTokenSource? _cancellation;
    private readonly PumpDevice pumpDevice;
    private readonly PressureDevice pressureDevice;
    private readonly TemperatureDevice temperatureDevice;
    private readonly DSWorkwareDevice dSWorkwareDevice;
    private readonly StandardService standardService;
    private readonly DSWorkwareService dSWorkwareService;
    private readonly ProcessFlowEnum processFlow;

    /// <summary>
    /// 初始化测试流程
    /// </summary>
    /// <param name="processFlow">流程类型</param>
    /// <param name="homePageItemData">显示绑定对象</param>
    public DSTestDetection(ProcessFlowEnum processFlow, ObservableCollection<object> homePageItemData) : base(
        processFlow, homePageItemData)
    {
        this.processFlow = processFlow;
        dSWorkwareService = App.GetService<DSWorkwareService>()!;
        standardService = App.GetService<StandardService>()!;
        Dictionary<DeviceTypeEnum, IDevice> deviceSerialPorts = GlobalData.Instance.DeviceSerialPorts;
        dSWorkwareDevice = (DSWorkwareDevice)deviceSerialPorts[Helpers.DeviceTypeEnum.DSWork];
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

        if (!dSWorkwareDevice.IsConnection())
        {
            await ShowDeviceConnnectionError("DS工装");
            return false;
        }

        // TODO 获得配置数据
        _ = await pressureDevice.SetCurrentStatus(150, 0.01f, 60);
        pressureDevice.SetCurrentPressureLook();
        await Task.Delay(30000);
        var check = pressureDevice.CheckAround(150, 0.01f);
        return check;
    }

    public override async Task ExecutionProcess()
    {
        if (!await ExecutionDetection())
        {
            GlobalData.Instance.IsOpenCheck = false;
            return;
        }
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

            return;
        }

        GlobalData.Instance.ProcessBar = 8;

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
            var temWeight=weight / 4;
            pressureDevice.SetCurrentPressureLook();

            if (await temperatureDevice.SetCurrentStatus(temperature.Value, temperature.ThresholdValue))
            {
            }

            GlobalData.Instance.ProcessBar += temWeight;

            if (_cancellation.IsCancellationRequested)
            {
                GlobalData.Instance.ProcessBar = 0;
                return;
            }

            if (await dSWorkwareDevice.SetCurrentStatus(temperature.Value, temperature.ThresholdValue))
            {
            }
            GlobalData.Instance.ProcessBar += temWeight;

            var pressureWright = (temWeight * 2) / 100;
            foreach (StandardData pressure in pressureList)
            {
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
                for (var n = 0; n < 10; n++)
                {
                    // 获得设备数据
                    for (var i = 0; i < homePageItem.Count; i++)
                    {
                        var data = (DSWorkwareGridModel)homePageItem[i];
                        DSWorkwareItem dSWorkwareItem = dSWorkwareItems[i];
                        if (n <= 0)
                        {
                            // 初始化数据
                            dSWorkwareItems[i] = new DSWorkwareItem
                            {
                                Equipment = data.Equipment,
                                StandardPressure = pressure.Value,
                                StandardTemperature = temperature.Value,
                            };
                        }

                        var dataPressure = data.Pressure ?? 0;
                        var dataTemperature = data.Temperature ?? 0;
                        dSWorkwareItem.DSWorkwareAreas.Add(new DSWorkwareArea
                        {
                            Pressure = dataPressure, Temperature = dataTemperature
                        });

                        // 检测数据是否合格
                        if (!(dataPressure.CheckAround(pressure.Value, pressure.ThresholdValue) &&
                              dataTemperature.CheckAround(temperature.Value, temperature.ThresholdValue)))
                        {
                            dSWorkwareItem.IsCheck = false;
                        }
                    }

                    await Task.Delay(1500);
                }

                dSWorkware.DSWorkwareItems.AddRange(dSWorkwareItems);
                dSWorkwareService.UpdateDSWorkware(dSWorkware);
                GlobalData.Instance.ProcessBar += pressureWright;
            }
        }

        // 检测数据校验
        dSWorkware.IsCheck = true;
        dSWorkwareService.UpdateDSWorkware(dSWorkware);
        var dictionary = dSWorkware.DSWorkwareItems.GroupBy(o => o.Equipment)
            .ToDictionary(o => o.Key, o => o.Any(x => !x.IsCheck));

        // 更新页面
        var homePageItemData = HomePageItemData.ToList();

        for (var i = 0; i < homePageItemData.Count; i++)
        {
            var v = (DSWorkwareGridModel)homePageItemData[i];

            // 检测是否合格
            v.IsCheck = !dictionary[v.Equipment];
        }
        GlobalData.Instance.ProcessBar = 100;
    }
}