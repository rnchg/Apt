﻿using General.Apt.Service.Utility;

namespace General.Apt.App.ViewModels.Windows.Chat.Gpt
{
    public partial class ConfigWindowViewModel : ObservableObject
    {
        private bool _isInitialized = false;

        public Action CloseAction { get; set; }

        [ObservableProperty]
        private string _promptSystem;

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

            await Utility.Message.ShowMessageInfo(string.Format(Language.Instance["ChatGptConfigWindowSetSaveSuccess"]));

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
