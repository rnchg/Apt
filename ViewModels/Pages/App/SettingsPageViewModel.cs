using General.Apt.App.Services;
using General.Apt.App.ViewModels.Windows.App;
using General.Apt.App.Views.Windows.App;
using General.Apt.App.Views.Windows.Chat.Gpt;
using General.Apt.Service.Consts;
using General.Apt.Service.Models;
using General.Apt.Service.Utility;
using System.IO;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;

namespace General.Apt.App.ViewModels.Pages.App
{
    public partial class SettingsPageViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;
        private INavigationService _navigationService;
        private WindowsProviderService _windowsService;

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<string, ApplicationTheme>> _themeSource;

        [ObservableProperty]
        private ComBoBoxItem<string, ApplicationTheme> _themeItem;

        public string Theme
        {
            get => ThemeItem?.Value.ToString();
            set => ThemeItem = ThemeSource.FirstOrDefault(e => e.Value.ToString() == value);
        }

        partial void OnThemeItemChanged(ComBoBoxItem<string, ApplicationTheme> value)
        {
            if (value?.Item == null) return;
            Current.Config.App.Theme = Theme;
            ApplicationThemeManager.Apply(value.Item);
        }

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<string, NavigationViewPaneDisplayMode>> _navigationStyleSource;

        [ObservableProperty]
        private ComBoBoxItem<string, NavigationViewPaneDisplayMode> _navigationStyleItem;

        public string NavigationStyle
        {
            get => NavigationStyleItem.Value.ToString();
            set => NavigationStyleItem = NavigationStyleSource.FirstOrDefault(e => e.Value.ToString() == value);
        }

        partial void OnNavigationStyleItemChanged(ComBoBoxItem<string, NavigationViewPaneDisplayMode> value)
        {
            if (value?.Item == null) return;
            Current.Config.App.NavigationStyle = NavigationStyle;
            _navigationService.SetPaneDisplayMode(value.Item);
        }

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<string, string>> _languageSource;

        [ObservableProperty]
        private ComBoBoxItem<string, string> _languageItem;

        public string Language
        {
            get => LanguageItem.Value.ToString();
            set => LanguageItem = LanguageSource.FirstOrDefault(e => e.Value.ToString() == value);
        }

        partial void OnLanguageItemChanged(ComBoBoxItem<string, string> value)
        {
            if (value?.Item == null) return;
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
            _windowsService.ShowDialog<ConfigWindow>();
        }

        [RelayCommand]
        private void SetLicense()
        {
            _windowsService.ShowDialog<LicenseWindow>();
        }

        public SettingsPageViewModel()
        {
            if (!_isInitialized) InitializeViewModel();
        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized) InitializeViewModel();
        }

        public void OnNavigatedFrom() { }

        public void InitializeViewModel()
        {
            _navigationService = Apt.App.App.Current.GetRequiredService<INavigationService>();
            _windowsService = Apt.App.App.Current.GetRequiredService<WindowsProviderService>();

            ThemeSource = new ObservableCollection<ComBoBoxItem<string, ApplicationTheme>>()
            {
                new ComBoBoxItem<string,ApplicationTheme>() { Text = Service.Utility.Language.Instance["SettingsPageThemeDark"], Value="Dark",  Item = ApplicationTheme.Dark },
                new ComBoBoxItem<string,ApplicationTheme>() { Text = Service.Utility.Language.Instance["SettingsPageThemeLight"], Value="Light", Item = ApplicationTheme.Light },
                new ComBoBoxItem<string,ApplicationTheme>() { Text = Service.Utility.Language.Instance["SettingsPageThemeHighContrast"], Value="HighContrast",  Item = ApplicationTheme.HighContrast }
            };
            NavigationStyleSource = new ObservableCollection<ComBoBoxItem<string, NavigationViewPaneDisplayMode>>()
            {
                new ComBoBoxItem<string, NavigationViewPaneDisplayMode>() { Text = Service.Utility.Language.Instance["SettingsPageNavigationLeft"], Value ="Left", Item = NavigationViewPaneDisplayMode.Left },
                new ComBoBoxItem<string, NavigationViewPaneDisplayMode>() { Text = Service.Utility.Language.Instance["SettingsPageNavigationLeftMinimal"], Value = "LeftMinimal",  Item = NavigationViewPaneDisplayMode.LeftMinimal },
                new ComBoBoxItem<string, NavigationViewPaneDisplayMode>() { Text = Service.Utility.Language.Instance["SettingsPageNavigationLeftFluent"], Value = "LeftFluent",  Item = NavigationViewPaneDisplayMode.LeftFluent }
            };
            LanguageSource = new ObservableCollection<ComBoBoxItem<string, string>>(Directory.GetFiles(AppConst.LanguagePath, "*.json").Select(x =>
            {
                var name = Path.GetFileNameWithoutExtension(x);
                return new ComBoBoxItem<string, string>()
                {
                    Text = name,
                    Value = name,
                    Item = name
                };
            }));

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
            //Apt.App.App.Current.GetRequiredService<Image.FrameInterpolation.IndexViewModel>().InitializeViewModel();
            Apt.App.App.Current.GetRequiredService<Image.FaceRestoration.IndexViewModel>().InitializeViewModel();
            Apt.App.App.Current.GetRequiredService<Video.SuperResolution.IndexViewModel>().InitializeViewModel();
            //Apt.App.App.Current.GetRequiredService<Video.AutoWipe.IndexViewModel>().InitializeViewModel();
            Apt.App.App.Current.GetRequiredService<Video.CartoonComic.IndexViewModel>().InitializeViewModel();
            Apt.App.App.Current.GetRequiredService<Video.Convert3d.IndexViewModel>().InitializeViewModel();
            Apt.App.App.Current.GetRequiredService<Video.ColorRestoration.IndexViewModel>().InitializeViewModel();
            Apt.App.App.Current.GetRequiredService<Video.FrameInterpolation.IndexViewModel>().InitializeViewModel();
            Apt.App.App.Current.GetRequiredService<Video.Organization.IndexViewModel>().InitializeViewModel();
            Language = Service.Utility.Language.Instance.Name;
            AppHostService.GetConfig();
        }
    }
}
