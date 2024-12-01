using Wpf.Ui.Abstractions.Controls;

namespace Apt.App.Views.Pages.App
{
    public partial class InfoPage : INavigableView<ViewModels.Pages.App.InfoPageViewModel>
    {
        public ViewModels.Pages.App.InfoPageViewModel ViewModel { get; }

        public InfoPage(ViewModels.Pages.App.InfoPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
