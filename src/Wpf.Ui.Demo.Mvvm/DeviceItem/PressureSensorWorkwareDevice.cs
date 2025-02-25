// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Wpf.Ui.Demo.Mvvm.Helpers.Extension;
using Wpf.Ui.Demo.Mvvm.Models;
using Wpf.Ui.Demo.Mvvm.ViewModels;

namespace Wpf.Ui.Demo.Mvvm.DeviceItem;

public class PressureSensorWorkwareDevice : IDevice
{
    private readonly ObservableCollection<object> _viewList;
    private CancellationTokenSource? _cancelTokenMsg;

    public PressureSensorWorkwareDevice(DeviceCard deviceCard, ObservableCollection<object> viewList)
        : base(deviceCard)
    {
        _viewList = viewList;
        SerialPort.SetDataReceiveData(ReceiveData);
    }

    public async override Task<bool> CloseConnect()
    {
        try
        {
            _cancelTokenMsg!.Cancel();
            await Task.Delay(500);
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
            SerialPort.SendHexCRC($"{DeviceCard.SerialPortModel.NetworkAddress} 03 00 90 00 70", SerialPortLock);
            SerialPort.SendHexCRC($"{DeviceCard.SerialPortModel.NetworkAddress} 03 00 90 00 70", SerialPortLock);

            _ = await Task.Factory.StartNew(
                async () =>
                {
                    while (!_cancelTokenMsg.IsCancellationRequested)
                    {
                        // 如果发送失败直接退出循环
                        try
                        {
                            SerialPort.SendHexCRC(
                                $"{DeviceCard.SerialPortModel.NetworkAddress} 03 00 90 00 70",
                                SerialPortLock);

                            // 每1秒获得数据
                            await Task.Delay(1000);
                        }
                        catch
                        {
                            // ignored
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

    public async override Task<bool> SetCurrentStatus(float value, float around, double timeOutSecond = 8200)
    {
        /**    using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(timeOutSecond));
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
        */
        return true;
    }

    public override bool CheckAround(float value, float checkAround)
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
        if (list.Count <= 0)
        {
            return;
        }

        // 数据排序（获取到的数据排序为17-24,1-8,9-16）设置为（1-24）
        //var data = list.Skip(32).Take(16).Concat(list.Take(16)).Concat(list.Skip(16).Take(16)).ToList();
        var data = list.Take(48).ToList();


        // 生成对象数据
        ObservableCollection<object> homePageItemData = _viewList;
        var count = 0;
        var datafloat = new List<float>();
        var dataTemperaturefloat = new List<float>();

        // 根据是否有数据进行更新和创建渲染
        if (homePageItemData.Count > 0)
        {
            for (var i = 0; i < data.Count / 2; i++)
            {
                var v = (DSWorkwareGridModel)homePageItemData[i];
                v.Pressure = Single.Parse(data[count++].ToString("#.000"));
                v.Temperature = Single.Parse(data[count++].ToString("#.00"));
                if (v.Pressure > 0)
                {
                    datafloat.Add((float)v.Pressure!);
                }
                if (v.Temperature > 0)
                {
                    dataTemperaturefloat.Add((float)v.Temperature!);
                }
            }
        }
        else
        {
            for (var i = 0; i < data.Count / 2; i++)
            {
                var dSWorkwareGridModel = new DSWorkwareGridModel();
                dSWorkwareGridModel.SerialNumber = i + 1;
                dSWorkwareGridModel.Pressure = Single.Parse(data[count++].ToString("#.000"));
                dSWorkwareGridModel.Temperature = Single.Parse(data[count++].ToString("#.00"));
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    homePageItemData.Add(dSWorkwareGridModel);
                }));
                if (dSWorkwareGridModel.Pressure > 0)
                {
                    datafloat.Add((float)dSWorkwareGridModel.Pressure!);
                }
                if (dSWorkwareGridModel.Temperature > 0)
                {
                    dataTemperaturefloat.Add((float)dSWorkwareGridModel.Temperature!);
                }
            }
        }

        if (datafloat.Count > 0)
        {
            DeviceCard.CurrentPressure = datafloat.Average();
        }
        if (dataTemperaturefloat.Count > 0)
        {
            DeviceCard.CurrentTemperature = dataTemperaturefloat.Average();

        }

    }
}