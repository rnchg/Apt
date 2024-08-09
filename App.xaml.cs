using General.Apt.App.Models;
using General.Apt.App.Services;
using General.Apt.App.Services.Contracts;
using General.Apt.App.Utility;
using General.Apt.App.ViewModels.Pages.App;
using General.Apt.App.ViewModels.Windows.App;
using General.Apt.App.Views.Pages.App;
using General.Apt.App.Views.Windows.App;
using General.Apt.Service.Extensions;
using Microsoft.Extensions.Logging;
using Serilog;
using Wpf.Ui;

namespace General.Apt.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;
        public IHost Host { get; }
        public Microsoft.Extensions.Logging.ILogger Logger { get; }
        public T GetService<T>() where T : class => Host.Services.GetService<T>();
        public T GetRequiredService<T>() where T : class => Host.Services.GetRequiredService<T>();

        public App()
        {
            var builder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();
            var appSettings = builder.Configuration.Get<AppSettings>();
            appSettings.App.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            builder.Services.AddSingleton(appSettings);
            builder.Services.AddSerilog((services, logger) => logger.ReadFrom.Configuration(builder.Configuration));
            builder.Services.AddSingleton<IWindow, MainWindow>();
            builder.Services.AddSingleton<MainWindowViewModel>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<ISnackbarService, SnackbarService>();
            builder.Services.AddSingleton<IContentDialogService, ContentDialogService>();
            builder.Services.AddSingleton<WindowsProviderService>();
            builder.Services.AddSingleton<DashboardPage>();
            builder.Services.AddSingleton<DashboardPageViewModel>();
            builder.Services.AddSingleton<SettingsPage>();
            builder.Services.AddSingleton<SettingsPageViewModel>();
            builder.Services.AddFromNamespace(ServiceLifetime.Singleton, "General.Apt.App.Views", Assembly.GetExecutingAssembly());
            builder.Services.AddFromNamespace(ServiceLifetime.Singleton, "General.Apt.App.ViewModels", Assembly.GetExecutingAssembly());
            builder.Services.AddHostedService<AppHostService>();
            Host = builder.Build();
            Logger = GetRequiredService<ILogger<App>>();
        }

        private async void OnStartup(object sender, StartupEventArgs e)
        {
            var processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            var processes = System.Diagnostics.Process.GetProcessesByName(processName);
            if (processes.Length > 1)
            {
                Dialog.ShowErrorDialog("The program is running and cannot be restarted!");
                Environment.Exit(1);
            }
            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            await Host.StartAsync();
        }

        private async void OnExit(object sender, ExitEventArgs e)
        {
            await Host.StopAsync();
            Host.Dispose();
            Environment.Exit(0);
        }

        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var exception = e.Exception;
            ExceptionHandler(exception.GetBaseException());
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            ExceptionHandler(exception.GetBaseException());
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            var exception = e.Exception;
            ExceptionHandler(exception.GetBaseException());
        }

        private void ExceptionHandler(Exception exception)
        {
            Logger.LogError(exception.ToString());
            Dialog.ShowErrorDialog(exception.Message);
            Host.StopAsync().GetAwaiter().GetResult();
        }
    }
}
