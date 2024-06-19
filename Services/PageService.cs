using Wpf.Ui;

namespace General.Apt.App.Services
{
    public class PageService : IPageService
    {
        private readonly IServiceProvider _serviceProvider;

        public PageService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T GetPage<T>() where T : class
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(typeof(T)))
                throw new InvalidOperationException("The page should be a WPF control.");

            return (T)_serviceProvider.GetService(typeof(T));
        }

        public FrameworkElement GetPage(Type pageType)
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(pageType))
                throw new InvalidOperationException("The page should be a WPF control.");

            return _serviceProvider.GetService(pageType) as FrameworkElement;
        }
    }
}
