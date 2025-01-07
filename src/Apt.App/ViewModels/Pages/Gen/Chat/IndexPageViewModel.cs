using Apt.App.Models;
using Apt.App.Views.Windows.Gen.Chat;
using Apt.Core.Consts;
using Apt.Core.Exceptions;
using Apt.Core.Services.Pages.Gen.Chat;
using Apt.Core.Utility;
using Apt.Service.Controls.ChatFrame;
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

        [ObservableProperty]
        private Func<ChatModel, bool> _addChat = null!;

        [ObservableProperty]
        private Func<ChatModel, bool> _sendChat = null!;

        [ObservableProperty]
        private Func<ChatModel, (string, ChatModel)> _sendAndBuildChat = null!;

        [ObservableProperty]
        private Action _cancelChat = null!;

        [ObservableProperty]
        private Action _clearChat = null!;

        private CancellationTokenSource _cancellationTokenSource = null!;

        [ObservableProperty]
        private int _promptMaxLength = Current.Config.GenChat.PromptMaxLength;
        partial void OnPromptMaxLengthChanged(int value)
        {
            MessageHeader = $"{Language.Instance["GenChatIndexPageMessage"]} [ {Message.Length}/{value} ]";
        }

        [ObservableProperty]
        private string _messageHeader = null!;

        [ObservableProperty]
        private string _placeholder = null!;

        [ObservableProperty]
        private string _message = string.Empty;

        partial void OnMessageChanged(string value)
        {
            MessageHeader = $"{Language.Instance["GenChatIndexPageMessage"]} [ {value.Length}/{PromptMaxLength} ]";
        }

        [ObservableProperty]
        private bool _messageEnabled = false;

        [ObservableProperty]
        private bool _sendEnabled = true;

        [ObservableProperty]
        private ObservableCollection<ChatModel> _chatList = [];

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
            ClearChat.Invoke();
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
            ChatList.Add(new ChatModel() { Text = Language.Instance["GenChatHelp"], IsOwner = false });

            _ = Start();

            _isInitialized = true;
        }

        private async Task Start()
        {
            try
            {
                if (_appSettings.App.Pack == "Full")
                {
                    Placeholder = Language.Instance["GenChatIndexPageModelInitWait"];
                    await Task.Run(() => _indexService = new IndexService());
                    MessageEnabled = true;
                    Placeholder = Language.Instance["GenChatIndexPageInputPrompt"];
                }
                else
                {
                    Placeholder = Language.Instance["GenChatIndexPageLite"];
                }
            }
            catch (Exception ex)
            {
                Placeholder = Language.Instance["GenChatIndexPageModelInitFailed"];
                SnackbarService.ShowSnackbarError(ex.Message);
            }
        }

        private async Task Send()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Message)) return;

                SendEnabled = false;

                Placeholder = Language.Instance["GenChatIndexPageModelProcessWait"];

                var (prompt, assistant) = SendAndBuildChat.Invoke(new ChatModel() { Type = GenConst.Chat.TypeUser, Text = Message, IsOwner = true });

                Message = string.Empty;

                _cancellationTokenSource = new CancellationTokenSource();

                var message = _indexService.Start(prompt, _cancellationTokenSource.Token);

                await Task.Run(async () => { await foreach (var m in message) assistant.Text += m; });

            }
            catch (ActivationException ex)
            {
                CancelChat.Invoke();
                ServiceProvider.ShowLicense(ex.Message);
            }
            catch (OperationCanceledException)
            {
                CancelChat.Invoke();
            }
            catch (Exception ex)
            {
                CancelChat.Invoke();
                SnackbarService.ShowSnackbarError(ex.Message);
            }
            finally
            {
                SendEnabled = true;
                Placeholder = Language.Instance["GenChatIndexPageInputPrompt"];
            }
        }
    }
}