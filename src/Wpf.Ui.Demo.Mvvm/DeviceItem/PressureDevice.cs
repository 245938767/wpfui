// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Text;
using CommunityToolkit.Mvvm.Messaging;
using Wpf.Ui.Demo.Mvvm.Helpers.Extension;
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
    private CancellationTokenSource? _cancelTokenMsg;

    public PressureDevice(DeviceCard deviceCard)
        : base(deviceCard)
    {
        SerialPort.SetDataReceiveData(ReceiveData);

    }

    /// <summary>
    /// 处理获得的数据
    /// </summary>
    /// <param name="receiveData">端口返回的数据</param>
    protected override void ReceiveData(byte[] receiveData)
    {
        try
        {
            var pressureEncoding = Encoding.ASCII.GetString(receiveData);
            var pressure = Single.Parse(pressureEncoding.Split(' ')[1]);
            DeviceCard.CurrentPressure = pressure;
        }
        catch (Exception)
        {
        }
    }

    public override async Task<bool> Open()
    {
        SerialPort.UpdateSerialPortModel(DeviceCard.SerialPortModel);
        if (!SerialPort.OpenPort())
        {
            return false;
        }

        // 启动时初始化
        SetCurrentPressureLook();
        SerialPort.SendStringMsg("SENS1:PRES:RANG \"11.00bara\"\n", SerialPortLock); // 设置输出格式
        _cancelTokenMsg = new CancellationTokenSource();

        // 开启获得数据线程
        _ = await Task.Factory.StartNew(
            async () =>
            {
                while (!_cancelTokenMsg.IsCancellationRequested)
                {
                    SerialPort.SendStringMsg("SENS1:PRES?\n", SerialPortLock);
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
        await Task.Delay(100);
        try
        {
            _cancelTokenMsg!.Cancel();
            SerialPort.ClosePort(SerialPortLock);
        }
        catch (InvalidOperationException)
        {
            return true;
        }

        return !SerialPort.IsOpen;
    }

    private async Task CurrentPressure(float pressure)
    {
        SerialPort.SendStringMsg("OUTP:STAT ON\n", SerialPortLock);
        SerialPort.SendStringMsg($"SOUR:PRES {pressure}\n", SerialPortLock);
        await Task.Delay(1000);
    }
    
    /// <summary>
    /// 进入观测模式
    /// </summary>
    public void SetCurrentPressureLook() {
        SerialPort.SendStringMsg("OUTP:STAT OFF\n", SerialPortLock);
    }
    public override bool CheckAround(float value, float checkAround)
    {
        var check = DeviceCard.CurrentPressure - value;
        return check > -checkAround && check < checkAround;
    }

    /// <summary>
    /// 重新设置设备的参数为初始状态
    /// </summary>
    private async Task CloseStatus()
    {
        SerialPort.SendStringMsg("OUTP:STAT ON\n", SerialPortLock);
        SerialPort.SendStringMsg("CAL:PRES:ZERO:VALV 0\n", SerialPortLock);
        await Task.Delay(500);
        SerialPort.SendStringMsg("CAL:PRES:ZERO:VALV 1\n", SerialPortLock);
        SetCurrentPressureLook();
    }

}