using General.Apt.Service.Utility;
using Wpf.Ui.Controls;

namespace General.Apt.App.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private ObservableCollection<object> _menuItems;

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems;

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems;

        public MainWindowViewModel()
        {
            if (!_isInitialized) InitializeViewModel();
        }

        public void InitializeViewModel()
        {
            MenuItems = new ObservableCollection<object>()
            {
                new NavigationViewItem()
                {
                    Content = Language.GetString("MainWindowHome"),
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                    TargetPageType = typeof(Views.Pages.App.DashboardPage)
                },
                new NavigationViewItem()
                {
                    Content = Language.GetString("MainWindowChat"),
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Chat24 },
                    MenuItemsSource = new ObservableCollection<object>()
                    {
                        new NavigationViewItem(Language.GetString("MainWindowChatChatGPT"), typeof(Views.Pages.Chat.Gpt.IndexPage))
                    },
                    IsExpanded = true
                },
                new NavigationViewItem()
                {
                    Content = Language.GetString("MainWindowImage"),
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Image24 },
                    MenuItemsSource = new ObservableCollection<object>()
                    {
                        new NavigationViewItem(Language.GetString("MainWindowImageSuperResolution"), typeof(Views.Pages.Image.SuperResolution.IndexPage)),
                        new NavigationViewItem(Language.GetString("MainWindowImageAutoWipe"), typeof(Views.Pages.Image.AutoWipe.IndexPage)),
                        new NavigationViewItem(Language.GetString("MainWindowImageCartoonComic"), typeof(Views.Pages.Image.CartoonComic.IndexPage)),
                        new NavigationViewItem(Language.GetString("MainWindowImageConvert3d"), typeof(Views.Pages.Image.Convert3d.IndexPage)),
                        new NavigationViewItem(Language.GetString("MainWindowImageColorRestoration"), typeof(Views.Pages.Image.ColorRestoration.IndexPage)),
                        new NavigationViewItem(Language.GetString("MainWindowImageFaceRestoration"), typeof(Views.Pages.Image.FaceRestoration.IndexPage))
                    },
                    IsExpanded = true
                },
                new NavigationViewItem()
                {
                    Content = Language.GetString("MainWindowVideo"),
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Video24 },
                    MenuItemsSource = new ObservableCollection<object>()
                    {
                        new NavigationViewItem(Language.GetString("MainWindowVideoSuperResolution"), typeof(Views.Pages.Video.SuperResolution.IndexPage)),
                        //new NavigationViewItem(Language.GetString("MainWindowVideoAutoWipe"), typeof(Views.Pages.Video.AutoWipe.IndexPage)),
                        new NavigationViewItem(Language.GetString("MainWindowVideoCartoonComic"), typeof(Views.Pages.Video.CartoonComic.IndexPage)),
                        new NavigationViewItem(Language.GetString("MainWindowVideoConvert3d"), typeof(Views.Pages.Video.Convert3d.IndexPage)),
                        new NavigationViewItem(Language.GetString("MainWindowVideoColorRestoration"), typeof(Views.Pages.Video.ColorRestoration.IndexPage)),
                        new NavigationViewItem(Language.GetString("MainWindowVideoFrameInterpolation"), typeof(Views.Pages.Video.FrameInterpolation.IndexPage)),
                        new NavigationViewItem(Language.GetString("MainWindowVideoOrganization"), typeof(Views.Pages.Video.Organization.IndexPage))
                    },
                    IsExpanded = true
                },
            };

            FooterMenuItems = new ObservableCollection<object>()
            {
                new NavigationViewItem()
                {
                    Content = Language.GetString("MainWindowSetting"),
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                    TargetPageType = typeof(Views.Pages.App.SettingsPage)
                },
                //new NavigationViewItem()
                //{
                //    Content = Language.GetString("MainWindowHelp"),
                //    Icon = new SymbolIcon { Symbol = SymbolRegular.ChatHelp24 },
                //    TargetPageType = typeof(Views.Pages.App.HelpPage)
                //}
            };

            TrayMenuItems = new ObservableCollection<MenuItem>()
            {
                new MenuItem { Header = "Home", Tag = "tray_home" }
            };

            _isInitialized = true;
        }
    }
}
