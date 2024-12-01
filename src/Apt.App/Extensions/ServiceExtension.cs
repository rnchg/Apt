using Apt.App.Providers;
using Apt.App.Services;
using Apt.App.ViewModels.Windows.App;
using Apt.App.Views.Windows.App;
using Apt.Service.Interfaces;
using Wpf.Ui;
using Wpf.Ui.Abstractions;

namespace Apt.App.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddNavigationViewPageProvider(this IServiceCollection services)
        {
            services.AddSingleton<INavigationViewPageProvider, DependencyInjectionNavigationViewPageProvider>();
            return services;
        }

        public static IServiceCollection AddFromNamespace(this IServiceCollection services, ServiceLifetime serviceLifetime, string namespaceName, params Assembly[] assemblies)
        {
            var types = assemblies.Select(x => x.GetTypes()).SelectMany(x => x).Where(x => !x.IsAbstract && (x.Namespace?.StartsWith(namespaceName) ?? false)).ToList();
            types.ForEach(x => services.Add(new ServiceDescriptor(x, x, serviceLifetime)));
            return services;
        }

        public static IServiceCollection AddBootService(this IServiceCollection services)
        {
            services.AddNavigationViewPageProvider();

            services.AddHostedService<AppHostService>();

            services.AddSingleton<IWindow, MainWindow>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<ISnackbarService, SnackbarService>();
            services.AddSingleton<IContentDialogService, ContentDialogService>();
            services.AddSingleton<WindowsProviderService>();

            services.AddFromNamespace(ServiceLifetime.Singleton, "Apt.App.Views", Assembly.GetExecutingAssembly());
            services.AddFromNamespace(ServiceLifetime.Singleton, "Apt.App.ViewModels", Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
