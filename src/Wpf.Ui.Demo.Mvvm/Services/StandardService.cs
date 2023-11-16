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
using Wpf.Ui.Demo.Mvvm.Helpers;
using Wpf.Ui.Demo.Mvvm.Models;

namespace Wpf.Ui.Demo.Mvvm.Services;
public class StandardService
{
    private readonly EntityDbContext _dbContext;

    public StandardService(EntityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Standard> GetList()
    {
        return _dbContext.Standards.Include(o => o.StandarDatas).ToList();
    }

    public Standard? GetStandard(ProcessFlowEnum processFlow) {
        var data=_dbContext.Standards.Include(o => o.StandarDatas.OrderBy(o=>o.StandardType).OrderBy(o=>o.Id)).FirstOrDefault(o => o.ProcessFlow == processFlow);
        return data;
    }

    public int Save(Standard standard) {
        _ = _dbContext.Standards.Add(standard);
        return _dbContext.SaveChanges();
    }
    public int SaveStandardData(StandardData standardData) {
        _ = _dbContext.StandardDatas.Add(standardData);
        return _dbContext.SaveChanges();
    }
    public int UpdateStandardData(StandardData standardData)
    {
        _ = _dbContext.StandardDatas.Update(standardData);
        return _dbContext.SaveChanges();
    }

    public int Update(Standard standard)
    {
        _ = _dbContext.Standards.Update(standard);
        return _dbContext.SaveChanges();
    }

    public int Deleted(int id) {
        _ = _dbContext.StandardDatas.Remove(_dbContext.StandardDatas.First(x => x.Id == id));
        return _dbContext.SaveChanges();
    }
}
