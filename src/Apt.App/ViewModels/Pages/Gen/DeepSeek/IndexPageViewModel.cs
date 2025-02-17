using Apt.App.Models;
using Apt.App.Views.Windows.Gen.DeepSeek;
using Apt.Core.Consts;
using Apt.Core.Exceptions;
using Apt.Core.Services.Pages.Gen.DeepSeek;
using Apt.Core.Utility;
using Apt.Service.Controls.GenView.DeepSeek;
using Apt.Service.Extensions;
using Apt.Service.Services;
using Apt.Service.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui;

namespace Apt.App.ViewModels.Pages.Gen.DeepSeek
{
    public partial class IndexPageViewModel : BaseViewModel
    {
        private readonly AppSettings _appSettings;

        private IndexService _indexService = null!;

        private bool _isInitialized = false;

        private Model _assistant = null!;

        [ObservableProperty]
        private Func<Model, bool> _addModel = null!;

        [ObservableProperty]
        private Func<Model, bool> _sendModel = null!;

        [ObservableProperty]
        private Func<Model, (string, Model)> _sendAndBuildModel = null!;

        [ObservableProperty]
        private Action _setGenViewCancel = null!;

        [ObservableProperty]
        private Action _setGenViewClear = null!;

        private CancellationTokenSource _cancellationTokenSource = null!;

        [ObservableProperty]
        private int _promptMaxLength = Current.Config.GenDeepSeek.PromptMaxLength;
        partial void OnPromptMaxLengthChanged(int value)
        {
            MessageHeader = $"{Language.Instance["GenDeepSeekIndexPageMessage"]} [ {Message.Length}/{value} ]";
        }

        [ObservableProperty]
        private string _messageHeader = null!;

        [ObservableProperty]
        private string _placeholder = null!;

        [ObservableProperty]
        private string _message = string.Empty;

        partial void OnMessageChanged(string value)
        {
            MessageHeader = $"{Language.Instance["GenDeepSeekIndexPageMessage"]} [ {value.Length}/{PromptMaxLength} ]";
        }

        [ObservableProperty]
        private bool _messageEnabled = false;

        [ObservableProperty]
        private bool _sendEnabled = true;

        [ObservableProperty]
        private ObservableCollection<Model> _genViewModels = [];

        [RelayCommand]
        private void SetSend() => _ = Send();

        [RelayCommand]
        private void SetCancel()
        {
            _cancellationTokenSource?.Cancel();
            Message = string.Empty;
        }

        [RelayCommand]
        private void SetReset()
        {
            SetCancel();
            SetGenViewClear.Invoke();
        }

        [RelayCommand]
        private void SetConfig()
        {
            ServiceProvider.GetRequiredService<WindowsProviderService>().ShowDialog<ConfigWindow>();
        }

        public IndexPageViewModel(
            IServiceProvider serviceProvider,
            ISnackbarService snackbarService,
            AppSettings appSettings) :
            base(serviceProvider, snackbarService)
        {
            _appSettings = appSettings;

            if (!_isInitialized) InitializeViewModel();
        }

        public override void OnNavigatedTo()
        {
            if (!_isInitialized) InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            GenViewModels.Add(new Model() { Type = GenConst.DeepSeek.TypeSystem, Text = Language.Instance["GenDeepSeekHelp"] });

            _indexService = new IndexService()
            {
                SetSequence = (sequence) => _assistant.Text += sequence
            };

            _ = Init();

            _isInitialized = true;
        }

        private async Task Init()
        {
            try
            {
                if (_appSettings.App.Pack == "Full")
                {
                    Placeholder = Language.Instance["GenDeepSeekIndexPageModelInitWait"];
                    await Task.Run(_indexService.Init);
                    MessageEnabled = true;
                    Placeholder = Language.Instance["GenDeepSeekIndexPageInputPrompt"];
                }
                else
                {
                    Placeholder = Language.Instance["GenDeepSeekIndexPageLite"];
                }
            }
            catch (Exception ex)
            {
                Placeholder = Language.Instance["GenDeepSeekIndexPageModelInitFailed"];
                SnackbarService.ShowSnackbarError(ex.Message);
            }
        }

        private async Task Send()
        {
            try
            {
                if (!SendEnabled) return;

                if (string.IsNullOrWhiteSpace(Message)) return;

                SendEnabled = false;

                Placeholder = Language.Instance["GenDeepSeekIndexPageModelProcessWait"];

                var prompt = string.Empty;

                (prompt, _assistant) = SendAndBuildModel.Invoke(new Model() { Type = GenConst.DeepSeek.TypeUser, Text = Message });

                Message = string.Empty;

                _cancellationTokenSource = new CancellationTokenSource();

                await Task.Run(() => _indexService.Start(prompt, _cancellationTokenSource.Token));
            }
            catch (ActivationException ex)
            {
                SetGenViewCancel.Invoke();
                ServiceProvider.ShowLicense(ex.Message);
            }
            catch (OperationCanceledException)
            {
                SetGenViewCancel.Invoke();
            }
            catch (Exception ex)
            {
                SetGenViewCancel.Invoke();
                SnackbarService.ShowSnackbarError(ex.Message);
            }
            finally
            {
                SendEnabled = true;
                Placeholder = Language.Instance["GenDeepSeekIndexPageInputPrompt"];
            }
        }
    }
}