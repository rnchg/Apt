using General.Apt.Service.Utility;
using Wpf.Ui.Controls;

namespace General.Apt.App.ViewModels.Windows.App
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
                    Content = Language.Instance["MainWindowHome"],
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                    TargetPageType = typeof(Views.Pages.App.DashboardPage)
                },
                new NavigationViewItem()
                {
                    Content = Language.Instance["MainWindowChat"],
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Chat24 },
                    MenuItemsSource = new ObservableCollection<object>()
                    {
                        new NavigationViewItem(Language.Instance["MainWindowChatChatGPT"], typeof(Views.Pages.Chat.Gpt.IndexPage))
                    },
                    IsExpanded = true
                },
                new NavigationViewItem()
                {
                    Content = Language.Instance["MainWindowImage"],
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Image24 },
                    MenuItemsSource = new ObservableCollection<object>()
                    {
                        new NavigationViewItem(Language.Instance["MainWindowImageSuperResolution"], typeof(Views.Pages.Image.SuperResolution.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowImageAutoWipe"], typeof(Views.Pages.Image.AutoWipe.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowImageCartoonComic"], typeof(Views.Pages.Image.CartoonComic.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowImageConvert3d"], typeof(Views.Pages.Image.Convert3d.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowImageColorRestoration"], typeof(Views.Pages.Image.ColorRestoration.IndexPage)),
                        //new NavigationViewItem(Language.Instance["MainWindowImageFrameInterpolation"], typeof(Views.Pages.Image.FrameInterpolation.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowImageFaceRestoration"], typeof(Views.Pages.Image.FaceRestoration.IndexPage))
                    },
                    IsExpanded = true
                },
                new NavigationViewItem()
                {
                    Content = Language.Instance["MainWindowVideo"],
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Video24 },
                    MenuItemsSource = new ObservableCollection<object>()
                    {
                        new NavigationViewItem(Language.Instance["MainWindowVideoSuperResolution"], typeof(Views.Pages.Video.SuperResolution.IndexPage)),
                        //new NavigationViewItem(Language.Instance["MainWindowVideoAutoWipe"], typeof(Views.Pages.Video.AutoWipe.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowVideoCartoonComic"], typeof(Views.Pages.Video.CartoonComic.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowVideoConvert3d"], typeof(Views.Pages.Video.Convert3d.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowVideoColorRestoration"], typeof(Views.Pages.Video.ColorRestoration.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowVideoFrameInterpolation"], typeof(Views.Pages.Video.FrameInterpolation.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowVideoOrganization"], typeof(Views.Pages.Video.Organization.IndexPage))
                    },
                    IsExpanded = true
                },
            };

            FooterMenuItems = new ObservableCollection<object>()
            {
                new NavigationViewItem()
                {
                    Content = Language.Instance["MainWindowSetting"],
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                    TargetPageType = typeof(Views.Pages.App.SettingsPage)
                },
                //new NavigationViewItem()
                //{
                //    Content = Language.Instance["MainWindowHelp"],
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
