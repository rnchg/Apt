using Apt.App.ViewModels.Windows.Gen.Chat;
using Apt.Core.Utility;

namespace Apt.App.Views.Windows.Gen.Chat
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
                ViewModel.PromptSystem = Current.Config.GenChat.PromptSystem;
                ViewModel.PromptMaxLength = Current.Config.GenChat.PromptMaxLength;
                ViewModel.ContextMaxLength = Current.Config.GenChat.ContextMaxLength;
            };
        }
    }
}
