// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.IO;
using System.Windows.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wpf.Ui.Controls;
using Wpf.Ui.Demo.Mvvm.DbContexts;
using Wpf.Ui.Demo.Mvvm.Models;
using Wpf.Ui.Demo.Mvvm.Services;

namespace Wpf.Ui.Demo.Mvvm;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    // The.NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    private static readonly IHost _host = Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(c =>
        {
            c.SetBasePath(Path.GetDirectoryName(AppContext.BaseDirectory));
        })
        .ConfigureServices(
            (context, services) =>
            {
                // App Hos// init DB ORM
                services.AddDbContext<EntityDbContext>((_, o) => DataServiceCollectionExtensions.GetSql(o));
                services.AddSingleton<Func<EntityDbContext>>(s =>
                {
                    var options = DataServiceCollectionExtensions.GetSql(new DbContextOptionsBuilder<EntityDbContext>()).Options;
                    return () => new EntityDbContext(options);
                });
                // add Module
                services.AddDbModules(); 
                services.AddHostedService<ApplicationHostService>();

                // Page resolver service
                services.AddSingleton<IPageService, PageService>();
                services.AddSingleton<IContentDialogService, ContentDialogService>();
                services.AddSingleton<DeviceService>();
                services.AddSingleton<StandardService>();
                services.AddSingleton<DSWorkwareService>();

                // Theme manipulation
                services.AddSingleton<IThemeService, ThemeService>();

                // TaskBar manipulation
                services.AddSingleton<ITaskBarService, TaskBarService>();

                // Service containing navigation, same as INavigationWindow... but without window
                services.AddSingleton<INavigationService, NavigationService>();

                // Main window with navigation
                services.AddSingleton<INavigationWindow, Views.MainWindow>();
                services.AddSingleton<ViewModels.MainWindowViewModel>();
                services.AddSingleton<WindowsProviderService>();
                services.AddSingleton<ISnackbarService, SnackbarService>();


                // Views and ViewModels
                services.AddSingleton<Views.Pages.DashboardPage>();
                services.AddSingleton<ViewModels.DashboardViewModel>();
                services.AddSingleton<Views.Pages.DataPage>();
                services.AddSingleton<ViewModels.DataViewModel>();
                services.AddSingleton<Views.Pages.SettingsPage>();
                services.AddSingleton<ViewModels.SettingsViewModel>();
                services.AddSingleton<Views.Pages.DevicePortConnectPage>();
                services.AddSingleton<ViewModels.DevicePortConnectViewModel>();
                services.AddSingleton<Views.Pages.DataConfigurationPage.DataConfigurationListPage>();
                services.AddSingleton<ViewModels.DataConfigurationListViewModel>();
                services.AddSingleton<Views.Pages.DataConfigurationPage.AddDataConfiguration>();
                services.AddSingleton<ViewModels.AddDataConfigurationViewModel>();





                // Configuration
                services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
            }
        )
        .Build();

    /// <summary>
    /// Gets registered service.
    /// </summary>
    /// <typeparam name="T">Type of the service to get.</typeparam>
    /// <returns>Instance of the service or <see langword="null"/>.</returns>
    public static T? GetService<T>()
        where T : class
    {
        return _host.Services.GetService(typeof(T)) as T;
    }

    /// <summary>
    /// Occurs when the application is loading.
    /// </summary>
    private async void OnStartup(object sender, StartupEventArgs e)
    {
        await _host.StartAsync();
    }

    /// <summary>
    /// Occurs when the application is closing.
    /// </summary>
    private async void OnExit(object sender, ExitEventArgs e)
    {
        await _host.StopAsync();

        _host.Dispose();
    }

    /// <summary>
    /// Occurs when an exception is thrown by an application but not handled.
    /// </summary>
    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
    }
}