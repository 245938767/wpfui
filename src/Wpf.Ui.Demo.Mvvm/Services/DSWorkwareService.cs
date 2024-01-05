// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Demo.Mvvm.DbContexts;
using Wpf.Ui.Demo.Mvvm.Models;

namespace Wpf.Ui.Demo.Mvvm.Services;

public class DSWorkwareService
{
    private readonly EntityDbContext _dbContext;

    public DSWorkwareService(EntityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public DSWorkware? GetNewsData(long? id) {

        DSWorkware dw = null;
        if (id!=null) {
            dw = _dbContext.Dsworkwares.Where(o => o.id == id).OrderByDescending(x => x.CreateTime).FirstOrDefault();
        }
        dw = _dbContext.Dsworkwares.OrderByDescending(x => x.CreateTime).FirstOrDefault();
        dw.DSWorkwareItems = _dbContext.DSWorkwareItems.Include(x => x.DSWorkwareAreas).Where(x => x.WorkwareId == dw.id).ToList();

        return dw;
    }
    public List<DSWorkware>? GetDsWorkwareList()
    {
        var dw = _dbContext.Dsworkwares.Include(o=>o.DSWorkwareItems).OrderByDescending(x => x.CreateTime).ToList();
        return dw;

    }

    public int SaveDSWorkware(DSWorkware dSWorkware)
    {
        _ = _dbContext.Dsworkwares.Add(dSWorkware);
        return _dbContext.SaveChanges();
    }

    public int UpdateDSWorkware(DSWorkware dSWorkware)
    {
        _ = _dbContext.Dsworkwares.Update(dSWorkware);
        return _dbContext.SaveChanges();
    }

    public int SaveDSWorkwareItem(DSWorkwareItem sWorkwareItem)
    {
        _ = _dbContext.DSWorkwareItems.Add(sWorkwareItem);
        return _dbContext.SaveChanges();
    }

    public int SaveDSWorkwareArea(DSWorkwareArea dSWorkwareArea)
    {
        _ = _dbContext.DsworkwareAreas.Add(dSWorkwareArea);
        return _dbContext.SaveChanges();
    }
    public bool DeleteWorkware(long id) {
        List<DSWorkware> dSWorkwares = _dbContext.Dsworkwares.Where(o => o.id == id).ToList();
        var dSWorkwareItems = _dbContext.DSWorkwareItems.Include(o => o.DSWorkwareAreas).Where(o => o.WorkwareId == id).ToList();
        _dbContext.DSWorkwareItems.RemoveRange(dSWorkwareItems);
        if(dSWorkwares.Count<=0) return false;
        _dbContext.Dsworkwares.Remove(dSWorkwares[0]);
        return _dbContext.SaveChanges()>0;
    }
}