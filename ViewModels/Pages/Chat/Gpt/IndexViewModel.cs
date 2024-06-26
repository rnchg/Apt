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

        public Action<Paragraph> MessageAction { get; set; }

        [ObservableProperty]
        private string _placeholder;

        [ObservableProperty]
        private string _userMessage;

        [ObservableProperty]
        private string _aIMessage;

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

        public IndexViewModel(AppSettings appSettings)
        {
            _appSettings = appSettings;
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
            Placeholder = Language.GetString("ChatGptIndexPageModelProcessWait");
            NewEnabled = false;
            SendEnabled = false;
            var userMessage = new ChatMessage() { Author = ChatConst.Gpt.UserAuthor, Text = UserMessage, IsOriginNative = true };
            ChatHistory.Add(userMessage);
            var aiMessage = new ChatMessage() { Author = ChatConst.Gpt.AiAuthor, Text = string.Empty, IsOriginNative = false };
            ChatHistory.Add(aiMessage);
            Task.Run(() => RunModel(ChatHistory));
            UserMessage = string.Empty;
        }

        private void RunModel(IList<ChatMessage> chatHistory)
        {
            try
            {
                Placeholder = Language.GetString("ChatGptIndexPageModelProcessWait");
                _indexService.Start(chatHistory.ToArray());
                NewEnabled = true;
                SendEnabled = true;
                Placeholder = Language.GetString("ChatGptIndexPageInputPrompt");
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
    }
}