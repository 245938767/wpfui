using CommunityToolkit.Mvvm.Messaging;
using Wpf.Ui.Demo.Mvvm.Helpers;
using Wpf.Ui.Demo.Mvvm.Models;

namespace Wpf.Ui.Demo.Mvvm.DeviceItem;

public class TemperatureDevice : IDevice
{
    /// <summary>
    /// 线程监听对象
    /// </summary>
    private CancellationTokenSource _cancelTokenMsg;

    public TemperatureDevice(DeviceCard deviceCard) : base(deviceCard)
    {
    }

    protected override void ReceiveData(byte[]? receiveData)
    {
        var list = BitConverter.ToString(CRCModelHelper.CheckCrc(receiveData) ?? Array.Empty<byte>()).Split('-')
            .ToList();
        var temperature = Convert.ToInt32(list[0] + list[1], 16) / 10f;
        _deviceCard.CurrentTemperature = temperature;

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

        _cancelTokenMsg = new CancellationTokenSource();
        // 开启获得数据线程
        await Task.Factory.StartNew(
            async () =>
            {
                while (!_cancelTokenMsg.IsCancellationRequested)
                {
                    _serialPort.SendHexCRC("01 03 1F 37 00 01", _serialPortLock);
                    await Task.Delay(1000);
                }
            },
            _cancelTokenMsg.Token);
        return true;
    }

    public override async Task<bool> SetCurrentStatus(float value, float around, double timeOutSecond = 8200)
    {
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
        await CloseTemperature();
        try
        {
            _serialPort.ClosePort(_serialPortLock);
            return true;
        }
        catch (InvalidOperationException e)
        {
            return true;
        }
    }


    protected override bool CheckAround(float value, float checkAround)
    {
        var check = _deviceCard.CurrentTemperature - value;
        return check > -checkAround && check < checkAround;
    }

    /// <summary>
    /// 关闭当前温箱运行
    /// </summary>
    private async Task CloseTemperature()
    {
        _serialPort.SendHexCRC("01 05 1F 41 FF 00", _serialPortLock);
        await Task.Delay(500);
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
        _serialPort.SendHexCRC($"01 06 1F A4 {convertData}", _serialPortLock);
        await Task.Delay(1000);
    }
}