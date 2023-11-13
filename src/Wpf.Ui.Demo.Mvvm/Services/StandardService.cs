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
}
