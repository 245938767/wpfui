// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Demo.Mvvm.Models;

namespace Wpf.Ui.Demo.Mvvm.DbContexts;

public partial class EntityDbContext : DbContext
{
    private readonly IEnumerable<DbModule> _modules;

    public EntityDbContext(DbContextOptions options, IEnumerable<DbModule> modules) : base(options)
    {
        _modules = modules;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        foreach (var each in _modules)
        {
            each.OnModelCreating(builder);
        }

        ConfigureConventions(builder);
    }

    private void ConfigureConventions(ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (entityType.BaseType != null) continue;
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

    public DbSet<DeviceCard> DeviceCards { get; set; }

}