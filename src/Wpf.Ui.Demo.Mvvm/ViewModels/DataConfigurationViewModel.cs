// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using System.Windows.Media;
using Wpf.Ui.Controls;
using Wpf.Ui.Demo.Mvvm.Models;

namespace Wpf.Ui.Demo.Mvvm.ViewModels;

public partial class DataConfigurationViewModel : ObservableObject, INavigationAware
{
    private bool _isInitialized = false;

    [ObservableProperty]
    private ObservableCollection<object> _navigationItems = new();


    public void OnNavigatedTo()
    {
        if (!_isInitialized)
            InitializeViewModel();
    }

    public void OnNavigatedFrom() { }

    private void InitializeViewModel()
    {
        NavigationItems.Clear();
        NavigationItems = new ObservableCollection<object>();

        NavigationItems.Add(new NavigationViewItem()
        {
            Content = "Home",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },

        });
           NavigationItems.Add( new NavigationViewItem()
            {
                Content = "Data",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
            });
          NavigationItems.Add(new NavigationViewItem() {
              Content = "Configuration",
              Icon = new SymbolIcon { Symbol = SymbolRegular.Accessibility20 },
          }
          );
       

        _isInitialized = true;
    }
}
