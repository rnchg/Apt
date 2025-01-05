using Apt.Core.Utility;
using Apt.Service.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Apt.App.ViewModels.Windows.App
{
    public partial class MainWindowViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<object> _menuItems = [];

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = [];

        public MainWindowViewModel(
            IServiceProvider serviceProvider,
            ISnackbarService snackbarService) :
            base(serviceProvider, snackbarService)
        {
            InitializeViewModel();
        }

        public void InitializeViewModel()
        {
            MenuItems =
            [
                new NavigationViewItem()
                {
                    Content = Language.Instance["MainWindowHome"],
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Home20 },
                    TargetPageType = typeof(Views.Pages.App.DashboardPage),
                },
                new NavigationViewItem()
                {
                    Content = Language.Instance["MainWindowGen"],
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Chat20 },
                    MenuItemsSource = new ObservableCollection<object>()
                    {
                        new NavigationViewItem(Language.Instance["MainWindowGenChat"], typeof(Views.Pages.Gen.Chat.IndexPage))
                    },
                    IsExpanded = true
                },
                new NavigationViewItem()
                {
                    Content = Language.Instance["MainWindowImage"],
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Image20 },
                    MenuItemsSource = new ObservableCollection<object>()
                    {
                        new NavigationViewItem(Language.Instance["MainWindowImageSuperResolution"], typeof(Views.Pages.Image.SuperResolution.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowImageAutoWipe"], typeof(Views.Pages.Image.AutoWipe.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowImageCartoonComic"], typeof(Views.Pages.Image.CartoonComic.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowImageConvert3d"], typeof(Views.Pages.Image.Convert3d.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowImageColorRestoration"], typeof(Views.Pages.Image.ColorRestoration.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowImageFrameInterpolation"], typeof(Views.Pages.Image.FrameInterpolation.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowImageMatting"], typeof(Views.Pages.Image.Matting.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowImageFaceRestoration"], typeof(Views.Pages.Image.FaceRestoration.IndexPage))
                    },
                    IsExpanded = true
                },
                new NavigationViewItem()
                {
                    Content = Language.Instance["MainWindowVideo"],
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Video20 },
                    MenuItemsSource = new ObservableCollection<object>()
                    {
                        new NavigationViewItem(Language.Instance["MainWindowVideoSuperResolution"], typeof(Views.Pages.Video.SuperResolution.IndexPage)),
                        //new NavigationViewItem(Language.Instance["MainWindowVideoAutoWipe"], typeof(Views.Pages.Video.AutoWipe.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowVideoCartoonComic"], typeof(Views.Pages.Video.CartoonComic.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowVideoConvert3d"], typeof(Views.Pages.Video.Convert3d.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowVideoColorRestoration"], typeof(Views.Pages.Video.ColorRestoration.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowVideoFrameInterpolation"], typeof(Views.Pages.Video.FrameInterpolation.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowVideoMatting"], typeof(Views.Pages.Video.Matting.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowVideoOrganization"], typeof(Views.Pages.Video.Organization.IndexPage))
                    },
                    IsExpanded = true
                },
                //new NavigationViewItem()
                //{
                //    Content = Language.Instance["MainWindowAudio"],
                //    Icon = new SymbolIcon { Symbol = SymbolRegular.Video20 },
                //    MenuItemsSource = new ObservableCollection<object>()
                //    {
                //        new NavigationViewItem(Language.Instance["MainWindowAudioDenoise"], typeof(Views.Pages.Audio.Denoise.IndexPage)),
                //        new NavigationViewItem(Language.Instance["MainWindowAudioVocalSplit"], typeof(Views.Pages.Audio.VocalSplit.IndexPage)),
                //        new NavigationViewItem(Language.Instance["MainWindowAudioCloneSingle"], typeof(Views.Pages.App.DashboardPage))
                //    },
                //    IsExpanded = true
                //}
            ];
            FooterMenuItems =
            [
                new NavigationViewItem()
                {
                    Content = Language.Instance["MainWindowSetting"],
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Settings20 },
                    TargetPageType = typeof(Views.Pages.App.SettingsPage)
                }
            ];
        }
    }
}
