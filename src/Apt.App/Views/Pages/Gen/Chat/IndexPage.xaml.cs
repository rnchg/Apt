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

            ViewModel.AddChat += ChatFrame.AddChat;

            ViewModel.SendChat += ChatFrame.SendChat;

            ViewModel.SendAndBuildChat += ChatFrame.SendAndBuildChat;

            ViewModel.CancelChat += ChatFrame.CancelChat;

            ViewModel.ClearChat += ChatFrame.ClearChat;

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