using General.Apt.Service.Consts;
using General.Apt.Service.Services.Pages.Chat.Phi3;
using General.Apt.Service.Utility;
using Microsoft.Extensions.Logging;
using System.Windows.Documents;
using Wpf.Ui.Controls;

namespace General.Apt.App.ViewModels.Pages.Chat.Phi3
{
    public partial class IndexViewModel : ObservableValidator, INavigationAware
    {
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

        public IndexViewModel()
        {
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
                //Lite
                Placeholder = Language.GetString("ChatPhi3IndexPageLite");
                return;

                ////Full
                //Placeholder = Language.GetString("ChatPhi3IndexPageModelInitWait");
                //_indexService = new IndexService();
                //NewEnabled = true;
                //SendEnabled = true;
                //Placeholder = Language.GetString("ChatPhi3IndexPageInputPrompt");
            }
            catch (Exception ex)
            {
                Placeholder = Language.GetString("ChatPhi3IndexPageModelInitFailed");
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Utility.Message.ShowSnackbarError(ex.Message);
                });
                Apt.App.App.Current.Logger.LogError(ex.ToString());
            }
        }

        private void Send()
        {
            NewEnabled = false;
            SendEnabled = false;
            var userMessage = new ChatMessage() { Author = ChatConst.Phi3.UserAuthor, Text = UserMessage, IsOriginNative = true };
            var chat = _indexService.ChatAsync(userMessage.Text, ChatHistory.TakeLast(8).ToList());
            ChatHistory.Add(userMessage);
            UserMessage = string.Empty;
            var aiMessage = new ChatMessage() { Author = ChatConst.Phi3.AiAuthor, Text = string.Empty, IsOriginNative = false };
            ChatHistory.Add(aiMessage);
            Task.Run(async () =>
            {
                Placeholder = Language.GetString("ChatPhi3IndexPageModelProcessWait");
                await foreach (var msg in chat) aiMessage.Text += msg;
                NewEnabled = true;
                SendEnabled = true;
                Placeholder = Language.GetString("ChatPhi3IndexPageInputPrompt");
            });
        }
    }
}