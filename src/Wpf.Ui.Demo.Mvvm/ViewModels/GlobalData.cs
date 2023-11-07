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
    /// 设备数据存储
    /// </summary>
    [ObservableProperty] private Dictionary<DeviceTypeEnum, IDevice> _deviceSerialPorts = new();

    /// <summary>
    /// 全局日志信息
    /// </summary>
    [ObservableProperty] private ObservableCollection<LogMessage> _logMessages = new();

    /// <summary>
    /// 设备的日志信息
    /// </summary>
    [ObservableProperty] private Dictionary<DeviceTypeEnum, ObservableCollection<LogMessage>> deviceLogMessages = new();
}