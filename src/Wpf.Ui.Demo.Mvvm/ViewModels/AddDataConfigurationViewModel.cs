using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Demo.Mvvm.Models;
using Wpf.Ui.Demo.Mvvm.Services;

namespace Wpf.Ui.Demo.Mvvm.ViewModels;


public partial class AddDataConfigurationViewModel:ObservableObject
{
    [ObservableProperty]
    private StandardData _standardData;
    private readonly StandardService _standardService;
    public AddDataConfigurationViewModel(StandardData standard,StandardService standardService) {
        StandardData = standard;
        _standardService = standardService;
    }

    [RelayCommand]
    private void Save() {

        if (StandardData.Id == 0)
        {
            _ = _standardService.SaveStandardData(StandardData);
        }
        else {
            _=_standardService.UpdateStandardData(StandardData);
        }
    }

}
