// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

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
}