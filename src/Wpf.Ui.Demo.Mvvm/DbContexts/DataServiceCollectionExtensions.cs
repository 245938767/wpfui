// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Ui.Demo.Mvvm.DbContexts;

public static class DataServiceCollectionExtensions
{
    /**
         * Other SQL install 
         * https://learn.microsoft.com/zh-cn/ef/core/providers/?tabs=dotnet-core-cli
         */
    public static DbContextOptionsBuilder GetSql(DbContextOptionsBuilder dbContextOptionsBuilder)
    {
         dbContextOptionsBuilder.UseSqlite("Data source = VTUDatabase.db;foreign keys=true");

        return dbContextOptionsBuilder;
    }

    /**
     * 添加对应的Module对象
     */
    public static void AddDbModules(this IServiceCollection services)
    {

    }
}