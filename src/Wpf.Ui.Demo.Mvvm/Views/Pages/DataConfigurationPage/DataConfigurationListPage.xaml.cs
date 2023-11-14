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

namespace Wpf.Ui.Demo.Mvvm.Views.Pages.DataConfigurationPage;

/// <summary>
/// DataConfigurationListPage.xaml 的交互逻辑
/// </summary>
public partial class DataConfigurationListPage : INavigableView<ViewModels.DataConfigurationListViewModel>
{
    public ViewModels.DataConfigurationListViewModel ViewModel { get; }

    public DataConfigurationListPage(ViewModels.DataConfigurationListViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }
}
