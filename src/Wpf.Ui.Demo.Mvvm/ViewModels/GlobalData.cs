using System.IO.Ports;
using Wpf.Ui.Demo.Mvvm.DeviceItem;
using Wpf.Ui.Demo.Mvvm.Helpers;

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
}