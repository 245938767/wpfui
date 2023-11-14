using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;
using Wpf.Ui.Demo.Mvvm.Helpers;
using Wpf.Ui.Demo.Mvvm.Models;
using Wpf.Ui.Demo.Mvvm.Services;

namespace Wpf.Ui.Demo.Mvvm.ViewModels;

public partial class DataConfigurationListViewModel : ObservableObject
{
    private readonly StandardService standardService;
    public DataConfigurationListViewModel( StandardService standardService)
    {
        this.standardService = standardService;
        InitializeViewModel();
    }
    [ObservableProperty]
    private Standard _standard;

    /// <summary>
    /// 配置页面流程选择
    /// </summary>
    [ObservableProperty]
    private ProcessFlowEnum _processFlowEnum = ProcessFlowEnum.DSTest;

    private void InitializeViewModel()
    {        //  获得全局变量
        // 获得当前的选择的数据
        Standard? standard = standardService.GetStandard(ProcessFlowEnum);
        if (standard == null) {
            standard = new Standard
            {
                Name = ProcessFlowEnum.ToDescription(),
                ProcessFlow = ProcessFlowEnum,
            };
            standardService.Save(standard);
        }

        Standard = standard;
    }
}
