using General.Apt.Service.Utility;

namespace General.Apt.App.ViewModels.Windows
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
        private async Task SetSave()
        {
            if (License.Validate(App.Current.Logger, ActivationCode, out var requestCode, out var message))
            {
                Current.Config.App.ActivationCode = ActivationCode;

                await Utility.Message.ShowMessageInfo(string.Format(Language.GetString("LicenseWindowSaveSuccess"), message));

                CloseAction?.Invoke();
            }
            else
            {
                await Utility.Message.ShowMessageError(string.Format(Language.GetString("LicenseWindowSaveError"), message));
            }
            Message = message;
        }

        public LicenseWindowViewModel()
        {
            if (!_isInitialized) InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            ActivationCode = Current.Config.App.ActivationCode;
            if (!License.TryGetRequestCode(out var requestCode, out var message))
            {
                Message = message;
                return;
            }
            RequestCode = requestCode;
            ValidateActivationCode();

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
