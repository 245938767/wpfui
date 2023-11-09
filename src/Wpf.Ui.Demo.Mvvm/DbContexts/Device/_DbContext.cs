using Microsoft.EntityFrameworkCore;
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

namespace Wpf.Ui.Demo.Mvvm.DbContexts;

public class DeviceModule : DbModule
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<DeviceCard> DeviceCardBuilder = modelBuilder.Entity<DeviceCard>();
        _ = DeviceCardBuilder.HasData(init());


    }
    private static List<DeviceCard> init() {
        var deviceCards = new List<DeviceCard>();
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
            Key = DeviceTypeEnum.DSWork,
            DeviceName = "DS工装",

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
}
