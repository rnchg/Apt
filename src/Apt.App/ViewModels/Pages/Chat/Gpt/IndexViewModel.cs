using Apt.App.Models;
using Apt.App.Services;
using Apt.App.ViewModels.Base;
using Apt.App.Views.Windows.Chat.Gpt;
using Apt.Service.Consts;
using Apt.Service.Services.Pages.Chat.Gpt;
using Apt.Service.Utility;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace Apt.App.ViewModels.Pages.Chat.Gpt
{
    public partial class IndexViewModel : BaseViewModel
    {
        private readonly AppSettings _appSettings;

        private bool _isInitialized = false;

        private IndexService _indexService = null!;
        private WindowsProviderService _windowsService = null!;
        private CancellationTokenSource _cancellationTokenSource = null!;

        public Action<Paragraph> MessageAction { get; set; } = null!;

        [ObservableProperty]
        private int _promptMaxLength = Current.Config.ChatGpt.PromptMaxLength;
        partial void OnPromptMaxLengthChanged(int value)
        {
            MessageHeader = $"{Language.Instance["ChatGptIndexPageMessage"]} [ {Message.Length}/{value} ]";
        }

        [ObservableProperty]
        private string _messageHeader = null!;

        [ObservableProperty]
        private string _placeholder = null!;

        [ObservableProperty]
        private string _message = string.Empty;

        partial void OnMessageChanged(string value)
        {
            MessageHeader = $"{Language.Instance["ChatGptIndexPageMessage"]} [ {value.Length}/{PromptMaxLength} ]";
        }

        [ObservableProperty]
        private bool _messageEnabled = false;

        [ObservableProperty]
        private bool _sendEnabled = true;

        [ObservableProperty]
        private ObservableCollection<ChatMessage> _chatHistory = [];

        [RelayCommand]
        private void SetSend()
        {
            Send();
        }

        [RelayCommand]
        private void SetCancel()
        {
            _cancellationTokenSource?.Cancel();
            Message = string.Empty;
        }

        [RelayCommand]
        private void SetReset()
        {
            ChatHistory.Clear();
            SetCancel();
        }

        [RelayCommand]
        private void SetConfig()
        {
            _windowsService.ShowDialog<ConfigWindow>();
        }

        public IndexViewModel(AppSettings appSettings)
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
            _windowsService = Apt.App.App.Current.GetRequiredService<WindowsProviderService>();

            Task.Run(InitModel);

            _isInitialized = true;
        }

        private void InitModel()
        {
            try
            {
                if (_appSettings.App.Pack == "Full")
                {
                    Placeholder = Language.Instance["ChatGptIndexPageModelInitWait"];
                    _indexService = new IndexService();
                    MessageEnabled = true;
                    Placeholder = Language.Instance["ChatGptIndexPageInputPrompt"];
                }
                else
                {
                    Placeholder = Language.Instance["ChatGptIndexPageLite"];
                }
            }
            catch (Exception ex)
            {
                Placeholder = Language.Instance["ChatGptIndexPageModelInitFailed"];
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Utility.Message.ShowSnackbarError(ex.Message);
                });
                Apt.App.App.Current.Logger.LogError(ex.ToString());
            }
        }

        private void Send()
        {
            if (string.IsNullOrWhiteSpace(Message))
            {
                return;
            }
            Placeholder = Language.Instance["ChatGptIndexPageModelProcessWait"];
            SendEnabled = false;
            var messageUser = new ChatMessage() { Type = ChatConst.Gpt.TypeUser, Text = Message, IsOwner = true };
            ChatHistory.Add(messageUser);
            var messageAssistant = new ChatMessage() { Type = ChatConst.Gpt.TypeAssistant, Text = string.Empty, IsOwner = false };
            ChatHistory.Add(messageAssistant);
            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => RunModel(ChatHistory, messageUser, messageAssistant, _cancellationTokenSource.Token), _cancellationTokenSource.Token);
            Message = string.Empty;
        }

        private void RunModel(IList<ChatMessage> chatHistory, ChatMessage messageUser, ChatMessage messageAssistant, CancellationToken cancellationToken)
        {
            try
            {
                Placeholder = Language.Instance["ChatGptIndexPageModelProcessWait"];
                _indexService.Start(chatHistory.ToArray(), cancellationToken);
                Placeholder = Language.Instance["ChatGptIndexPageInputPrompt"];
            }
            catch (OperationCanceledException ex)
            {
                Placeholder = Language.Instance["ChatGptIndexPageInputPrompt"];
                Application.Current.Dispatcher.Invoke(() =>
                {
                    chatHistory.Remove(messageAssistant);
                    chatHistory.Remove(messageUser);
                });
                Apt.App.App.Current.Logger.LogError(ex.ToString());
            }
            catch (Exception ex)
            {
                Placeholder = Language.Instance["ChatGptIndexPageModelInitFailed"];
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Utility.Message.ShowSnackbarError(ex.Message);
                });
                Apt.App.App.Current.Logger.LogError(ex.ToString());
            }
            finally
            {
                SendEnabled = true;
            }
        }
    }
}