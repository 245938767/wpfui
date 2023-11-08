// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Demo.Mvvm.Helpers;
using Wpf.Ui.Demo.Mvvm.Models;

namespace Wpf.Ui.Demo.Mvvm.DeviceItem;
public class DSWorkwareDevice : IDevice
{
    private CancellationTokenSource? _cancelTokenMsg;

    public DSWorkwareDevice(DeviceCard deviceCard)
        : base(deviceCard)
    {
    }

    public async override Task<bool> CloseConnect()
    {
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

    public async override Task<bool> Open()
    {
        SerialPort.UpdateSerialPortModel(DeviceCard.SerialPortModel);
        if (!SerialPort.OpenPort())
        {
            return false;
        }

        _cancelTokenMsg = new CancellationTokenSource();

        try
        {
            // 工装特性:要先发送两次后才会返回数据
            SerialPort.SendHexCRC("11 03 00 90 00 70",SerialPortLock);
            SerialPort.SendHexCRC("11 03 00 90 00 70", SerialPortLock);

            _ = await Task.Factory.StartNew(
                async () =>
            {
                while (!_cancelTokenMsg.IsCancellationRequested)
                {
                    // 如果发送失败直接退出循环
                    try
                    {
                        SerialPort.SendHexCRC("11 03 00 90 00 70", SerialPortLock);

                        // 每1秒获得数据
                        await Task.Delay(1000);
                    }
                    catch
                    {
                    }
                }
            },
                _cancelTokenMsg.Token);
            return true;

        }
        catch (Exception)
        {
            return false;
        }
    }

    public async override Task<bool> SetCurrentStatus(float value, float around, double timeOutSecond)
    {
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(timeOutSecond));
        try
        {
            return await await Task.Factory.StartNew(
                async () =>
            {
                while (!CheckAround(value, around) && !cancellationTokenSource.IsCancellationRequested)
                {
                    await Task.Delay(5000);
                }
                return true;
            },
                cancellationTokenSource.Token);
        }
        catch (TaskCanceledException)
        {
            return false;
        }
    }

    protected override bool CheckAround(float value, float checkAround)
    {
        var check = value - DeviceCard.CurrentTemperature;
        if (check > -checkAround && check < checkAround)
        {
            return true;
        }

        return false;
    }

    protected override void ReceiveData(byte[] receiveData)
    {
        if (receiveData.Length < 56)
        {
            return;
        }

        // 装换成float数据
        List<float> list = CRCModelHelper.TranlationByteForFloat(receiveData);
        var data = new List<float>();
        if (list != null && list.Count > 0)
        {
            // 数据排序（获取到的数据排序为17-24,1-8,9-16）设置为（1-24）
            data = list.Skip(32).Take(16).Concat(list.Take(16)).Concat(list.Skip(16).Take(16)).ToList();
            DeviceCard.CurrentTemperature = list.Skip(48).Where(x => x > 0).Average();
        }
    }
}
