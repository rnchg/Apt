using General.Apt.Service.Utility;

namespace General.Apt.App.ViewModels.Windows.App
{
    public partial class LicenseWindowViewModel : ObservableObject
    {
        private bool _isInitialized = false;

        public Action CloseAction { get; set; }

        [ObservableProperty]
        private string _requestCode;

        [ObservableProperty]
        private string _activationCode;

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
        private string _message;

        public Brush MessageForeground
        {
            get => ActivationCodeResult ? Brushes.Green : Brushes.Red;
        }

        [RelayCommand]
        private async Task GetRequestCode()
        {
            if (License.TryGetRequestCode(out var requestCode, out var message))
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
            if (License.Validate(Apt.App.App.Current.Logger, ActivationCode, out var requestCode, out var message))
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
