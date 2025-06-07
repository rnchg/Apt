using Apt.Core.Utility;
using Apt.Service.Extensions;
using Apt.Service.ViewModels.Base;
using Common.WindowsDesktop.Helpers;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui;

namespace Apt.App.ViewModels.Pages.App
{
    public partial class DashboardPageViewModel : BaseViewModel
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;

        [RelayCommand]
        private void OnCardClick(string parameter)
        {
            if (parameter == "StayTuned")
            {
                SnackbarService.ShowSnackbarInfo(Language.Instance["DashboardPageStayTuned"], timeout: 60);
                return;
            }
            _navigationService.Navigate(PageHelper.ToType(parameter, "Apt.App.Views.Pages", Assembly.GetExecutingAssembly()));
        }

        public DashboardPageViewModel(
            IServiceProvider serviceProvider,
            ISnackbarService snackbarService,
            INavigationService navigationService) :
            base(serviceProvider, snackbarService)
        {
            _navigationService = navigationService;

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
