using Apt.App.ViewModels.Pages.Gen.Phi;
using Wpf.Ui.Abstractions.Controls;

namespace Apt.App.Views.Pages.Gen.Phi
{
    public partial class IndexPage : INavigableView<IndexPageViewModel>
    {
        public IndexPageViewModel ViewModel { get; }

        public IndexPage(IndexPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            ViewModel.AddModel += GenView.AddModel;

            ViewModel.SendModel += GenView.SendModel;

            ViewModel.SendAndBuildModel += GenView.SendAndBuildModel;

            ViewModel.SetGenViewCancel += GenView.SetCancel;

            ViewModel.SetGenViewClear += GenView.SetClear;

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