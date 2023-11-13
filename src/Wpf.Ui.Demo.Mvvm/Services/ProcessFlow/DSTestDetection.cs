using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Demo.Mvvm.DeviceItem;
using Wpf.Ui.Demo.Mvvm.Helpers;
using Wpf.Ui.Demo.Mvvm.ViewModels;

namespace Wpf.Ui.Demo.Mvvm.Services.ProcessFlow;

class DSTestDetection : IProcessFlow
{
    private CancellationTokenSource? _cancellation;
    private readonly PumpDevice pumpDevice;
    private readonly PressureDevice pressureDevice;
    private readonly TemperatureDevice temperatureDevice;
    private readonly DSWorkwareDevice dSWorkwareDevice;


    public DSTestDetection(ProcessFlowEnum processFlow, ObservableCollection<object> homePageItemData):base(processFlow,homePageItemData)
    {
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

        // 检测当前是否有缓存数据

        // 数据加载缓存的，DoMain ID,加载日志记录

        // 步骤使用X,Y进行记录，并且记录当前的测试数据，再次加载时，根据X，Y的标记点去除之前的数据

        // 加载当前此测用例的缓存步骤数据
        GlobalData.Instance.IsOpenCheck = true;

        _cancellation = new CancellationTokenSource();
        // 开始处理流程数据
        
        // 获得测试标准数据和阈值
        
    }
}