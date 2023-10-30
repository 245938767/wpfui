using System.IO.Ports;
using Wpf.Ui.Demo.Mvvm.Models;

namespace Wpf.Ui.Demo.Mvvm.DeviceItem;

public interface IDevice
{
    /// <summary>
    /// 开启设备（初始化）
    /// </summary>
    /// <param name="serialPort"></param>
    /// <returns></returns>
    Task<bool> Open();

    /// <summary>
    /// 关闭设备连接
    /// </summary>
    /// <returns></returns>
    Task<bool> CloseConnect();
}