using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Demo.Mvvm.DbContexts;
using Wpf.Ui.Demo.Mvvm.Helpers;
using Wpf.Ui.Demo.Mvvm.Models;
using Wpf.Ui.Demo.Mvvm.Views.Pages;

namespace Wpf.Ui.Demo.Mvvm.Services;

public class DeviceService
{
    private readonly EntityDbContext _dbContext;
    private readonly string deviceCardKey = "deviceCards";

    public DeviceService(EntityDbContext dbContext) { 
        _dbContext = dbContext; 
    }

    /// <summary>
    /// 获得本地数据
    /// </summary>
    /// <returns></returns>
    public List<DeviceCard> GetLocaltionData() {
        var deviceCards = _dbContext.DeviceCards.ToList<DeviceCard>();
        return deviceCards;
    }

    /// <summary>
    /// 更新本地数据
    /// </summary>
    /// <param name="deviceCard">修改后的数据</param>
    public void UpdateLocaltionData(DeviceCard deviceCard) {
        // 本地数据默认设备状态为关闭，并且数值为空
        var deviceCardData = _dbContext.DeviceCards.FirstOrDefault(o=>o.Key==deviceCard.Key);
        _dbContext.Update(deviceCardData);
        _dbContext.SaveChanges();
        return;
    }
}
