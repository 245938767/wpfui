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

    public async override Task<bool> Open()
    {
        SerialPort.UpdateSerialPortModel(DeviceCard.SerialPortModel);
        if (!SerialPort.OpenPort())
        {
            return false;
        }

        SerialPort.RtsEnable = true;
        SerialPort.DtrEnable = true;
        return true;
    }

    public override Task<bool> SetCurrentStatus(float value, float around, double timeOutSecond)
    {
        throw new NotSupportedException();
    }

    public async override Task<bool> CloseConnect()
    {
        SerialPort.RtsEnable = true;
        SerialPort.DtrEnable = true;
        try
        {
            SerialPort.ClosePort(SerialPortLock);
        }
        catch (InvalidOperationException)
        {
            return true;
        }

        return true;
    }

    public override bool CheckAround(float value, float checkAround)
    {
        throw new NotSupportedException();
    }
}