// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Boiido.IO.SerialPort;
using Wpf.Ui.Demo.Mvvm.Models;


namespace Wpf.Ui.Demo.Mvvm.ViewModels;

public partial class DevicePortConnectViewModel : ObservableObject
{
    [ObservableProperty] private SerialPortModel _serialPortModel;

    public DevicePortConnectViewModel(SerialPortModel? serialPortModel = null)
    {
        _serialPortModel = serialPortModel ??
                           new SerialPortModel { BaudRate = 9600, DataBit = 8, NetworkAddress = "01", StopBit = 1 };
        PortList = SerialPort.portArray;
        StopBitList = new List<int>() { 1, 2, 3 };
        BaudRateList = new List<int>()
        {
            110,
            300,
            600,
            1200,
            2400,
            4800,
            9600,
            14400,
            19200,
            38400,
            56000,
            57600,
            115200,
        };
        DataBitList = new List<int>() { 6, 7, 8 };
        NetworkAddressList = new List<string>() { "01", "02", "03" };

        Init();
    }

    private void Init()
    {
        if (PortList is { Length: > 0 })
        {
            Port = Array.IndexOf(PortList, SerialPortModel.PortName);
        }

        StopBit = Array.IndexOf(StopBitList.ToArray(), SerialPortModel.StopBit);
        BaudRate = Array.IndexOf(BaudRateList.ToArray(), SerialPortModel.BaudRate);
        DataBit = Array.IndexOf(DataBitList.ToArray(), SerialPortModel.DataBit);
        NetworkAddress = Array.IndexOf(NetworkAddressList.ToArray(), SerialPortModel.NetworkAddress);
    }

    /// <summary>
    /// 端口数据
    /// </summary>
    [ObservableProperty] private Array? _portList;

    [ObservableProperty] private int _port;

    /// <summary>
    /// 停止位
    /// </summary>
    [ObservableProperty] private List<int> _stopBitList;

    [ObservableProperty] private int _stopBit;

    /// <summary>
    /// 波特率
    /// </summary>
    [ObservableProperty] private List<int> _baudRateList;

    [ObservableProperty] private int _baudRate;

    /// <summary>
    /// 数据位
    /// </summary>
    [ObservableProperty] private List<int> _dataBitList;

    [ObservableProperty] private int _dataBit;

    /// <summary>
    /// 网络地址
    /// </summary>
    [ObservableProperty] private List<string> _networkAddressList;

    [ObservableProperty] private int _networkAddress;
}