using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Demo.Mvvm.Models;
using Wpf.Ui.Demo.Mvvm.Services;

namespace Wpf.Ui.Demo.Mvvm.ViewModels;


public partial class AddDataConfigurationViewModel : ObservableObject
{
    [ObservableProperty]
    private StandardData _standardData;
    [ObservableProperty]
    private string _name;

    private readonly StandardService standardService;
    public AddDataConfigurationViewModel(StandardData standard,string name,StandardService standardService) {
        _standardData = standard;
        _name = name;
        this.standardService = standardService;
    }

    [RelayCommand]
    private void Save()
    {

        if (StandardData.Id == 0)
        {
            _ = standardService.SaveStandardData(StandardData);
        }
        else
        {
            _ = standardService.UpdateStandardData(StandardData);
        }
    }

}
