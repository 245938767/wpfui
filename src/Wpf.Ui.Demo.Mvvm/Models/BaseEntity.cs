// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Ui.Demo.Mvvm.Models;
public class BaseEntity
{
    [Key]
    [Description("ID")] public int Id { get; init; }
    [Description("创建时间")] public DateTime CreateDateTime { get; set; }
    [Description("更新时间时间")] public DateTime UpdateDateTime { get; set; }
    [Description("乐观锁")][Timestamp] public byte[] Version { get; private set; }

    public void preCreateTime()
    {
        CreateDateTime = DateTime.Now;
        UpdateDateTime = DateTime.Now;
    }

    public void preUpdateTime()
    {
        UpdateDateTime = DateTime.Now;
    }
}