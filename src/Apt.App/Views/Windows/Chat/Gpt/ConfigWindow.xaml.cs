using Apt.App.ViewModels.Windows.Chat.Gpt;
using Apt.Service.Utility;

namespace Apt.App.Views.Windows.Chat.Gpt
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
                ViewModel.PromptSystem = Current.Config.ChatGpt.PromptSystem;
                ViewModel.PromptMaxLength = Current.Config.ChatGpt.PromptMaxLength;
                ViewModel.ContextMaxLength = Current.Config.ChatGpt.ContextMaxLength;
            };
        }
    }
}
