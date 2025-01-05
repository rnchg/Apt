using Apt.App.Models;
using Apt.App.Views.Windows.Gen.Chat;
using Apt.Core.Consts;
using Apt.Core.Exceptions;
using Apt.Core.Services.Pages.Gen.Chat;
using Apt.Core.Utility;
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

        private WindowsProviderService _windowsService = null!;
        private CancellationTokenSource _cancellationTokenSource = null!;

        public Action<Paragraph> MessageAction { get; set; } = null!;

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
        private ObservableCollection<ChatMessage> _chatHistory = [];

        [ObservableProperty]
        private ChatMessage _chatUser = null!;

        [ObservableProperty]
        private ChatMessage _chatAssistant = null!;

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
            ChatHistory.Clear();
        }

        [RelayCommand]
        private void SetConfig()
        {
            _windowsService.ShowDialog<ConfigWindow>();
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
            _windowsService = ServiceProvider.GetRequiredService<WindowsProviderService>();

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

                _cancellationTokenSource = new CancellationTokenSource();

                var (prompt, assistant) = Build();

                var message = _indexService.Start(prompt, _cancellationTokenSource.Token);

                await Task.Run(async () => { await foreach (var m in message) assistant.Text += m; });

            }
            catch (ActivationException ex)
            {
                ChatHistory.Remove(ChatAssistant);
                ChatHistory.Remove(ChatUser);
                ServiceProvider.ShowLicense(ex.Message);
            }
            catch (OperationCanceledException)
            {
                ChatHistory.Remove(ChatAssistant);
                ChatHistory.Remove(ChatUser);
            }
            catch (Exception ex)
            {
                ChatHistory.Remove(ChatAssistant);
                ChatHistory.Remove(ChatUser);
                SnackbarService.ShowSnackbarError(ex.Message);
            }
            finally
            {
                SendEnabled = true;
                Placeholder = Language.Instance["GenChatIndexPageInputPrompt"];
            }
        }

        private (string, ChatMessage) Build()
        {
            ChatUser = new ChatMessage() { Type = GenConst.Chat.TypeUser, Text = Message, IsOwner = true };

            ChatHistory.Add(ChatUser);

            ChatAssistant = new ChatMessage() { Type = GenConst.Chat.TypeAssistant, Text = string.Empty, IsOwner = false };

            ChatHistory.Add(ChatAssistant);

            Message = string.Empty;

            var promptHistory = ChatHistory.Where(e => e.Type == GenConst.Chat.TypeUser || e.Type == GenConst.Chat.TypeAssistant).Reverse().ToArray();

            var promptAssistant = promptHistory[0];

            if (promptAssistant.Type != GenConst.Chat.TypeAssistant)
            {
                throw new Exception(Language.Instance["GenChatIndexPageChatHistoryLast1NotAi"]);
            }

            var promptUser = promptHistory[1];

            if (promptUser.Type != GenConst.Chat.TypeUser)
            {
                throw new Exception(Language.Instance["GenChatIndexPageChatHistoryLast2NotUser"]);
            }
            if (string.IsNullOrEmpty(promptUser.Text))
            {
                throw new Exception(Language.Instance["GenChatIndexPageChatHistoryUserEmpty"]);
            }

            var promptStart = string.IsNullOrWhiteSpace(Current.Config?.GenChat.PromptSystem) ? string.Empty : $"<|system|>{Current.Config.GenChat.PromptSystem}<|end|>";

            var promptContent = string.Empty;

            var promptEnd = "<|assistant|>";

            for (var i = 1; i < promptHistory.Length; i++)
            {
                var prompt = promptHistory[i];

                if (string.IsNullOrWhiteSpace(prompt.Text))
                {
                    continue;
                }

                var promptChat = $"<|{prompt.Type.ToLower()}|>{prompt.Text}<|end|>{promptContent}";

                if (promptStart.Length + promptChat.Length + promptEnd.Length > Current.Config?.GenChat.PromptMaxLength)
                {
                    break;
                }

                if (i > Current.Config?.GenChat.ContextMaxLength)
                {
                    break;
                }

                promptContent = promptChat;
            }

            var promptFull = $"{promptStart}{promptContent}{promptEnd}";

            return (promptFull, promptAssistant);
        }
    }
}