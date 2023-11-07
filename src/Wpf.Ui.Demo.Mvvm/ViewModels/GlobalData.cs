using System.Collections.ObjectModel;
using System.IO.Ports;
using Wpf.Ui.Demo.Mvvm.DeviceItem;
using Wpf.Ui.Demo.Mvvm.Helpers;
using Wpf.Ui.Demo.Mvvm.Models;

namespace Wpf.Ui.Demo.Mvvm.ViewModels;

public partial class GlobalData : ObservableObject
{
    private GlobalData()
    {
    }

    public static GlobalData Instance { get; private set; } = new();

    /// <summary>
    /// 设备数据存储（只有点击连接设备时才会加入到字典中完成初始化）
    /// </summary>
    [ObservableProperty] private Dictionary<DeviceTypeEnum, IDevice> deviceSerialPorts = new();

    /// <summary>
    /// 全局日志信息
    /// </summary>
    [ObservableProperty] private ObservableCollection<LogMessage> _logMessages = new();

    /// <summary>
    /// 设备的日志信息
    /// </summary>
    [ObservableProperty] private Dictionary<DeviceTypeEnum, ObservableCollection<LogMessage>> deviceLogMessages = new();
}