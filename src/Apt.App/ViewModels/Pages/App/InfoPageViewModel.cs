using Apt.Service.ViewModels.Base;
using Wpf.Ui;

namespace Apt.App.ViewModels.Pages.App
{
    public partial class InfoPageViewModel : BaseViewModel
    {
        private bool _isInitialized = false;

        public InfoPageViewModel(
            IServiceProvider serviceProvider,
            ISnackbarService snackbarService) :
            base(serviceProvider, snackbarService)
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
