// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Demo.Mvvm.Helpers;
using Wpf.Ui.Demo.Mvvm.Models;

namespace Wpf.Ui.Demo.Mvvm.DeviceItem;

public class PumpDevice : IDevice
{
    public PumpDevice(DeviceCard deviceCard)
        : base(deviceCard)
    {
    }

    protected override void ReceiveData(byte[]? receiveData)
    {
    }

    public override Task<bool> Open()
    {
        _serialPort.UpdateSerialPortModel(_deviceCard.SerialPortModel);

        // 设置数据解析格式
        _serialPort.OpenPort();

        _serialPort.RtsEnable = true;
        _serialPort.DtrEnable = true;
        return Task.FromResult(true);
    }

    public override Task<bool> SetCurrentStatus(float value, float around, double timeOutSecond)
    {
        throw new NotSupportedException();
    }

    public override Task<bool> CloseConnect()
    {
        _serialPort.RtsEnable = true;
        _serialPort.DtrEnable = true;

        try
        {
            _serialPort.ClosePort(_serialPortLock);
        }
        catch (InvalidOperationException invalidOperationException)
        {
            return Task.FromResult(true);
        }

        return Task.FromResult(true);
    }

    protected override bool CheckAround(float value, float checkAround)
    {
        throw new NotSupportedException();
    }
}