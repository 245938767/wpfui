using System.IO.Ports;
using Wpf.Ui.Demo.Mvvm.DeviceItem;
using Wpf.Ui.Demo.Mvvm.Helpers;

namespace Wpf.Ui.Demo.Mvvm.ViewModels;

public partial class GlobalData : ObservableObject
{
    private GlobalData()
    {
    }

    public static GlobalData Instance { private set; get; } = new GlobalData();

    [ObservableProperty] private Dictionary<DeviceTypeEnum, IDevice> deviceSerialPorts = new();
}