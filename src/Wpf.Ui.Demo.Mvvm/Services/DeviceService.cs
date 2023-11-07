using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Demo.Mvvm.Helpers;
using Wpf.Ui.Demo.Mvvm.Models;
using Wpf.Ui.Demo.Mvvm.Views.Pages;

namespace Wpf.Ui.Demo.Mvvm.Services;

public class DeviceService
{
    /// <summary>
    /// 获得本地数据
    /// </summary>
    /// <returns></returns>
    public ObservableCollection<DeviceCard> GetLocaltionData() {
        //如果是第一次获取返回基本数据并保存到本地
        var deviceCards = new ObservableCollection<DeviceCard>();
        var pop = new DeviceCard
        {
            Key = DeviceTypeEnum.Pump,
            DeviceName = "真空泵",

            SerialPortModel = new SerialPortModel()
            {
                PortName = null,
                StopBit = StopBits.One,
                BaudRate = 9600,
                DataBit = 8,
                NetworkAddress = "01",
                DeviceStatus = false,
            },
            UnitP = "Kpa",
            UnitT = "℃",
            ImageUrl = "pack://application:,,,/Assets/WinUiGallery/Pump.png",
            Version = 1.0f,
        };
        var pressure = new DeviceCard
        {
            Key = DeviceTypeEnum.Pressure,
            DeviceName = "压力源",

            SerialPortModel = new SerialPortModel()
            {
                PortName = null,
                StopBit = StopBits.One,
                BaudRate = 9600,
                DataBit = 8,
                DeviceStatus = false,
                NetworkAddress = "01"
            },
            CurrentPressure = 100f,
            CurrentTemperature = 20f,
            UnitP = "Kpa",
            UnitT = "℃",

            ImageUrl = "pack://application:,,,/Assets/WinUiGallery/Pressure.png",
            Version = 1.0f,
        };
        var temperature = new DeviceCard
        {
            Key = DeviceTypeEnum.Temperature,
            DeviceName = "温箱",

            SerialPortModel = new SerialPortModel()
            {
                PortName = null,
                StopBit = StopBits.One,
                BaudRate = 9600,
                DataBit = 8,
                DeviceStatus = false,
                NetworkAddress = "01"
            },
            UnitP = "Kpa",
            UnitT = "℃",
            ImageUrl = "pack://application:,,,/Assets/WinUiGallery/Temperature.png",
            Version = 1.0f,

        };
        var work = new DeviceCard
        {
            Key = DeviceTypeEnum.Work,
            DeviceName = "工装",

            SerialPortModel = new SerialPortModel()
            {
                PortName = null,
                StopBit = StopBits.One,
                BaudRate = 9600,
                DataBit = 8,
                DeviceStatus = false,
                NetworkAddress = "01"
            },
            UnitP = "Kpa",
            UnitT = "℃",
            ImageUrl = "pack://application:,,,/Assets/WinUiGallery/Working.png",
            Version = 1.0f,
        };
        deviceCards.Add(pop);
        deviceCards.Add(pressure);
        deviceCards.Add(temperature);
        deviceCards.Add(work);
        return deviceCards;
    }
    /// <summary>
    /// 更新本地数据
    /// </summary>
    /// <param name="deviceCard">修改后的数据</param>
    public void UpdateLocaltionData(DeviceCard deviceCard) {
        //本地数据默认设备状态为关闭，并且数值为空
        return;
    }
}
