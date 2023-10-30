using System.Globalization;
using System.IO.Ports;
using System.Text;
using Wpf.Ui.Demo.Mvvm.Helpers;
using Wpf.Ui.Demo.Mvvm.Models;

namespace Wpf.Ui.Demo.Mvvm.DeviceItem;

/// <summary>
/// 压力源设备
/// </summary>
public class PressureDevice : IDevice
{
    private readonly SerialPort _serialPort = new();
    private readonly CancellationTokenSource _cancelTokenMsg = new();
    private readonly DeviceCard _deviceCard;


    /// <summary>
    /// 处理获得的数据
    /// </summary>
    /// <param name="receiveData">端口返回的数据</param>
    private void ReceiveData(byte[] receiveData)
    {
        var pressureEncoding = Encoding.ASCII.GetString(receiveData);
        var pressure = Single.Parse(pressureEncoding.Split(' ')[1]);
        _deviceCard.DeviceCardDetail.CurrentPressure = pressure.ToString(CultureInfo.InvariantCulture);
    }

    public PressureDevice(DeviceCard deviceCard)
    {
        _deviceCard = deviceCard;
        _serialPort.UpdateSerialPortModel(deviceCard.DeviceCardDetail.SerialPortModel);
        _serialPort.SetDataReceiveData(ReceiveData);
    }

    public async Task<bool> Open()
    {
        _serialPort.UpdateSerialPortModel(_deviceCard.DeviceCardDetail.SerialPortModel);

        // 设置数据解析格式
        if (!_serialPort.OpenPort())
        {
            return false;
        }

        // 启动时初始化
        await SetCurrentPressureLook();
        _serialPort.SendStringMsg("SENS1:PRES:RANG \"11.00bara\"\n"); // 设置输出格式

        // 开启获得数据线程
        await Task.Factory.StartNew(
            async () =>
            {
                while (!_cancelTokenMsg.IsCancellationRequested)
                {
                    _serialPort.SendStringMsg("SENS1:PRES?\n");
                    await Task.Delay(1000);
                }
            },
            _cancelTokenMsg.Token);
        return true;
    }

    /// <summary>
    /// 关闭连接
    /// </summary>
    /// <returns>关闭连接是否成功</returns>
    public async Task<bool> CloseConnect()
    {
        await CloseStatus();
        await Task.Delay(1000);
        _serialPort.Close();
        return !_serialPort.IsOpen;
    }

    /// <summary>
    /// 重新设置设备的参数为初始状态
    /// </summary>
    private async Task CloseStatus()
    {
        _serialPort.SendStringMsg("OUTP:STAT ON\n");
        _serialPort.SendStringMsg("CAL:PRES:ZERO:VALV 0\n");
        await Task.Delay(500);
        _serialPort.SendStringMsg("CAL:PRES:ZERO:VALV 1\n");
        await SetCurrentPressureLook();
    }

    /// <summary>
    /// 设置当前压力源进入观测模式
    /// </summary>
    private async Task SetCurrentPressureLook()
    {
        await Task.Delay(500);
        _serialPort.SendStringMsg("OUTP:STAT OFF\n");
    }
}