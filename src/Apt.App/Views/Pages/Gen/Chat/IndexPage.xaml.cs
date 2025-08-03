using APT.App.ViewModels.Pages.Gen.Chat;
using Wpf.Ui.Abstractions.Controls;

namespace APT.App.Views.Pages.Gen.Chat
{
    public partial class IndexPage : INavigableView<IndexPageViewModel>
    {
        public IndexPageViewModel ViewModel { get; }

        public IndexPage(IndexPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            ViewModel.SendAndBuildModel = ChatView.SendAndBuildChat;

            ViewModel.CancelMessage = ChatView.CancelChat;

            ViewModel.ResetMessage = ChatView.ResetChat;

            ViewModel.ReceiveMessage = ChatView.ReceiveChat;

            Prompt.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        ViewModel.SetSendCommand.Execute(null);
                    }
                }
            };
        }
    }
}