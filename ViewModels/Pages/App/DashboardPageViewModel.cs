using General.Apt.Service.Helpers;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace General.Apt.App.ViewModels.Pages.App
{
    public partial class DashboardPageViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;
        private INavigationService _navigationService;

        public string ApplicationFullTitle => $"{Service.Utility.Language.GetString("ApplicationTitle")} V{Assembly.GetExecutingAssembly().GetName().Version}";

        [RelayCommand]
        private void OnCardClick(string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter))
            {
                return;
            }
            var pageType = PageHelper.ToType(parameter);
            if (pageType == null)
            {
                return;
            }
            _navigationService.Navigate(pageType);
        }

        public DashboardPageViewModel()
        {
            if (!_isInitialized) InitializeViewModel();
        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized) InitializeViewModel();
        }

        public void OnNavigatedFrom() { }

        private void InitializeViewModel()
        {
            _navigationService = Apt.App.App.Current.GetRequiredService<INavigationService>();

            _isInitialized = true;
        }
    }
}
