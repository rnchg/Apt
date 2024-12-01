using Apt.App.ViewModels.Base;
using Apt.Service.Helpers;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui;

namespace Apt.App.ViewModels.Pages.App
{
    public partial class DashboardPageViewModel : BaseViewModel
    {
        private INavigationService _navigationService;

        [RelayCommand]
        private void OnCardClick(string parameter) => _navigationService.Navigate(PageHelper.ToType(parameter, "Apt.App.Views.Pages", Assembly.GetExecutingAssembly()));

        public DashboardPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            InitializeViewModel();
        }

        private void InitializeViewModel()
        {

        }
    }
}
