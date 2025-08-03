using Wpf.Ui.Abstractions.Controls;

namespace APT.App.Views.Pages.App
{
    public partial class DashboardPage : INavigableView<ViewModels.Pages.App.DashboardPageViewModel>
    {
        public ViewModels.Pages.App.DashboardPageViewModel ViewModel { get; }

        public DashboardPage(ViewModels.Pages.App.DashboardPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
