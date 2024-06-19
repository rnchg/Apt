using General.Apt.App.ViewModels.Pages.Chat.Phi3;
using General.Apt.Service.Services.Pages.Chat.Phi3;
using System.Collections.Specialized;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace General.Apt.App.Views.Pages.Chat.Phi3
{
    /// <summary>
    /// IndexPage.xaml 的交互逻辑
    /// </summary>
    public partial class IndexPage : INavigableView<IndexViewModel>
    {
        public IndexViewModel ViewModel { get; }

        public IndexPage(IndexViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            InitializeData();
        }

        public void InitializeData()
        {
            ViewModel.ChatHistory.Add(new ChatMessage() { Text = Service.Utility.Language.GetString("ChatPhi3Help"), IsOriginNative = false });

            ViewModel.ChatHistory.CollectionChanged += ChatHistory_CollectionChanged;
        }

        private void ChatHistory_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && MessagesItemsCtrl.Template != null)
            {
                var scrollViewer = (ScrollViewer)MessagesItemsCtrl.Template.FindName("scrollViewer", MessagesItemsCtrl);
                if (scrollViewer != null)
                {
                    scrollViewer.ScrollToEnd();
                }
            }
        }
    }
}
