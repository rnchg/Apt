using General.Apt.App.ViewModels.Windows;

namespace General.Apt.App.Views.Windows
{
    /// <summary>
    /// LicenseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LicenseWindow
    {
        public LicenseWindowViewModel ViewModel { get; init; }

        public LicenseWindow(LicenseWindowViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            InitializeData();
        }

        public void InitializeData()
        {
            ViewModel.CloseAction = Close;
        }
    }
}
