using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Windows.Threading;
using Wpf.Ui.Demo.Mvvm.DeviceItem;
using Wpf.Ui.Demo.Mvvm.Helpers;
using Wpf.Ui.Demo.Mvvm.Models;
using Wpf.Ui.Demo.Mvvm.Services.ProcessFlow;

namespace Wpf.Ui.Demo.Mvvm.ViewModels;

public partial class GlobalData : ObservableObject
{
    private readonly Dispatcher dispatcher;
    private GlobalData()
    {
        dispatcher = Application.Current.Dispatcher;
    }

    public static GlobalData Instance { get; private set; } = new();

    /// <summary>
    /// 是否开启了检测
    /// </summary>
    [ObservableProperty] private bool _isOpenCheck;

    /// <summary>
    /// 设备数据存储
    /// </summary>
    [ObservableProperty] private Dictionary<DeviceTypeEnum, IDevice> _deviceSerialPorts = new();

    [ObservableProperty] private Dictionary<ProcessFlowEnum, IProcessFlow> _processFlow = new();

    /// <summary>
    /// 全局日志信息
    /// </summary>
    [ObservableProperty] private ObservableCollection<LogMessage> _logMessages = new();

    /// <summary>
    /// 设备的日志信息
    /// </summary>
    [ObservableProperty] private Dictionary<DeviceTypeEnum, ObservableCollection<LogMessage>> deviceLogMessages = new();
}