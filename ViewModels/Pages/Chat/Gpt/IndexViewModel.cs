using General.Apt.App.Models;
using General.Apt.App.Services;
using General.Apt.App.Views.Windows.Chat.Gpt;
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
        private WindowsProviderService _windowsService;
        private CancellationTokenSource _cancellationTokenSource;

        public Action<Paragraph> MessageAction { get; set; }

        [ObservableProperty]
        private string _placeholder;

        [ObservableProperty]
        private string _message;

        [ObservableProperty]
        private bool _messageEnabled = false;

        [ObservableProperty]
        private bool _sendEnabled = true;

        [ObservableProperty]
        private ObservableCollection<ChatMessage> _chatHistory;

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

        public void OnNavigatedTo()
        {
            if (!_isInitialized) InitializeViewModel();
        }

        public void OnNavigatedFrom() { }

        private void InitializeViewModel()
        {
            _windowsService = Apt.App.App.Current.GetRequiredService<WindowsProviderService>();
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