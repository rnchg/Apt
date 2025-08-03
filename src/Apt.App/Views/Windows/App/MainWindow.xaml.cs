using APT.Service.Utility;
using System.ComponentModel;
using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Controls;

namespace APT.App.Views.Windows.App
{
    public partial class MainWindow : INavigationWindow
    {
        public ViewModels.Windows.App.MainWindowViewModel ViewModel { get; }

        public MainWindow(
            ViewModels.Windows.App.MainWindowViewModel viewModel,
            INavigationService navigationService,
            IContentDialogService contentDialogService,
            ISnackbarService snackbarService)
        {
            ViewModel = viewModel;
            DataContext = this;

            Wpf.Ui.Appearance.SystemThemeWatcher.Watch(this);

            InitializeComponent();

            navigationService.SetNavigationControl(NavigationView);
            contentDialogService.SetDialogHost(ContentPresenter);
            snackbarService.SetSnackbarPresenter(SnackbarPresenter);
        }

        public INavigationView GetNavigation() => NavigationView;

        public bool Navigate(Type pageType) => NavigationView.Navigate(pageType);

        public void SetPageService(INavigationViewPageProvider navigationViewPageProvider) => NavigationView.SetPageProviderService(navigationViewPageProvider);

        public void ShowWindow() => Show();

        public void CloseWindow() => Close();

        protected override async void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            await Message.ShowMessageConfirm(Core.Utility.Language.Instance["Application.ExitConfirm"], cancel: () => e.Cancel = true);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
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
}
