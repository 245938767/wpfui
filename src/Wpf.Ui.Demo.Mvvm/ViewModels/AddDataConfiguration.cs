using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Demo.Mvvm.Models;

namespace Wpf.Ui.Demo.Mvvm.ViewModels;


public partial class AddDataConfiguration:ObservableObject
{
    [ObservableProperty]
    private Standard _standard;

    [RelayCommand]
    public void Save(StandardData standardData) {
    
    }

}
