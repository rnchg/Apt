using General.Apt.App.Services.Contracts;
using General.Apt.App.Utility;
using General.Apt.App.Views.Pages.App;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace General.Apt.App.Views.Windows
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : IWindow
    {
        public ViewModels.Windows.MainWindowViewModel ViewModel { get; }

        public MainWindow(
            ViewModels.Windows.MainWindowViewModel viewModel,
            INavigationService navigationService,
            IServiceProvider serviceProvider,
            ISnackbarService snackbarService,
            IContentDialogService contentDialogService)
        {
            Wpf.Ui.Appearance.SystemThemeWatcher.Watch(this);

            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            snackbarService.SetSnackbarPresenter(SnackbarPresenter);
            navigationService.SetNavigationControl(NavigationView);
            contentDialogService.SetDialogHost(RootContentDialog);

            NavigationView.SetServiceProvider(serviceProvider);
        }

        private bool _isUserClosedPane;

        private bool _isPaneOpenedOrClosedFromCode;

        private void OnNavigationSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (sender is not NavigationView navigationView)
            {
                return;
            }

            NavigationView.SetCurrentValue(
                NavigationView.HeaderVisibilityProperty,
                navigationView.SelectedItem?.TargetPageType != typeof(DashboardPage)
                    ? Visibility.Visible
                    : Visibility.Collapsed
            );
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_isUserClosedPane)
            {
                return;
            }

            _isPaneOpenedOrClosedFromCode = true;
            NavigationView.SetCurrentValue(NavigationView.IsPaneOpenProperty, e.NewSize.Width > 1200);
            _isPaneOpenedOrClosedFromCode = false;
        }

        private void NavigationView_OnPaneOpened(NavigationView sender, RoutedEventArgs args)
        {
            if (_isPaneOpenedOrClosedFromCode)
            {
                return;
            }

            _isUserClosedPane = false;
        }

        private void NavigationView_OnPaneClosed(NavigationView sender, RoutedEventArgs args)
        {
            if (_isPaneOpenedOrClosedFromCode)
            {
                return;
            }

            _isUserClosedPane = true;
        }

        private async void FluentWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await Message.ShowMessageConfirm(Service.Utility.Language.GetString("MainWindowExitConfirm"), cancel: () => e.Cancel = true);
        }
    }
}
