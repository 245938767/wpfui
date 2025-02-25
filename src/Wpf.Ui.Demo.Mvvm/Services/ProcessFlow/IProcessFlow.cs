using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Demo.Mvvm.DeviceItem;
using Wpf.Ui.Demo.Mvvm.Helpers;
using Wpf.Ui.Demo.Mvvm.Models;

namespace Wpf.Ui.Demo.Mvvm.Services.ProcessFlow;

public abstract class IProcessFlow : IDisposable
{
    protected ProcessFlowEnum ProcessFlow = ProcessFlowEnum.DSTest;
    /// <summary>
    /// 首页表格数据
    /// </summary>
     protected ObservableCollection<object> HomePageItemData = new ObservableCollection<object>();

    public IProcessFlow(ProcessFlowEnum processFlow, ObservableCollection<object> homePageItemData)
    {
        ProcessFlow = processFlow;
        HomePageItemData = homePageItemData;
    }



    /// <summary>
    /// 执行检测
    /// </summary>
    /// <returns> 检测是否成功</returns>
    public abstract Task<bool> ExecutionDetection();

    /// <summary>
    /// 执行程序
    /// </summary>
    /// <returns></returns>
    public abstract Task ExecutionProcess();

    /// <summary>
    /// 检测程序是否在执行
    /// </summary>
    /// <returns></returns>
    public abstract bool CheckExecution();

    /// <summary>
    /// 中断执行
    /// </summary>
    /// <returns></returns>
    public abstract void Dispose();

    /// <summary>
    /// 显示设备初始化失败的错误信息
    /// </summary>
    /// <param name="deviceName"> 设备名称</param>
    /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
    protected async Task ShowDeviceConnnectionError(string deviceName)
    {
        await new Wpf.Ui.Controls.MessageBox
        {
            Title = "设备初始化失败",
            Content =
                $"请检测”{deviceName}“设备是否已经连接",
        }.ShowDialogAsync();
    }
    /// <summary>
    /// 显示设备在初始化过程中事变
    /// </summary>
    /// <param name="deviceName"> 设备名称</param>
    /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
    protected async Task ShowDeviceProcessErrorMessages(string deviceName)
    {
        await new Wpf.Ui.Controls.MessageBox
        {
            Title = "设备测试流程失败",
            Content =
                $"失败原因”{deviceName}“",
        }.ShowDialogAsync();
    }
}