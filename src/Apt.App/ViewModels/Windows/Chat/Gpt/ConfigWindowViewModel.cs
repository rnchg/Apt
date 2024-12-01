using Apt.App.ViewModels.Base;
using Apt.Service.Utility;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Apt.App.ViewModels.Windows.Chat.Gpt
{
    public partial class ConfigWindowViewModel : BaseViewModel
    {
        private bool _isInitialized = false;

        public Action CloseAction { get; set; } = null!;

        [ObservableProperty]
        private string _promptSystem = null!;

        [ObservableProperty]
        private int _promptMaxLength;

        [ObservableProperty]
        private int _contextMaxLength;

        [RelayCommand]
        private async Task SetSave()
        {
            Current.Config.ChatGpt.PromptSystem = PromptSystem;
            Current.Config.ChatGpt.PromptMaxLength = PromptMaxLength;
            Current.Config.ChatGpt.ContextMaxLength = ContextMaxLength;
            Apt.App.App.Current.GetRequiredService<Pages.Chat.Gpt.IndexViewModel>().PromptMaxLength = Current.Config.ChatGpt.PromptMaxLength;
            await Utility.Message.ShowMessageInfo(Language.Instance["ChatGptConfigWindowSetSaveSuccess"]);
            CloseAction?.Invoke();
        }

        [RelayCommand]
        private void SetClose()
        {
            CloseAction?.Invoke();
        }

        public ConfigWindowViewModel()
        {
            if (!_isInitialized) InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            _isInitialized = true;
        }
    }
}
