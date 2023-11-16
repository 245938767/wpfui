using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.VoiceCommands;
using Wpf.Ui.Controls;
using Wpf.Ui.Demo.Mvvm.Helpers;
using Wpf.Ui.Demo.Mvvm.Models;
using Wpf.Ui.Demo.Mvvm.Services;
using Wpf.Ui.Demo.Mvvm.Views.Pages.DataConfigurationPage;

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
    private Standard? _standard;

    /// <summary>
    /// 配置页面流程选择
    /// </summary>
    [ObservableProperty]
    private ProcessFlowEnum _processFlowEnum = ProcessFlowEnum.DSTest;

    private void InitializeViewModel()
    {      
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

    [RelayCommand]
    private void Init()
    {
        InitializeViewModel();
    }

    [RelayCommand]
    private void EditOrCreate() {
            var standardData = new StandardData();
            standardData.StandardId = Standard.Id;
            var addDataConfiguration = new AddDataConfigurationViewModel(standardData,Standard.Name, standardService);
            var addDataConfigurationPage = new AddDataConfiguration(addDataConfiguration);
            addDataConfigurationPage.Show();
            InitializeViewModel();
    }

    [RelayCommand]
    private void Edit(StandardData standard) {
        var addDataConfiguration = new AddDataConfigurationViewModel(standard,Standard.Name, standardService);
        var addDataConfigurationPage = new AddDataConfiguration(addDataConfiguration);
        addDataConfigurationPage.Show();
    }

    [RelayCommand]
    private void Deleted(StandardData standard) {
        _ = standardService.Deleted(standard.Id);
        var v = Standard.StandarDatas.Remove(standard);

    }
}
