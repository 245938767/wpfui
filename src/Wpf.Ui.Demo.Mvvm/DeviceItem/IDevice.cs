using System.IO.Ports;
using Wpf.Ui.Demo.Mvvm.Helpers;
using Wpf.Ui.Demo.Mvvm.Models;

namespace Wpf.Ui.Demo.Mvvm.DeviceItem;

public abstract class IDevice
{
    /// <summary>
    /// 设备端口
    /// </summary>
    protected readonly SerialPort _serialPort = new();

    /// <summary>
    /// 端口发送请求锁
    /// </summary>
    protected readonly object _serialPortLock = new();

    /// <summary>
    /// 设备信息Model
    /// </summary>
    protected readonly DeviceCard _deviceCard;

    protected abstract void ReceiveData(byte[]? receiveData);

    /// <summary>
    /// Initializes a new instance of the <see cref="IDevice"/> class.
    /// 统一初始化
    /// </summary>
    /// <param name="deviceCard">设备对应的Model数据</param>
    protected IDevice(DeviceCard deviceCard)
    {
        _deviceCard = deviceCard;
        _serialPort.UpdateSerialPortModel(deviceCard.DeviceCardDetail.SerialPortModel);
        _serialPort.SetDataReceiveData(ReceiveData);
    }

    /// <summary>
    /// 开启设备
    /// </summary>
    /// <returns> 是否开启成功</returns>
    public abstract Task<bool> Open();

    public abstract Task<bool> SetCurrentStatus(float value, float around, double timeOutSecond);

    /// <summary>
    /// 关闭设备连接
    /// </summary>
    /// <returns> 是否关闭成功</returns>
    public abstract Task<bool> CloseConnect();

    /// <summary>
    /// 检测是否在范围内
    /// </summary>
    /// <param name="value">阈值</param>
    /// <param name="checkAround">鉴定范围</param>
    /// <returns>是否在鉴定范围内</returns>
    protected abstract bool CheckAround(float value, float checkAround);
}