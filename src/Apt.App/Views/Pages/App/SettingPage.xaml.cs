using Wpf.Ui.Abstractions.Controls;

namespace Apt.App.Views.Pages.App
{
    public partial class SettingPage : INavigableView<ViewModels.Pages.App.SettingPageViewModel>
    {
        public ViewModels.Pages.App.SettingPageViewModel ViewModel { get; }

        public SettingPage(ViewModels.Pages.App.SettingPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
