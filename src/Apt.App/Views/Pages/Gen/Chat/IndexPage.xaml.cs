using Apt.App.ViewModels.Pages.Gen.Chat;
using System.Collections.Specialized;
using System.Windows.Controls;
using Wpf.Ui.Abstractions.Controls;

namespace Apt.App.Views.Pages.Gen.Chat
{
    public partial class IndexPage : INavigableView<IndexPageViewModel>
    {
        public IndexPageViewModel ViewModel { get; }

        public IndexPage(IndexPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            InitializeData();
        }

        public void InitializeData()
        {
            ViewModel.ChatHistory.Add(new ChatMessage() { Text = Core.Utility.Language.Instance["GenChatHelp"], IsOwner = false });

            ViewModel.ChatHistory.CollectionChanged += ChatHistory_CollectionChanged;
        }

        private void ChatHistory_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && MessagesItemsCtrl.Template is not null)
            {
                var scrollViewer = (ScrollViewer)MessagesItemsCtrl.Template.FindName("scrollViewer", MessagesItemsCtrl);
                scrollViewer?.ScrollToEnd();
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    ViewModel.SetSendCommand.Execute(null);
                }
            }
        }
    }
}