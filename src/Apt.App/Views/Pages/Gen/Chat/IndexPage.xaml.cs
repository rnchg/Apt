using Apt.App.ViewModels.Pages.Gen.Chat;
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

            ViewModel.SendAndBuildModel = ChatView.SendAndBuildModel;

            ViewModel.CancelMessage = ChatView.CancelMessage;

            ViewModel.ResetMessage = ChatView.ResetMessage;

            ViewModel.ReceiveMessage = ChatView.ReceiveMessage;

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