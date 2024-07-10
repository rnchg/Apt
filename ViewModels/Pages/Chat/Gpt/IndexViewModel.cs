using General.Apt.App.Models;
using General.Apt.Service.Consts;
using General.Apt.Service.Services.Pages.Chat.Gpt;
using General.Apt.Service.Utility;
using Microsoft.Extensions.Logging;
using System.Windows.Documents;
using Wpf.Ui.Controls;

namespace General.Apt.App.ViewModels.Pages.Chat.Gpt
{
    public partial class IndexViewModel : ObservableValidator, INavigationAware
    {
        private readonly AppSettings _appSettings;

        private bool _isInitialized = false;
        private IndexService _indexService;
        private CancellationTokenSource _cancellationTokenSource;

        public Action<Paragraph> MessageAction { get; set; }

        [ObservableProperty]
        private string _placeholder;

        [ObservableProperty]
        private string _message;

        [ObservableProperty]
        private int _maxLength;

        [ObservableProperty]
        private bool _newEnabled;

        [ObservableProperty]
        private bool _sendEnabled;

        [ObservableProperty]
        private ObservableCollection<ChatMessage> _chatHistory;

        [RelayCommand]
        private void SetNew() => ChatHistory.Clear();

        [RelayCommand]
        private void SetSend() => Send();

        [RelayCommand]
        private void SetCancel() => _cancellationTokenSource?.Cancel();

        public IndexViewModel(AppSettings appSettings)
        {
            _appSettings = appSettings;
            MaxLength = _appSettings.Chat.Gpt.PromptMaxLength;
            if (!_isInitialized) InitializeViewModel();
        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized) InitializeViewModel();
        }

        public void OnNavigatedFrom() { }

        private void InitializeViewModel()
        {
            ChatHistory = new ObservableCollection<ChatMessage>();

            Task.Run(InitModel);

            _isInitialized = true;
        }

        private void InitModel()
        {
            try
            {
                if (_appSettings.App.Pack == "Full")
                {
                    Placeholder = Language.GetString("ChatGptIndexPageModelInitWait");
                    _indexService = new IndexService();
                    NewEnabled = true;
                    SendEnabled = true;
                    Placeholder = Language.GetString("ChatGptIndexPageInputPrompt");
                }
                else
                {
                    Placeholder = Language.GetString("ChatGptIndexPageLite");
                }
            }
            catch (Exception ex)
            {
                Placeholder = Language.GetString("ChatGptIndexPageModelInitFailed");
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
            Placeholder = Language.GetString("ChatGptIndexPageModelProcessWait");
            NewEnabled = false;
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
                Placeholder = Language.GetString("ChatGptIndexPageModelProcessWait");
                _indexService.Start(chatHistory.ToArray(), _appSettings.Chat.Gpt.PromptSystem, _appSettings.Chat.Gpt.PromptMaxLength, cancellationToken);
                Placeholder = Language.GetString("ChatGptIndexPageInputPrompt");
            }
            catch (OperationCanceledException ex)
            {
                Placeholder = Language.GetString("ChatGptIndexPageInputPrompt");
                Application.Current.Dispatcher.Invoke(() =>
                {
                    chatHistory.Remove(messageAssistant);
                    chatHistory.Remove(messageUser);
                });
                Apt.App.App.Current.Logger.LogError(ex.ToString());
            }
            catch (Exception ex)
            {
                Placeholder = Language.GetString("ChatGptIndexPageModelInitFailed");
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Utility.Message.ShowSnackbarError(ex.Message);
                });
                Apt.App.App.Current.Logger.LogError(ex.ToString());
            }
            finally
            {
                NewEnabled = true;
                SendEnabled = true;
            }
        }
    }
}