using Wpf.Ui.Controls;

namespace General.Apt.App.Views.Pages.App
{
    public partial class SettingsPage : INavigableView<ViewModels.Pages.App.SettingsPageViewModel>
    {
        public ViewModels.Pages.App.SettingsPageViewModel ViewModel { get; }

        public SettingsPage(ViewModels.Pages.App.SettingsPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
