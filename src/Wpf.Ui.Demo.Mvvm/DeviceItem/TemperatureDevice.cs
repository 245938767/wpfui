using Wpf.Ui.Demo.Mvvm.Helpers.Extension;
using Wpf.Ui.Demo.Mvvm.Models;

namespace Wpf.Ui.Demo.Mvvm.DeviceItem;

public class TemperatureDevice : IDevice
{
    /// <summary>
    /// 线程监听对象
    /// </summary>
    private CancellationTokenSource? _cancelTokenMsg;

    public TemperatureDevice(DeviceCard deviceCard)
        : base(deviceCard)
    {
        SerialPort.SetDataReceiveData(ReceiveData);
    }

    protected override void ReceiveData(byte[]? receiveData)
    {
        var list = BitConverter.ToString(CRCModelHelper.CheckCrc(receiveData) ?? Array.Empty<byte>()).Split('-')
            .ToList();
        if (list.Count >= 2)
        {
            var temperature = Convert.ToInt32(list[0] + list[1], 16);
            if (list[0] == "FF")
            {
                DeviceCard.CurrentTemperature = (temperature - 65536) / 10f;

            }
            else
            {
                DeviceCard.CurrentTemperature = temperature / 10f;

            }
        }
    }

    public override async Task<bool> Open()
    {
        SerialPort.UpdateSerialPortModel(DeviceCard.SerialPortModel);

        // 设置数据解析格式
        if (!SerialPort.OpenPort())
        {
            return false;
        }

        _cancelTokenMsg = new CancellationTokenSource();
        OpenTemperature();
        // 开启获得数据线程
        _ = await Task.Factory.StartNew(
    async () =>
    {
        while (!_cancelTokenMsg.IsCancellationRequested)
        {
            SerialPort.SendHexCRC("01 03 1F 37 00 01", SerialPortLock);
            await Task.Delay(1000);
        }
    },
    _cancelTokenMsg.Token);
        return true;
    }

    public override async Task<bool> SetCurrentStatus(float value, float around, double timeOutSecond = 8200)
    {
        await CurrentTemperature(value);
        await Task.Delay(1000);
        await CurrentTemperature(value);
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

    public override async Task<bool> CloseConnect()
    {
        _cancelTokenMsg.Cancel();
        CloseTemperature();
        try
        {
            SerialPort.ClosePort(SerialPortLock);
            return true;
        }
        catch (InvalidOperationException e)
        {
            return true;
        }
    }


    public override bool CheckAround(float value, float checkAround)
    {
        var check = DeviceCard.CurrentTemperature - value;
        return check > -checkAround && check < checkAround;
    }

    /// <summary>
    /// 关闭当前温箱运行
    /// </summary>
    public void CloseTemperature()
    {
        SerialPort.SendHexCRC("01 05 1F 41 FF 00", SerialPortLock);
    }

    /// <summary>
    /// 开启
    /// </summary>
    public void OpenTemperature()
    {
        SerialPort.SendHexCRC("01 05 1F 40 FF 00", SerialPortLock);
    }

    /// <summary>
    /// 当前温度设置
    /// </summary>
    /// <param name="temperature">要设置的温度</param>
    private async Task CurrentTemperature(float temperature)
    {
        var temperatureData = (int)(Single.Parse(temperature.ToString("0.#")) * 10);

        if (temperatureData < 0)
        {
            temperatureData = 65536 + temperatureData;
        }

        var convertData = temperatureData.ToString("X4");
        SerialPort.SendHexCRC($"01 06 1F A4 {convertData}", SerialPortLock);
        await Task.Delay(500);
    }
}