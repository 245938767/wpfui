// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Text;
using CommunityToolkit.Mvvm.Messaging;
using Wpf.Ui.Demo.Mvvm.Helpers;
using Wpf.Ui.Demo.Mvvm.Models;
using Wpf.Ui.Demo.Mvvm.ViewModels;

namespace Wpf.Ui.Demo.Mvvm.DeviceItem;

/// <summary>
/// 压力源设备
/// </summary>
public class PressureDevice : IDevice
{
    /// <summary>
    /// 线程监听对象
    /// </summary>
    private CancellationTokenSource _cancelTokenMsg;

    public PressureDevice(DeviceCard deviceCard)
        : base(deviceCard)
    {
    }

    /// <summary>
    /// 处理获得的数据
    /// </summary>
    /// <param name="receiveData">端口返回的数据</param>
    protected override void ReceiveData(byte[]? receiveData)
    {
        var pressureEncoding = Encoding.ASCII.GetString(receiveData);
        var pressure = Single.Parse(pressureEncoding.Split(' ')[1]);
        _deviceCard.CurrentPressure = pressure;
        WeakReferenceMessenger.Default.Send(_deviceCard);
    }


    public override async Task<bool> Open()
    {
        _serialPort.UpdateSerialPortModel(_deviceCard.SerialPortModel);

        // 设置数据解析格式
        if (!_serialPort.OpenPort())
        {
            return false;
        }

        // 启动时初始化
        await SetCurrentPressureLook();
        _serialPort.SendStringMsg("SENS1:PRES:RANG \"11.00bara\"\n", _serialPortLock); // 设置输出格式
        _cancelTokenMsg = new CancellationTokenSource();

        // 开启获得数据线程
        await Task.Factory.StartNew(
            async () =>
            {
                while (!_cancelTokenMsg.IsCancellationRequested)
                {
                    _serialPort.SendStringMsg("SENS1:PRES?\n", _serialPortLock);
                    await Task.Delay(1000);
                }
            },
            _cancelTokenMsg.Token);
        return true;
    }

    public override async Task<bool> SetCurrentStatus(float value, float around, double timeOutSecond)
    {
        await CurrentPressure(value);
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(timeOutSecond));
        try
        {
            return await await Task.Factory.StartNew(
                async () =>
                {
                    while (!CheckAround(value, around) && !cancellationTokenSource.IsCancellationRequested)
                    {
                        await Task.Delay(5000, cancellationTokenSource.Token);
                    }

                    return true;
                },
                cancellationTokenSource.Token);
        }
        catch (TaskCanceledException e)
        {
            return false;
        }
    }

    /// <summary>
    /// 关闭连接
    /// </summary>
    /// <returns>关闭连接是否成功</returns>
    public override async Task<bool> CloseConnect()
    {
        await CloseStatus();
        await Task.Delay(1000);
        try
        {
            _cancelTokenMsg.Cancel();
            _serialPort.ClosePort(_serialPortLock);
        }
        catch (InvalidOperationException invalidOperationException)
        {
            return true;
        }

        return !_serialPort.IsOpen;
    }

    private async Task CurrentPressure(float pressure)
    {
        _serialPort.SendStringMsg("OUTP:STAT ON\n", _serialPortLock);
        _serialPort.SendStringMsg($"SOUR:PRES {pressure}\n", _serialPortLock);
        await Task.Delay(1000);
    }

    protected override bool CheckAround(float value, float checkAround)
    {
        var check = _deviceCard.CurrentPressure - value;
        return check > -checkAround && check < checkAround;
    }

    /// <summary>
    /// 重新设置设备的参数为初始状态
    /// </summary>
    private async Task CloseStatus()
    {
        _serialPort.SendStringMsg("OUTP:STAT ON\n", _serialPortLock);
        _serialPort.SendStringMsg("CAL:PRES:ZERO:VALV 0\n", _serialPortLock);
        await Task.Delay(500);
        _serialPort.SendStringMsg("CAL:PRES:ZERO:VALV 1\n", _serialPortLock);
        await SetCurrentPressureLook();
    }

    /// <summary>
    /// 设置当前压力源进入观测模式
    /// </summary>
    private async Task SetCurrentPressureLook()
    {
        await Task.Delay(500);
        _serialPort.SendStringMsg("OUTP:STAT OFF\n", _serialPortLock);
    }
}