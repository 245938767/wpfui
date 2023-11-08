// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;
using Wpf.Ui.Demo.Mvvm.ViewModels;

namespace Wpf.Ui.Demo.Mvvm.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : INavigationWindow
{
    public ViewModels.MainWindowViewModel ViewModel { get; }
    private readonly IContentDialogService _contentDialogService;

    public MainWindow(
        ViewModels.MainWindowViewModel viewModel,
        IPageService pageService,
        INavigationService navigationService,
        IContentDialogService contentDialogService
    )
    {
        ViewModel = viewModel;
        DataContext = this;
        _contentDialogService = contentDialogService;

        
        Appearance.SystemThemeWatcher.Watch(this);

        InitializeComponent();
        SetPageService(pageService);

        contentDialogService.SetContentPresenter(RootContentDialog);
        navigationService.SetNavigationControl(RootNavigation);
    }

    #region INavigationWindow methods

    public INavigationView GetNavigation() => RootNavigation;

    public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

    public void SetPageService(IPageService pageService) => RootNavigation.SetPageService(pageService);

    public void ShowWindow() => Show();

    public void CloseWindow() => Close();

    #endregion INavigationWindow methods

    /// <summary>
    /// Raises the closed event.
    /// </summary>
    protected override async void OnClosed(EventArgs e)
    {
        var isOpenCheck = GlobalData.Instance.IsOpenCheck;
        if (isOpenCheck) {
            ContentDialogResult result = await _contentDialogService.ShowSimpleDialogAsync(
              new SimpleContentDialogCreateOptions()
              {
                  Title = "正在运行提醒",
                  Content = $"测试正在运行是否关闭？",
                  PrimaryButtonText = "确定",
                  CloseButtonText = "取消",
              }
          );
            if (result == ContentDialogResult.Secondary) {
                return;
            }
        }
        base.OnClosed(e);

        // Make sure that closing this window will begin the process of closing the application.
        Application.Current.Shutdown();
    }

    INavigationView INavigationWindow.GetNavigation()
    {
        throw new NotImplementedException();
    }

    public void SetServiceProvider(IServiceProvider serviceProvider)
    {
        throw new NotImplementedException();
    }
}
