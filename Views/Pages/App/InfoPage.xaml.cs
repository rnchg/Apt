using Wpf.Ui.Controls;

namespace General.Apt.App.Views.Pages.App
{
    /// <summary>
    /// InfoPage.xaml 的交互逻辑
    /// </summary>
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
