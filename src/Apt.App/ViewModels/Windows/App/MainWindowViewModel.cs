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
                    Content = Language.Instance["Route.Dashboard"],
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Home20 },
                    TargetPageType = typeof(Views.Pages.App.DashboardPage)
                },
                new NavigationViewItem()
                {
                    Content = Language.Instance["Route.Gen"],
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Chat20 },
                    MenuItemsSource = new ObservableCollection<object>()
                    {
                        new NavigationViewItem(Language.Instance["Route.GenChat"], SymbolRegular.Chat20, typeof(Views.Pages.Gen.Chat.IndexPage))
                    },
                    IsExpanded = true
                },
                new NavigationViewItem()
                {
                    Content = Language.Instance["Route.Image"],
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Image24 },
                    MenuItemsSource = new ObservableCollection<object>()
                    {
                        new NavigationViewItem(Language.Instance["Route.ImageSuperResolution"], SymbolRegular.Image24, typeof(Views.Pages.Image.SuperResolution.IndexPage)),
                        new NavigationViewItem(Language.Instance["Route.ImageAutoWipe"], SymbolRegular.Video36024, typeof(Views.Pages.Image.AutoWipe.IndexPage)),
                        new NavigationViewItem(Language.Instance["Route.ImageCartoonComic"], SymbolRegular.CommunicationPerson24, typeof(Views.Pages.Image.CartoonComic.IndexPage)),
                        new NavigationViewItem(Language.Instance["Route.ImageConvert3d"], SymbolRegular.VideoSwitch24, typeof(Views.Pages.Image.Convert3d.IndexPage)),
                        new NavigationViewItem(Language.Instance["Route.ImageColorRestoration"], SymbolRegular.VideoPersonSparkle24, typeof(Views.Pages.Image.ColorRestoration.IndexPage)),
                        new NavigationViewItem(Language.Instance["Route.ImageFrameInterpolation"], SymbolRegular.VideoClipMultiple24, typeof(Views.Pages.Image.FrameInterpolation.IndexPage)),
                        new NavigationViewItem(Language.Instance["Route.ImageMatting"], SymbolRegular.ImageMultiple24, typeof(Views.Pages.Image.Matting.IndexPage)),
                        new NavigationViewItem(Language.Instance["Route.ImageFaceRestoration"], SymbolRegular.VideoPersonStar24, typeof(Views.Pages.Image.FaceRestoration.IndexPage))
                    },
                    IsExpanded = true
                },
                new NavigationViewItem()
                {
                    Content = Language.Instance["Route.Video"],
                    Icon = new SymbolIcon { Symbol = SymbolRegular.VideoClip24 },
                    MenuItemsSource = new ObservableCollection<object>()
                    {
                        new NavigationViewItem(Language.Instance["Route.VideoSuperResolution"], SymbolRegular.Video24, typeof(Views.Pages.Video.SuperResolution.IndexPage)),
                        new NavigationViewItem(Language.Instance["Route.VideoAutoWipe"], SymbolRegular.Video36024, typeof(Views.Pages.Video.AutoWipe.IndexPage)),
                        new NavigationViewItem(Language.Instance["Route.VideoCartoonComic"], SymbolRegular.CommunicationPerson24, typeof(Views.Pages.Video.CartoonComic.IndexPage)),
                        new NavigationViewItem(Language.Instance["Route.VideoConvert3d"], SymbolRegular.VideoSwitch24, typeof(Views.Pages.Video.Convert3d.IndexPage)),
                        new NavigationViewItem(Language.Instance["Route.VideoColorRestoration"], SymbolRegular.VideoPersonSparkle24, typeof(Views.Pages.Video.ColorRestoration.IndexPage)),
                        new NavigationViewItem(Language.Instance["Route.VideoFrameInterpolation"], SymbolRegular.VideoClipMultiple24, typeof(Views.Pages.Video.FrameInterpolation.IndexPage)),
                        new NavigationViewItem(Language.Instance["Route.VideoMatting"], SymbolRegular.ImageMultiple24, typeof(Views.Pages.Video.Matting.IndexPage)),
                        new NavigationViewItem(Language.Instance["Route.VideoOrganization"], SymbolRegular.VideoClip24, typeof(Views.Pages.Video.Organization.IndexPage))
                    },
                    IsExpanded = true
                },
                new NavigationViewItem()
                {
                    Content = Language.Instance["Route.Audio"],
                    Icon = new SymbolIcon { Symbol = SymbolRegular.HeadphonesSoundWave24 },
                    MenuItemsSource = new ObservableCollection<object>()
                    {
                        new NavigationViewItem(Language.Instance["Route.AudioVocalSplit"], SymbolRegular.MusicNote224, typeof(Views.Pages.Audio.VocalSplit.IndexPage)),
                        new NavigationViewItem(Language.Instance["Route.AudioDenoise"], SymbolRegular.HeadphonesSoundWave24, typeof(Views.Pages.Audio.Denoise.IndexPage)),
                        //new NavigationViewItem(Language.Instance["Route.AudioTTS"], SymbolRegular.SoundWaveCircle24, typeof(Views.Pages.Audio.VocalSplit.IndexPage)),
                        //new NavigationViewItem(Language.Instance["Route.AudioSTT"], SymbolRegular.ClipboardTextLtr24, typeof(Views.Pages.Audio.VocalSplit.IndexPage)),
                    },
                    IsExpanded = true
                }
            ];
            FooterMenuItems =
            [
                new NavigationViewItem()
                {
                    Content = Language.Instance["Route.Setting"],
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Settings20 },
                    TargetPageType = typeof(Views.Pages.App.SettingPage)
                }
            ];
        }
    }
}
