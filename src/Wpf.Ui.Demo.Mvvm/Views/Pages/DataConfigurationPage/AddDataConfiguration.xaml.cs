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
using System.Windows.Shapes;
using static Wpf.Ui.Demo.Mvvm.Views.Pages.DevicePortConnectPage;
using Wpf.Ui.Demo.Mvvm.Models;

namespace Wpf.Ui.Demo.Mvvm.Views.Pages.DataConfigurationPage
{
    /// <summary>
    /// AddDataConfiguration.xaml 的交互逻辑
    /// </summary>
    public partial class AddDataConfiguration 
    {
        public readonly ViewModels.AddDataConfiguration ViewModel;
        public AddDataConfiguration(ViewModels.AddDataConfiguration viewModel)
        {
            this.ViewModel = viewModel;
            InitializeComponent();
        }
        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void OK_OnClickAsync(object sender, RoutedEventArgs e)
        {

        }
    }
   
}
