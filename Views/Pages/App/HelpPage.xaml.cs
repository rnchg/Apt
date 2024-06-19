using Wpf.Ui.Controls;

namespace General.Apt.App.Views.Pages.App
{
    /// <summary>
    /// HelpPage.xaml 的交互逻辑
    /// </summary>
    public partial class HelpPage : INavigableView<ViewModels.Pages.App.HelpPageViewModel>
    {
        public ViewModels.Pages.App.HelpPageViewModel ViewModel { get; }

        public HelpPage(ViewModels.Pages.App.HelpPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            InitializeData();
        }

        public void InitializeData()
        {

        }
    }
}
