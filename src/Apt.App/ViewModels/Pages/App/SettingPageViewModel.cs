using Apt.App.Extensions;
using Apt.App.ViewModels.Windows.App;
using Apt.Core.Consts;
using Apt.Core.Models;
using Apt.Core.Utility;
using Apt.Service.Services;
using Apt.Service.ViewModels.Base;
using Apt.Service.Views.Windows.License;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui;
using Wpf.Ui.Appearance;

namespace Apt.App.ViewModels.Pages.App
{
    public partial class SettingPageViewModel : BaseViewModel
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<string, ApplicationTheme>> _themeSource = [];

        [ObservableProperty]
        private ComBoBoxItem<string, ApplicationTheme> _themeItem = null!;

        public string Theme
        {
            get => ThemeItem.Value;
            set => ThemeItem = ThemeSource.First(e => e.Value == value);
        }

        partial void OnThemeItemChanged(ComBoBoxItem<string, ApplicationTheme> value)
        {
            if (value?.Item is null) return;
            Current.Config.Setting.Theme = Theme;
            ApplicationThemeManager.Apply(value.Item);
        }

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<string>> _languageSource = null!;

        [ObservableProperty]
        private ComBoBoxItem<string> _languageItem = null!;

        public string Language
        {
            get => LanguageItem.Value;
            set => LanguageItem = LanguageSource.FirstOrDefault(e => e.Value == (value.StartsWith("zh") ? "zh-CN" : "en-US")) ?? LanguageSource.First();
        }

        partial void OnLanguageItemChanged(ComBoBoxItem<string> value)
        {
            if (value?.Value is null) return;
            Current.Config.Setting.Language = Language;
            UpdateLanguage();
        }

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<string>> _modeSource = [];

        [ObservableProperty]
        private ComBoBoxItem<string> _modeItem = null!;

        public string Mode
        {
            get => ModeItem.Value;
            set => ModeItem = ModeSource.First(e => e.Value == value);
        }

        partial void OnModeItemChanged(ComBoBoxItem<string> value)
        {
            if (value?.Value is null) return;
            Current.Config.Setting.Mode = Mode;
        }

        [RelayCommand]
        private void SetGenChatConfig()
        {
            ServiceProvider.GetRequiredService<WindowsProviderService>().ShowDialog<Views.Windows.Gen.Chat.ConfigWindow>();
        }

        [RelayCommand]
        private void SetLicenseInfo()
        {
            ServiceProvider.GetRequiredService<WindowsProviderService>().ShowDialog<InfoWindow>();
        }

        [RelayCommand]
        private void SetLicenseOrder()
        {
            ServiceProvider.GetRequiredService<WindowsProviderService>().ShowDialog<OrderWindow>();
        }

        public SettingPageViewModel(
            IServiceProvider serviceProvider,
            ISnackbarService snackbarService) :
            base(serviceProvider, snackbarService)
        {
            if (!_isInitialized) InitializeViewModel();
        }

        public override void OnNavigatedTo()
        {
            if (!_isInitialized) InitializeViewModel();
        }

        public void InitializeViewModel()
        {
            ThemeSource =
            [
                new ComBoBoxItem<string,ApplicationTheme>() { Text = Core.Utility.Language.Instance["SettingPageThemeSystem"], Value="System",  Item = ApplicationTheme.Dark },
                new ComBoBoxItem<string,ApplicationTheme>() { Text = Core.Utility.Language.Instance["SettingPageThemeLight"], Value="Light", Item = ApplicationTheme.Light },
                new ComBoBoxItem<string,ApplicationTheme>() { Text = Core.Utility.Language.Instance["SettingPageThemeDark"], Value="Dark",  Item = ApplicationTheme.Dark },
            ];
            LanguageSource = [.. Directory.GetFiles(AppConst.LanguagePath, "*.json").Select(x =>
            {
                var name = Path.GetFileNameWithoutExtension(x);
                return new ComBoBoxItem<string>() { Text = name, Value = name };
            })];
            ModeSource =
            [
                new ComBoBoxItem<string>() { Text = Core.Utility.Language.Instance["SettingPageModeBalanced"], Value ="Balanced" },
                new ComBoBoxItem<string>() { Text = Core.Utility.Language.Instance["SettingPageModePerformance"], Value = "Performance" }
            ];

            Theme = Current.Config.Setting.Theme;
            Mode = Current.Config.Setting.Mode;
            Language = Current.Config.Setting.Language;

            _isInitialized = true;
        }

        private void UpdateLanguage()
        {
            if (Core.Utility.Language.Instance.Name == Current.Config.Setting.Language)
            {
                return;
            }
            Core.Utility.Language.Instance.Update(Current.Config.Setting.Language);
            ServiceProvider.GetRequiredService<MainWindowViewModel>().InitializeViewModel();
            ServiceProvider.GetRequiredService<SettingPageViewModel>().InitializeViewModel();
            ServiceProvider.GetRequiredService<Image.SuperResolution.IndexPageViewModel>().InitializeViewModel();
            ServiceProvider.GetRequiredService<Image.AutoWipe.IndexPageViewModel>().InitializeViewModel();
            ServiceProvider.GetRequiredService<Image.CartoonComic.IndexPageViewModel>().InitializeViewModel();
            ServiceProvider.GetRequiredService<Image.Convert3d.IndexPageViewModel>().InitializeViewModel();
            ServiceProvider.GetRequiredService<Image.ColorRestoration.IndexPageViewModel>().InitializeViewModel();
            ServiceProvider.GetRequiredService<Image.FrameInterpolation.IndexPageViewModel>().InitializeViewModel();
            ServiceProvider.GetRequiredService<Image.Matting.IndexPageViewModel>().InitializeViewModel();
            ServiceProvider.GetRequiredService<Image.FaceRestoration.IndexPageViewModel>().InitializeViewModel();
            ServiceProvider.GetRequiredService<Video.SuperResolution.IndexPageViewModel>().InitializeViewModel();
            ServiceProvider.GetRequiredService<Video.AutoWipe.IndexPageViewModel>().InitializeViewModel();
            ServiceProvider.GetRequiredService<Video.CartoonComic.IndexPageViewModel>().InitializeViewModel();
            ServiceProvider.GetRequiredService<Video.Convert3d.IndexPageViewModel>().InitializeViewModel();
            ServiceProvider.GetRequiredService<Video.ColorRestoration.IndexPageViewModel>().InitializeViewModel();
            ServiceProvider.GetRequiredService<Video.FrameInterpolation.IndexPageViewModel>().InitializeViewModel();
            ServiceProvider.GetRequiredService<Video.Matting.IndexPageViewModel>().InitializeViewModel();
            ServiceProvider.GetRequiredService<Video.Organization.IndexPageViewModel>().InitializeViewModel();
            ServiceProvider.GetRequiredService<Audio.VocalSplit.IndexPageViewModel>().InitializeViewModel();
            ServiceProvider.GetConfig();
        }
    }
}
