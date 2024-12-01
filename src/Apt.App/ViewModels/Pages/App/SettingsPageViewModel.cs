using Apt.App.Services;
using Apt.App.ViewModels.Base;
using Apt.App.ViewModels.Windows.App;
using Apt.App.Views.Windows.App;
using Apt.App.Views.Windows.Chat.Gpt;
using Apt.Service.Consts;
using Apt.Service.Models;
using Apt.Service.Utility;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;

namespace Apt.App.ViewModels.Pages.App
{
    public partial class SettingsPageViewModel : BaseViewModel
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
            Current.Config.App.Theme = Theme;
            ApplicationThemeManager.Apply(value.Item);
        }

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<string, NavigationViewPaneDisplayMode>> _navigationStyleSource = [];

        [ObservableProperty]
        private ComBoBoxItem<string, NavigationViewPaneDisplayMode> _navigationStyleItem = null!;

        public string NavigationStyle
        {
            get => NavigationStyleItem.Value;
            set => NavigationStyleItem = NavigationStyleSource.First(e => e.Value == value);
        }

        partial void OnNavigationStyleItemChanged(ComBoBoxItem<string, NavigationViewPaneDisplayMode> value)
        {
            if (value?.Item is null) return;
            Current.Config.App.NavigationStyle = NavigationStyle;
            Apt.App.App.Current.GetRequiredService<INavigationService>().SetPaneDisplayMode(value.Item);
        }

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<string, string>> _languageSource = new(Directory.GetFiles(AppConst.LanguagePath, "*.json").Select(x =>
        {
            var name = Path.GetFileNameWithoutExtension(x);
            return new ComBoBoxItem<string, string>() { Text = name, Value = name, Item = name };
        }));

        [ObservableProperty]
        private ComBoBoxItem<string, string> _languageItem = null!;

        public string Language
        {
            get => LanguageItem.Value.ToString();
            set => LanguageItem = LanguageSource.First(e => e.Value.ToString() == value);
        }

        partial void OnLanguageItemChanged(ComBoBoxItem<string, string> value)
        {
            if (value?.Item is null) return;
            Current.Config.App.CurrentLanguage = Language;
            UpdateLanguage();
        }

        public bool IsAutoOpenOutput
        {
            get => Current.Config.App.IsAutoOpenOutput;
            set => Current.Config.App.IsAutoOpenOutput = value;
        }

        [RelayCommand]
        private void SetChatGptConfig()
        {
            Apt.App.App.Current.GetRequiredService<WindowsProviderService>().ShowDialog<ConfigWindow>();
        }

        [RelayCommand]
        private void SetLicense()
        {
            Apt.App.App.Current.GetRequiredService<WindowsProviderService>().ShowDialog<LicenseWindow>();
        }

        public SettingsPageViewModel()
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
                new ComBoBoxItem<string,ApplicationTheme>() { Text = Service.Utility.Language.Instance["SettingsPageThemeDark"], Value="Dark",  Item = ApplicationTheme.Dark },
                new ComBoBoxItem<string,ApplicationTheme>() { Text = Service.Utility.Language.Instance["SettingsPageThemeLight"], Value="Light", Item = ApplicationTheme.Light },
                new ComBoBoxItem<string,ApplicationTheme>() { Text = Service.Utility.Language.Instance["SettingsPageThemeHighContrast"], Value="HighContrast",  Item = ApplicationTheme.HighContrast }
            ];
            NavigationStyleSource =
            [
                new ComBoBoxItem<string, NavigationViewPaneDisplayMode>() { Text = Service.Utility.Language.Instance["SettingsPageNavigationLeft"], Value ="Left", Item = NavigationViewPaneDisplayMode.Left },
                new ComBoBoxItem<string, NavigationViewPaneDisplayMode>() { Text = Service.Utility.Language.Instance["SettingsPageNavigationLeftMinimal"], Value = "LeftMinimal",  Item = NavigationViewPaneDisplayMode.LeftMinimal },
                new ComBoBoxItem<string, NavigationViewPaneDisplayMode>() { Text = Service.Utility.Language.Instance["SettingsPageNavigationLeftFluent"], Value = "LeftFluent",  Item = NavigationViewPaneDisplayMode.LeftFluent }
            ];

            Theme = Current.Config.App.Theme;
            NavigationStyle = Current.Config.App.NavigationStyle;
            IsAutoOpenOutput = Current.Config.App.IsAutoOpenOutput;

            _isInitialized = true;
        }

        private void UpdateLanguage()
        {
            if (Current.Config.App.CurrentLanguage == Service.Utility.Language.Instance.Name) return;
            Service.Utility.Language.Instance.Update(Current.Config.App.CurrentLanguage);
            Apt.App.App.Current.GetRequiredService<MainWindowViewModel>().InitializeViewModel();
            Apt.App.App.Current.GetRequiredService<SettingsPageViewModel>().InitializeViewModel();
            Apt.App.App.Current.GetRequiredService<Image.SuperResolution.IndexViewModel>().InitializeViewModel();
            Apt.App.App.Current.GetRequiredService<Image.AutoWipe.IndexViewModel>().InitializeViewModel();
            Apt.App.App.Current.GetRequiredService<Image.CartoonComic.IndexViewModel>().InitializeViewModel();
            Apt.App.App.Current.GetRequiredService<Image.Convert3d.IndexViewModel>().InitializeViewModel();
            Apt.App.App.Current.GetRequiredService<Image.ColorRestoration.IndexViewModel>().InitializeViewModel();
            Apt.App.App.Current.GetRequiredService<Image.FrameInterpolation.IndexViewModel>().InitializeViewModel();
            Apt.App.App.Current.GetRequiredService<Image.Matting.IndexViewModel>().InitializeViewModel();
            Apt.App.App.Current.GetRequiredService<Image.FaceRestoration.IndexViewModel>().InitializeViewModel();
            Apt.App.App.Current.GetRequiredService<Video.SuperResolution.IndexViewModel>().InitializeViewModel();
            //Apt.App.App.Current.GetRequiredService<Video.AutoWipe.IndexViewModel>().InitializeViewModel();
            Apt.App.App.Current.GetRequiredService<Video.CartoonComic.IndexViewModel>().InitializeViewModel();
            Apt.App.App.Current.GetRequiredService<Video.Convert3d.IndexViewModel>().InitializeViewModel();
            Apt.App.App.Current.GetRequiredService<Video.ColorRestoration.IndexViewModel>().InitializeViewModel();
            Apt.App.App.Current.GetRequiredService<Video.FrameInterpolation.IndexViewModel>().InitializeViewModel();
            Apt.App.App.Current.GetRequiredService<Video.Matting.IndexViewModel>().InitializeViewModel();
            Apt.App.App.Current.GetRequiredService<Video.Organization.IndexViewModel>().InitializeViewModel();
            Language = Service.Utility.Language.Instance.Name;
            AppHostService.GetConfig();
        }
    }
}
