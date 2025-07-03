using Apt.App.Models;
using Apt.App.Views.Windows.Gen.Chat;
using Apt.Core.Consts;
using Apt.Core.Exceptions;
using Apt.Core.Services.Pages.Gen.Chat;
using Apt.Core.Utility;
using Apt.Service.Controls.ChatView;
using Apt.Service.Extensions;
using Apt.Service.Services;
using Apt.Service.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui;

namespace Apt.App.ViewModels.Pages.Gen.Chat
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
        private Action _setViewCancel = null!;

        [ObservableProperty]
        private Action _setViewClear = null!;

        private CancellationTokenSource _cancellationTokenSource = null!;

        [ObservableProperty]
        private int _promptMaxLength = Current.Config.GenChat.PromptMaxLength;
        partial void OnPromptMaxLengthChanged(int value)
        {
            MessageLength = $"[ {Message.Length}/{value} ]";
        }

        [ObservableProperty]
        private string _messageInfo = null!;

        [ObservableProperty]
        private string _messageLength = null!;

        [ObservableProperty]
        private string _message = string.Empty;

        partial void OnMessageChanged(string value)
        {
            MessageLength = $"[ {value.Length}/{PromptMaxLength} ]";
        }

        [ObservableProperty]
        private bool _messageEnabled = false;

        [ObservableProperty]
        private bool _sendEnabled = true;

        [ObservableProperty]
        private ObservableCollection<Model> _models = [];

        [RelayCommand]
        private void SetSend() => _ = Send();

        [RelayCommand]
        private void SetCancel()
        {
            _cancellationTokenSource?.Cancel();
            SetViewCancel.Invoke();
            MessageInfo = Language.Instance["GenChatIndexPageInputPrompt"];
        }

        [RelayCommand]
        private void SetReset()
        {
            SetCancel();
            SetViewClear.Invoke();
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
            Models.Add(new Model() { Type = GenConst.Chat.TypeSystem, Text = Language.Instance["GenChatHelp"] });

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
                MessageLength = $"[ {Message.Length}/{PromptMaxLength} ]";
                MessageInfo = Language.Instance["GenChatIndexPageModelInitWait"];
                await _indexService.InitAsync();
                MessageEnabled = true;
                MessageInfo = Language.Instance["GenChatIndexPageInputPrompt"];
            }
            catch (Exception ex)
            {
                MessageInfo = Language.Instance["GenChatIndexPageModelInitFailed"];
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

                MessageInfo = Language.Instance["GenChatIndexPageModelProcessWait"];

                var prompt = string.Empty;

                (prompt, _assistant) = SendAndBuildModel.Invoke(new Model() { Type = GenConst.Chat.TypeUser, Text = Message });

                Message = string.Empty;

                _cancellationTokenSource = new CancellationTokenSource();

                await _indexService.StartAsync(prompt, _cancellationTokenSource.Token);

                MessageInfo = Language.Instance["GenChatIndexPageInputPrompt"];
            }
            catch (ActivationException ex)
            {
                ServiceProvider.ShowLicense(ex.Message);
                SetViewCancel.Invoke();
                Message = string.Empty;
            }
            catch (ProcessStopException)
            {
                Message = string.Empty;
            }
            catch (Exception ex)
            {
                Message = string.Empty;
                SnackbarService.ShowSnackbarError(ex.Message);
            }
            finally
            {
                SendEnabled = true;
            }
        }
    }
}