// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Demo.Mvvm.Helpers;
using Wpf.Ui.Demo.Mvvm.Models;

namespace Wpf.Ui.Demo.Mvvm.DbContexts;

public partial class EntityDbContext : DbContext
{

    public EntityDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {

        Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<SerialPortModel> SerialPortBuilder = builder.Entity<SerialPortModel>();
        _ = SerialPortBuilder.HasData(initSerialPort());
        SerialPortBuilder.Ignore(o => o.DeviceStatus);

        Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<DeviceCard> DeviceCardBuilder = builder.Entity<DeviceCard>();
        DeviceCardBuilder.Ignore(o => o.CurrentPressure);
        DeviceCardBuilder.Ignore(o => o.CurrentTemperature);
        DeviceCardBuilder.Ignore(o => o.SettingPressure);
        DeviceCardBuilder.Ignore(o => o.SettingTemperature);
        DeviceCardBuilder.HasOne(o => o.SerialPortModel).WithOne(o => o.DeviceCards).HasForeignKey<DeviceCard>(o => o.ForeignKey);
        _ = DeviceCardBuilder.HasData(init());

        Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Standard> StandardBuilder = builder.Entity<Standard>();
        StandardBuilder.HasMany(o => o.StandarDatas).WithOne(o => o.Standard).HasForeignKey(k => k.StandardId).IsRequired();
        StandardBuilder.HasKey(o => o.Id);
        StandardBuilder.HasData(StandardInitial());

        Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<StandardData> StandardDataBuilder = builder.Entity<StandardData>();
        StandardDataBuilder.HasData(StandardDataInit());


        Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<DSWorkware> DSWorkwareBuilder = builder.Entity<DSWorkware>();
        DSWorkwareBuilder.HasMany(o => o.DSWorkwareItems).WithOne(o => o.Dsworkware).HasForeignKey(k => k.WorkwareId).IsRequired();

        Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<DSWorkwareItem> DSWorkwareItemBuilder = builder.Entity<DSWorkwareItem>();
        DSWorkwareItemBuilder.HasMany(o => o.DSWorkwareAreas).WithOne(o => o.DSWorkwareItem).HasForeignKey(o => o.DSWorkwareItemId).IsRequired();


        ConfigureConventions(builder);
    }

    public DbSet<DeviceCard> DeviceCards { get; set; }
    public DbSet<SerialPortModel> SerialPortModels { get; set; }
    public DbSet<Standard> Standards { get; set; }
    public DbSet<StandardData> StandardDatas { get; set; }
    public DbSet<DSWorkware> Dsworkwares { get; set; }
    public DbSet<DSWorkwareItem> DSWorkwareItems { get; set; }
    public DbSet<DSWorkwareArea> DsworkwareAreas { get; set; }


    /// <summary>
    /// 初始化测试数据
    /// </summary>
    /// <returns></returns>
    private static List<Standard> StandardInitial() => new List<Standard> {
        new Standard
        {
            Id=1,
            ProcessFlow=ProcessFlowEnum.DSTest,
            Name=ProcessFlowEnum.DSTest.ToDescription()
        }
        };
    private static List<StandardData> StandardDataInit()
    {
        return new List<StandardData> { 
        new StandardData {Id=1, StandardId=1,StandardType=StandardEnum.Pressure,Value=80,ThresholdValue=0.01f },
        new StandardData {Id=2, StandardId=1,StandardType=StandardEnum.Pressure,Value=90,ThresholdValue=0.01f },
        new StandardData {Id=3, StandardId=1,StandardType=StandardEnum.Pressure,Value=100,ThresholdValue=0.01f },
        new StandardData {Id=4, StandardId=1,StandardType=StandardEnum.Pressure,Value=110,ThresholdValue=0.01f },
        new StandardData {Id=5, StandardId=1,StandardType=StandardEnum.Pressure,Value=120,ThresholdValue=0.01f },
        new StandardData {Id=6, StandardId=1,StandardType=StandardEnum.Pressure,Value=130,ThresholdValue=0.01f },
        new StandardData {Id=7, StandardId=1,StandardType=StandardEnum.Temperature,Value=60,ThresholdValue=0.5f },
        new StandardData {Id=8, StandardId=1,StandardType=StandardEnum.Temperature,Value=35,ThresholdValue=0.5f },
        new StandardData {Id=9, StandardId=1,StandardType=StandardEnum.Temperature,Value=20,ThresholdValue=0.5f },
        new StandardData {Id=10, StandardId=1,StandardType=StandardEnum.Temperature,Value=0,ThresholdValue=0.5f },
        new StandardData {Id=11, StandardId=1,StandardType=StandardEnum.Temperature,Value=-10,ThresholdValue=0.5f },
        new StandardData { Id=12,StandardId=1,StandardType=StandardEnum.Temperature,Value=-20,ThresholdValue=0.5f },};
    }
    /// <summary>
    /// 设备卡片实例数据初始化
    /// </summary>
    /// <returns></returns>
    private static List<DeviceCard> init()
    {
        var deviceCards = new List<DeviceCard>();
        var pop = new DeviceCard
        {
            Id = 1,
            Key = DeviceTypeEnum.Pump,
            DeviceName = "真空泵",
            ForeignKey = 1,

            UnitP = "Kpa",
            UnitT = "℃",
            ImageUrl = "pack://application:,,,/Assets/WinUiGallery/Pump.png",
            Version = 1.0f,
        };
        var pressure = new DeviceCard
        {
            Id = 2,
            Key = DeviceTypeEnum.Pressure,
            DeviceName = "压力源",
            ForeignKey = 2,

            UnitP = "Kpa",
            UnitT = "℃",

            ImageUrl = "pack://application:,,,/Assets/WinUiGallery/Pressure.png",
            Version = 1.0f,
        };
        var temperature = new DeviceCard
        {
            Id = 3,
            Key = DeviceTypeEnum.Temperature,
            DeviceName = "温箱",
            ForeignKey = 3,

            UnitP = "Kpa",
            UnitT = "℃",
            ImageUrl = "pack://application:,,,/Assets/WinUiGallery/Temperature.png",
            Version = 1.0f,

        };
        var work = new DeviceCard
        {
            Id = 4,
            Key = DeviceTypeEnum.DSWork,
            DeviceName = "DS工装",
            ForeignKey = 4,

            UnitP = "Kpa",
            UnitT = "℃",
            ImageUrl = "pack://application:,,,/Assets/WinUiGallery/Working.png",
            Version = 1.0f,
        };
        var pressureSensor = new DeviceCard
        {
            Id = 5,
            Key = DeviceTypeEnum.PressureSensor,
            DeviceName = "压力传感器",
            ForeignKey = 5,
            UnitP = "Kpa",
            UnitT = "℃",
            ImageUrl = "pack://application:,,,/Assets/WinUiGallery/Working.png",
            Version = 1.0f,
        };
        deviceCards.Add(pop);
        deviceCards.Add(pressure);
        deviceCards.Add(temperature);
        deviceCards.Add(work);
        deviceCards.Add(pressureSensor);
        return deviceCards;
    }
    private static List<SerialPortModel> initSerialPort()
    {
        var deviceCards = new List<SerialPortModel>();
        var pop = new SerialPortModel()
        {
            Id = 1,
            PortName = null,
            StopBit = StopBits.One,
            BaudRate = 9600,
            DataBit = 8,
            NetworkAddress = "01",
            DeviceStatus = false,

        };
        var pressure = new SerialPortModel()
        {
            Id = 2,
            PortName = null,
            StopBit = StopBits.One,
            BaudRate = 9600,
            DataBit = 8,
            DeviceStatus = false,
            NetworkAddress = "01"

        };
        var temperature = new SerialPortModel()
        {
            Id = 3,
            PortName = null,
            StopBit = StopBits.One,
            BaudRate = 9600,
            DataBit = 8,
            DeviceStatus = false,
            NetworkAddress = "01"


        };
        var work = new SerialPortModel()
        {
            Id = 4,
            PortName = null,
            StopBit = StopBits.One,
            BaudRate = 115200,
            DataBit = 8,
            DeviceStatus = false,
            NetworkAddress = "11"

        };
        var pressuresen = new SerialPortModel()
        {
            Id = 5,
            PortName = null,
            StopBit = StopBits.One,
            BaudRate = 115200,
            DataBit = 8,
            DeviceStatus = false,
            NetworkAddress = "11"

        };
        deviceCards.Add(pop);
        deviceCards.Add(pressure);
        deviceCards.Add(temperature);
        deviceCards.Add(work);
        deviceCards.Add(pressuresen);
        return deviceCards;
    }

    private void ConfigureConventions(ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (entityType.BaseType != null)
            {
                continue;
            }

            var tableName = entityType.GetTableName();
            entityType.SetTableName("t_" + tableName);

            // 用DescriptionAttribute生成表说明
            var attr = entityType.ClrType.GetCustomAttribute<DescriptionAttribute>();
            if (attr != null)
            {
                entityType.SetComment(attr.Description);
            }

            foreach (var propertyType in entityType.GetProperties())
            {
                var propertyInfo = propertyType.PropertyInfo;
                attr = propertyInfo?.GetCustomAttribute<DescriptionAttribute>();
                if (attr == null || propertyInfo == null) continue;
                var desc = attr.Description;
                if (propertyInfo.PropertyType.IsEnum)
                {
                    var enums = Enum.GetNames(propertyInfo.PropertyType)
                        .ToDictionary(o => Enum.Parse(propertyInfo.PropertyType, o),
                            o => propertyInfo.PropertyType?.GetField(o)
                                .GetCustomAttribute<DescriptionAttribute>()?.Description)
                        .Where(o => !string.IsNullOrEmpty(o.Value) && o.Value != "-")
                        .ToArray();

                    desc += $"({string.Join(",", enums.Select(o => $"{(int)o.Key}={o.Value}"))})";
                }

                propertyType.SetComment(desc);
            }
        }
    }

}