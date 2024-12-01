namespace Apt.App.Services
{
    public class WindowsProviderService
    {
        private readonly IServiceProvider _serviceProvider;

        public WindowsProviderService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T Get<T>() where T : class
        {
            if (!typeof(Window).IsAssignableFrom(typeof(T)))
            {
                throw new InvalidOperationException($"The window class should be derived from {typeof(Window)}.");
            }
            var service = _serviceProvider.GetService<T>();
            if (service is null)
            {
                throw new InvalidOperationException("Window is not registered as service.");
            }
            return service;
        }

        public void Show(Window window)
        {
            window.Owner = App.Current.MainWindow;
            window.Closing += (s, e) => { e.Cancel = true; window.Visibility = Visibility.Hidden; };
            window.Show();
        }

        public bool? ShowDialog(Window window)
        {
            window.Owner = App.Current.MainWindow;
            window.Closing += (s, e) => { e.Cancel = true; window.Visibility = Visibility.Hidden; };
            return window.ShowDialog();
        }

        public void Show<T>() where T : class
        {
            if (!typeof(Window).IsAssignableFrom(typeof(T)))
            {
                throw new InvalidOperationException($"The window class should be derived from {typeof(Window)}.");
            }
            var window = _serviceProvider.GetService<T>() as Window;
            if (window is null)
            {
                throw new InvalidOperationException("Window is not registered as service.");
            }
            window.Owner = Application.Current.MainWindow;
            window.Closing += (s, e) => { e.Cancel = true; window.Visibility = Visibility.Hidden; };
            window.Show();
        }

        public bool? ShowDialog<T>() where T : class
        {
            if (!typeof(Window).IsAssignableFrom(typeof(T)))
            {
                throw new InvalidOperationException($"The window class should be derived from {typeof(Window)}.");
            }
            var window = _serviceProvider.GetService<T>() as Window;
            if (window is null)
            {
                throw new InvalidOperationException("Window is not registered as service.");
            }
            window.Owner = Application.Current.MainWindow;
            window.Closing += (s, e) => { e.Cancel = true; window.Visibility = Visibility.Hidden; };
            return window.ShowDialog();
        }
    }
}
