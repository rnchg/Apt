using Wpf.Ui.Abstractions;

namespace Apt.App.Providers
{
    public class DependencyInjectionNavigationViewPageProvider(IServiceProvider serviceProvider) : INavigationViewPageProvider
    {
        public object? GetPage(Type pageType)
        {
            return serviceProvider.GetService(pageType);
        }
    }
}
