using Apt.App.ViewModels.Windows.App;
using Apt.Service.Utility;

namespace Apt.App.Views.Windows.App
{
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

            IsVisibleChanged += (s, e) =>
            {
                if (!IsVisible) return;
                ViewModel.RequestCode = Current.Config.App.RequestCode;
                ViewModel.ActivationCode = Current.Config.App.ActivationCode;
                ViewModel.ValidateActivationCode();
            };
        }
    }
}
