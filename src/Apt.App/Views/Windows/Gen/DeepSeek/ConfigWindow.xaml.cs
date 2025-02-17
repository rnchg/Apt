using Apt.App.ViewModels.Windows.Gen.DeepSeek;
using Apt.Core.Utility;

namespace Apt.App.Views.Windows.Gen.DeepSeek
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
                ViewModel.PromptSystem = Current.Config.GenDeepSeek.PromptSystem;
                ViewModel.PromptMaxLength = Current.Config.GenDeepSeek.PromptMaxLength;
                ViewModel.ContextMaxLength = Current.Config.GenDeepSeek.ContextMaxLength;
            };
        }
    }
}
