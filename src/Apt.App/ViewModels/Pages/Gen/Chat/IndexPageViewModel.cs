using APT.App.Models;
using APT.App.Views.Windows.Gen.Chat;
using APT.Core.Consts;
using APT.Core.Exceptions;
using APT.Core.Services.Pages.Gen.Chat;
using APT.Core.Utility;
using APT.Service.Adapters.Windows;
using APT.Service.Controls.ChatView;
using APT.Service.Extensions;
using APT.Service.Services;
using APT.Service.ViewModels.Base;
using Common.WindowsDesktop.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Services.Pages.Gen.Chat;
using Wpf.Ui;

namespace APT.App.ViewModels.Pages.Gen.Chat
{
    public partial class IndexPageViewModel : BaseViewModel
    {
        private readonly AppSettings _appSettings;

        private IndexService _indexService = null!;

        private bool _isInitialized = false;

        private bool _isInit = false;

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<string>> _providerSource = [];

        [ObservableProperty]
        private ComBoBoxItem<string> _providerItem = null!;

        public string Provider
        {
            get => ProviderItem.Value;
            set => ProviderItem = ProviderSource.FirstOrDefault(e => e.Value == value) ?? ProviderSource.First();
        }

        [ObservableProperty]
        private bool _think = false;

        [ObservableProperty]
        private Func<string, bool, (List<ChatModel>, Model?, Model?)> _sendAndBuildModel = null!;

        [ObservableProperty]
        private Action<string, bool, Model?, Model?> _receiveMessage = null!;

        [ObservableProperty]
        private Action _cancelMessage = null!;

        [ObservableProperty]
        private Action _resetMessage = null!;

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
        private ObservableCollection<Model> _chatList = [];

        [RelayCommand]
        private void SetSend() => _ = Send();

        [RelayCommand]
        private void SetCancel()
        {
            _cancellationTokenSource?.Cancel();
            CancelMessage.Invoke();
            MessageInfo = Language.Instance["Gen.Chat.InputPrompt"];
        }

        [RelayCommand]
        private void SetReset()
        {
            SetCancel();
            ResetMessage.Invoke();
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
            if (!_isInit) _ = Init();
        }

        private void InitializeViewModel()
        {
            ProviderSource = Adapter.CpuAndDml;

            ChatList.Add(new Model() { Type = GenConst.Chat.TypeSystem, Text = Language.Instance["Gen.Chat.Help"] });

            _indexService = new IndexService();

            _isInitialized = true;
        }

        private async Task Init()
        {
            try
            {
                MessageLength = $"[ {Message.Length}/{PromptMaxLength} ]";
                MessageInfo = Language.Instance["Gen.Chat.ModelInitWait"];
                await _indexService.InitAsync();
                MessageEnabled = true;
                MessageInfo = Language.Instance["Gen.Chat.InputPrompt"];
                _isInit = true;
            }
            catch (Exception ex)
            {
                MessageInfo = Language.Instance["Gen.Chat.ModelInitFailed"];
                SnackbarService.ShowSnackbarError(ex.Message);
                _isInit = false;
            }
        }

        private async Task Send()
        {
            try
            {
                if (!SendEnabled) return;

                if (string.IsNullOrWhiteSpace(Message)) return;

                SendEnabled = false;

                MessageInfo = Language.Instance["Gen.Chat.ModelProcessWait"];

                var (prompts, _thinkMessage, _assistantMessage) = SendAndBuildModel.Invoke(Message, Think);

                Message = string.Empty;

                _cancellationTokenSource = new CancellationTokenSource();

                _indexService.ReceiveMessage = (e) => ReceiveMessage(e, Think, _thinkMessage, _assistantMessage);

                await _indexService.StartAsync(prompts, Think, Provider, _cancellationTokenSource.Token);

                MessageInfo = Language.Instance["Gen.Chat.InputPrompt"];
            }
            catch (ActivationException ex)
            {
                ServiceProvider.ShowLicense(ex.Message);
                CancelMessage.Invoke();
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