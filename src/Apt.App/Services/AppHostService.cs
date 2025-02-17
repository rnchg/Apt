using Apt.App.Extensions;
using Apt.App.Interfaces;
using Apt.App.ViewModels.Pages.App;
using Apt.App.Views.Pages.App;
using Apt.App.Views.Windows.App;
using Apt.Core.Utility;
using Apt.Service.Extensions;
using Common.NETCore.Utility;
using Microsoft.Extensions.Logging;

namespace Apt.App.Services
{
    public class AppHostService : IHostedService
    {
        private readonly ILogger<AppHostService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public AppHostService(
            ILogger<AppHostService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _logger.LogInformation($"[{Session.AssemblyName.Name} V {Session.AssemblyName.Version}]");
            _logger.LogInformation($"--------Init--------");
            Session.ServiceProvider = _serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Current.GetConfig();
            Language.Instance.Init();
            _serviceProvider.GetConfig();
            _logger.LogInformation($"--------Start--------");
            return HandleActivationAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _serviceProvider.SetConfig();
            Current.SetConfig();
            _logger.LogInformation($"--------Stop--------");
            return Task.CompletedTask;
        }

        private Task HandleActivationAsync()
        {
            if (Application.Current.Windows.OfType<MainWindow>().Any())
            {
                return Task.CompletedTask;
            }
            var mainWindow = _serviceProvider.GetRequiredService<IWindow>();
            mainWindow.Loaded += OnMainWindowLoaded;
            mainWindow?.Show();
            return Task.CompletedTask;
        }

        private void OnMainWindowLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is not MainWindow mainWindow)
            {
                return;
            }
            var settingPageViewModel = _serviceProvider.GetRequiredService<SettingPageViewModel>();
            _ = mainWindow.NavigationView.Navigate(typeof(DashboardPage));
            _serviceProvider.ValidateLicense();
        }
    }
}
