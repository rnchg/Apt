using Apt.App.ViewModels.Windows.Gen.Phi;
using Apt.Core.Utility;

namespace Apt.App.Views.Windows.Gen.Phi
{
    public partial class ConfigWindow
    {
        public ConfigWindowViewModel ViewModel { get; init; }

        public ConfigWindow(ConfigWindowViewModel viewModel)
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
                ViewModel.PromptSystem = Current.Config.GenPhi.PromptSystem;
                ViewModel.PromptMaxLength = Current.Config.GenPhi.PromptMaxLength;
                ViewModel.ContextMaxLength = Current.Config.GenPhi.ContextMaxLength;
            };
        }
    }
}
