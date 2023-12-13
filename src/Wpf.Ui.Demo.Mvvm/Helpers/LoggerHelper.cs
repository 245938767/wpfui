// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Demo.Mvvm.Models;
using Wpf.Ui.Demo.Mvvm.ViewModels;

namespace Wpf.Ui.Demo.Mvvm.Helpers;
public class LoggerHelper
{
    private static LoggerHelper instance;
    private static object lockObject = new object();

    private LoggerHelper()
    {
        // 初始化日志配置，例如选择文件路径等
    }

    public static LoggerHelper Instance
    {
        get
        {
            lock (lockObject)
            {
                return instance ?? (instance = new LoggerHelper());
            }
        }
    }

    public void Log(string message)
    {
        // 记录日志到文件或其他目标
        // 确保线程安全
        lock (lockObject)
        {
            DateTime nowDate = DateTime.Now;

            // 实际日志记录逻辑
            var log = new LogMessage
            {
                CreateTime = nowDate,
                Message = message,
                SendDirection = MessageSendDirection.RECEIVE
            };
            GlobalData.Instance.LogMessages.Add(log);
            GlobalData.Instance.LogStringMessages += $"{nowDate} : {message} \n";
        }
    }
}
