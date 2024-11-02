using Wpf.Ui.Controls;

namespace General.Apt.App.ViewModels.Pages.App
{
    public partial class InfoPageViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        public InfoPageViewModel()
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
            _isInitialized = true;
        }
    }
}
