using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Demo.Mvvm.DeviceItem;
using Wpf.Ui.Demo.Mvvm.ViewModels;

namespace Wpf.Ui.Demo.Mvvm.Services.ProcessFlow;

class DSTestDetection : IProcessFlow
{
    private CancellationTokenSource _cancellation;
    private readonly PumpDevice? pumpDevice;
    private readonly PressureDevice? pressureDevice;
    private readonly TemperatureDevice? temperatureDevice;
    private readonly DSWorkwareDevice? dSWorkwareDevice;


    public DSTestDetection() {
        var deviceSerialPorts = GlobalData.Instance.DeviceSerialPorts;
       dSWorkwareDevice= (DSWorkwareDevice?)deviceSerialPorts[Helpers.DeviceTypeEnum.DSWork];
        temperatureDevice = (TemperatureDevice?)deviceSerialPorts[Helpers.DeviceTypeEnum.Temperature];
        pressureDevice = (PressureDevice?)deviceSerialPorts[Helpers.DeviceTypeEnum.Pressure];
        pumpDevice = (PumpDevice?)deviceSerialPorts[Helpers.DeviceTypeEnum.Pump];
    }
    public override bool BreakExecution()
    {
        _cancellation?.Cancel();
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }
    
    public override Task<bool> ExecutionDetection()
    { 
        // TODO 设备检测

        // TODO 漏气检测

        throw new NotImplementedException();
    }

    public async override Task ExecutionProcess()
    {
        if (!await ExecutionDetection())
        {
            return;
        }

        _cancellation = new CancellationTokenSource();
        throw new NotImplementedException();
    }
}
