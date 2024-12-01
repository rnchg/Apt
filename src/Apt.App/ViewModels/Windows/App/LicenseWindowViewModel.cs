using Apt.App.ViewModels.Base;
using Apt.Service.Utility;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Apt.App.ViewModels.Windows.App
{
    public partial class LicenseWindowViewModel : BaseViewModel
    {
        private bool _isInitialized = false;

        public Action CloseAction { get; set; } = null!;

        [ObservableProperty]
        private string _requestCode = null!;

        [ObservableProperty]
        private string _activationCode = null!;

        partial void OnActivationCodeChanged(string value)
        {
            ValidateActivationCode();
        }

        [ObservableProperty]
        private bool _activationCodeResult;

        partial void OnActivationCodeResultChanged(bool value)
        {
            OnPropertyChanged(nameof(MessageForeground));
        }

        [ObservableProperty]
        private string _message = null!;

        public Brush MessageForeground
        {
            get => ActivationCodeResult ? Brushes.Green : Brushes.Red;
        }

        [RelayCommand]
        private async Task GetRequestCode()
        {
            if (License.TryGetRequestCode(out var requestCode, out var message) && requestCode is not null)
            {
                RequestCode = requestCode;

                await Utility.Message.ShowMessageInfo(string.Format(Language.Instance["LicenseWindowGetRequestCodeSuccess"], message));
            }
            else
            {
                await Utility.Message.ShowMessageError(string.Format(Language.Instance["LicenseWindowGetRequestCodeError"], message));
            }
            Message = message;
        }

        [RelayCommand]
        private async Task SetSave()
        {
            if (License.Validate(ActivationCode, out var requestCode, out var message))
            {
                Current.Config.App.ActivationCode = ActivationCode;

                await Utility.Message.ShowMessageInfo(string.Format(Language.Instance["LicenseWindowSetSaveSuccess"], message));

                CloseAction?.Invoke();
            }
            else
            {
                await Utility.Message.ShowMessageError(string.Format(Language.Instance["LicenseWindowSetSaveError"], message));
            }
            Message = message;
        }

        [RelayCommand]
        private void SetClose()
        {
            CloseAction?.Invoke();
        }

        public LicenseWindowViewModel()
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

        public bool ValidateActivationCode()
        {
            ActivationCodeResult = License.TryValidateActivationCode(ActivationCode, out var message);
            Message = message;
            return ActivationCodeResult;
        }
    }
}
