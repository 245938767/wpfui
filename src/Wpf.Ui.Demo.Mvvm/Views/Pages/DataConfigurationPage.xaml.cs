// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Navigation;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Demo.Mvvm.Views.Pages;

/// <summary>
/// Interaction logic for DataConfigurationPage.xaml
/// </summary>
public partial class DataConfigurationPage : INavigableView<ViewModels.DataConfigurationViewModel>
{
    public ViewModels.DataConfigurationViewModel ViewModel { get; }

    public DataConfigurationPage(ViewModels.DataConfigurationViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();

    }
}
