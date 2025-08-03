using APT.App.Views.Dialogs;
using APT.Service.ViewModels.Base;
using Common.WindowsDesktop.Helpers;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui;

namespace APT.App.ViewModels.Pages.App
{
    public partial class DashboardPageViewModel : BaseViewModel
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;
        private IContentDialogService _contentDialogService;

        [RelayCommand]
        private async Task OnCardClick(string parameter)
        {
            if (parameter == "Plus")
            {
                await new PlusDialog(_contentDialogService.GetDialogHost()).ShowAsync();
                return;
            }
            _navigationService.Navigate(PageHelper.ToType(parameter, "APT.App.Views.Pages", Assembly.GetExecutingAssembly()));
        }

        public DashboardPageViewModel(
            IServiceProvider serviceProvider,
            ISnackbarService snackbarService,
            INavigationService navigationService,
            IContentDialogService contentDialogService) :
            base(serviceProvider, snackbarService)
        {
            _navigationService = navigationService;
            _contentDialogService = contentDialogService;

            if (!_isInitialized) InitializeViewModel();
        }

        public override void OnNavigatedTo()
        {
            if (!_isInitialized) InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            _isInitialized = true;
        }
    }
}
