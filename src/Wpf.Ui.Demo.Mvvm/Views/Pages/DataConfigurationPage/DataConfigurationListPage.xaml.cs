using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Controls;
using Wpf.Ui.Demo.Mvvm.Models;

namespace Wpf.Ui.Demo.Mvvm.Views.Pages.DataConfigurationPage;

/// <summary>
/// DataConfigurationListPage.xaml 的交互逻辑
/// </summary>
public partial class DataConfigurationListPage : INavigableView<ViewModels.DataConfigurationListViewModel>
{
    public ViewModels.DataConfigurationListViewModel ViewModel { get; init; }

    public DataConfigurationListPage(ViewModels.DataConfigurationListViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.InitCommand.Execute(null);
    }
    private void ListView_Edit(object sender, RoutedEventArgs e)
    {
        var selectedItem = (StandardData)ListViewx.SelectedItem;
        ViewModel.EditCommand.Execute(selectedItem);
    }

    private void ListView_Deleted(object sender, RoutedEventArgs e)
    {
        var selectedItem = (StandardData)ListViewx.SelectedItem;
        ViewModel.DeletedCommand.Execute(selectedItem);
    }

}
