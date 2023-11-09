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
public interface DbModule
{
    void OnModelCreating(ModelBuilder modelBuilder);
}

public static class DbModuleServiceCollectionExtensions
{
    public static void AddDbModule<T>(this IServiceCollection services)
        where T : class, DbModule
    {
        services.AddSingleton<DbModule, T>();
    }
}