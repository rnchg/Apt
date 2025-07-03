using Apt.App.Extensions;
using Apt.App.ViewModels.Pages.App;
using Apt.App.Views.Pages.App;
using Apt.App.Views.Windows.App;
using Apt.Core.Utility;
using Apt.Service.Extensions;
using Common.NETCore.Utility;
using Microsoft.Extensions.Logging;
using Wpf.Ui;

namespace Apt.App.Services
{
    public class AppHostService : IHostedService, IAsyncDisposable
    {
        private readonly ILogger<AppHostService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private INavigationWindow? _navigationWindow;

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

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Current.GetConfig();
            Language.Instance.Init();
            _serviceProvider.GetConfig();
            _logger.LogInformation($"--------Start--------");
            await HandleActivationAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _serviceProvider.SetConfig();
            Current.SetConfig();
            _logger.LogInformation($"--------Stop--------");
            await Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            _logger.LogInformation($"--------Dispose--------");
            await ValueTask.CompletedTask;
        }

        private async Task HandleActivationAsync()
        {
            await Task.CompletedTask;
            if (!Application.Current.Windows.OfType<MainWindow>().Any())
            {
                _navigationWindow = (_serviceProvider.GetService(typeof(INavigationWindow)) as INavigationWindow)!;
                _navigationWindow!.ShowWindow();

                _ = _navigationWindow.Navigate(typeof(DashboardPage));
                _ = _serviceProvider.GetRequiredService<SettingPageViewModel>();
                _ = _serviceProvider.ValidateLicense();
            }
            await Task.CompletedTask;
        }
    }
}