using Apt.App.Extensions;
using Apt.App.Models;
using Apt.Service.Helpers;
using Microsoft.Extensions.Logging;
using Serilog;
using Utility;

namespace Apt.App
{
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;
        public IHost Host { get; }
        public Microsoft.Extensions.Logging.ILogger Logger { get; }
        public T? GetService<T>() where T : class => Host.Services.GetService<T>();
        public T GetRequiredService<T>() where T : class => Host.Services.GetRequiredService<T>();

        public App()
        {
            var builder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();
            var appSettings = builder.Configuration.Get<AppSettings>();
            builder.Services.AddSingleton(Guard.NotNull(appSettings));
            builder.Services.AddSerilog((services, logger) => logger.ReadFrom.Configuration(builder.Configuration));
            builder.Services.AddBootService();
            Host = builder.Build();
            Logger = GetRequiredService<ILogger<App>>();
        }

        private async void OnStartup(object sender, StartupEventArgs e)
        {
            OnceStart();
            await Host.StartAsync();
        }

        private async void OnExit(object sender, ExitEventArgs e)
        {
            await Host.StopAsync();
            Host.Dispose();
            Environment.Exit(0);
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var exception = e.Exception;
            Logger.LogError(exception.ToString());
            DialogHelper.ShowErrorDialog(exception.Message);
        }

        private void OnceStart()
        {
            var processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            var processes = System.Diagnostics.Process.GetProcessesByName(processName);
            if (processes.Length > 1)
            {
                DialogHelper.ShowErrorDialog("The program is running and cannot be restarted!");
                Environment.Exit(1);
            }
        }
    }
}
