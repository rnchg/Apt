using Apt.App.Extensions;
using Apt.App.ViewModels.Windows.App;
using Apt.App.Views.Windows.Gen.Chat;
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
            ServiceProvider.GetRequiredService<INavigationService>().SetPaneDisplayMode(value.Item);
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
        private void SetGenChatConfig()
        {
            ServiceProvider.GetRequiredService<WindowsProviderService>().ShowDialog<ConfigWindow>();
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

        public SettingsPageViewModel(
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
                new ComBoBoxItem<string,ApplicationTheme>() { Text = Core.Utility.Language.Instance["SettingsPageThemeDark"], Value="Dark",  Item = ApplicationTheme.Dark },
                new ComBoBoxItem<string,ApplicationTheme>() { Text = Core.Utility.Language.Instance["SettingsPageThemeLight"], Value="Light", Item = ApplicationTheme.Light },
                new ComBoBoxItem<string,ApplicationTheme>() { Text = Core.Utility.Language.Instance["SettingsPageThemeHighContrast"], Value="HighContrast",  Item = ApplicationTheme.HighContrast }
            ];
            NavigationStyleSource =
            [
                new ComBoBoxItem<string, NavigationViewPaneDisplayMode>() { Text = Core.Utility.Language.Instance["SettingsPageNavigationLeft"], Value ="Left", Item = NavigationViewPaneDisplayMode.Left },
                new ComBoBoxItem<string, NavigationViewPaneDisplayMode>() { Text = Core.Utility.Language.Instance["SettingsPageNavigationLeftMinimal"], Value = "LeftMinimal",  Item = NavigationViewPaneDisplayMode.LeftMinimal },
                new ComBoBoxItem<string, NavigationViewPaneDisplayMode>() { Text = Core.Utility.Language.Instance["SettingsPageNavigationLeftFluent"], Value = "LeftFluent",  Item = NavigationViewPaneDisplayMode.LeftFluent }
            ];

            Theme = Current.Config.App.Theme;
            NavigationStyle = Current.Config.App.NavigationStyle;
            IsAutoOpenOutput = Current.Config.App.IsAutoOpenOutput;

            _isInitialized = true;
        }

        private void UpdateLanguage()
        {
            if (Current.Config.App.CurrentLanguage == Core.Utility.Language.Instance.Name) return;
            Core.Utility.Language.Instance.Update(Current.Config.App.CurrentLanguage);
            ServiceProvider.GetRequiredService<MainWindowViewModel>().InitializeViewModel();
            ServiceProvider.GetRequiredService<SettingsPageViewModel>().InitializeViewModel();
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
            ServiceProvider.GetRequiredService<Audio.Denoise.IndexPageViewModel>().InitializeViewModel();
            ServiceProvider.GetRequiredService<Audio.VocalSplit.IndexPageViewModel>().InitializeViewModel();
            Language = Core.Utility.Language.Instance.Name;
            ServiceProvider.GetConfig();
        }
    }
}
