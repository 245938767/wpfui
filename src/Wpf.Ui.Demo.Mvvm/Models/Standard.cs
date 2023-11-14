// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Demo.Mvvm.Helpers;

namespace Wpf.Ui.Demo.Mvvm.Models;

/// <summary>
/// 测试数据对象集
/// </summary>
public class Standard
{
    public int Id { get; set; }
    [Description("数据名称")]
    public string Name { get; set; }
    [Description("流程类型")]
    public ProcessFlowEnum ProcessFlow { get; set; }

    [Description("数据")]
    public ICollection<StandardData> StandarDatas { get; set; } = new List<StandardData>();

}
/// <summary>
/// DS 工装 标准对象
/// </summary>
public class StandardData
{
    public int Id { get; set; }
    [Description("父Id")]
    public int StandardId { get; set; }
    [Description("配置类型")]
    public StandardEnum StandardType { get; set; }

    /// <summary>
    /// 标准值
    /// </summary>
    [Description("标准值")]

    public float Value { get; set; }

    /// <summary>
    /// 阈值
    /// </summary>
    [Description("阈值")]
    public float ThresholdValue { get; set; }
    public Standard Standard { get; set; }
}

/// <summary>
/// 标准类型
/// </summary>
public enum StandardEnum
{
    Pressure,
    Temperature,
}