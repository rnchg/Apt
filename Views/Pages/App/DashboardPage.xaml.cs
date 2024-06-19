using Wpf.Ui.Controls;

namespace General.Apt.App.Views.Pages.App
{
    /// <summary>
    /// DashboardPage.xaml 的交互逻辑
    /// </summary>
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
