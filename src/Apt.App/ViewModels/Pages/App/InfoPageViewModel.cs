using Apt.App.ViewModels.Base;

namespace Apt.App.ViewModels.Pages.App
{
    public partial class InfoPageViewModel : BaseViewModel
    {
        private bool _isInitialized = false;

        public InfoPageViewModel()
        {
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
