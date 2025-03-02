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
                    TargetPageType = typeof(Views.Pages.App.DashboardPage)
                },
                new NavigationViewItem()
                {
                    Content = Language.Instance["MainWindowGen"],
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Chat20 },
                    MenuItemsSource = new ObservableCollection<object>()
                    {
                        new NavigationViewItem(Language.Instance["MainWindowGenChat"], SymbolRegular.Chat20, typeof(Views.Pages.Gen.Chat.IndexPage))
                    },
                    IsExpanded = true
                },
                new NavigationViewItem()
                {
                    Content = Language.Instance["MainWindowImage"],
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Image24 },
                    MenuItemsSource = new ObservableCollection<object>()
                    {
                        new NavigationViewItem(Language.Instance["MainWindowImageSuperResolution"], SymbolRegular.Image24, typeof(Views.Pages.Image.SuperResolution.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowImageAutoWipe"], SymbolRegular.Video36024, typeof(Views.Pages.Image.AutoWipe.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowImageCartoonComic"], SymbolRegular.CommunicationPerson24, typeof(Views.Pages.Image.CartoonComic.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowImageConvert3d"], SymbolRegular.VideoSwitch24, typeof(Views.Pages.Image.Convert3d.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowImageColorRestoration"], SymbolRegular.VideoPersonSparkle24, typeof(Views.Pages.Image.ColorRestoration.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowImageFrameInterpolation"], SymbolRegular.VideoClipMultiple24, typeof(Views.Pages.Image.FrameInterpolation.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowImageMatting"], SymbolRegular.ImageMultiple24, typeof(Views.Pages.Image.Matting.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowImageFaceRestoration"], SymbolRegular.VideoPersonStar24, typeof(Views.Pages.Image.FaceRestoration.IndexPage))
                    },
                    IsExpanded = true
                },
                new NavigationViewItem()
                {
                    Content = Language.Instance["MainWindowVideo"],
                    Icon = new SymbolIcon { Symbol = SymbolRegular.VideoClip24 },
                    MenuItemsSource = new ObservableCollection<object>()
                    {
                        new NavigationViewItem(Language.Instance["MainWindowVideoSuperResolution"], SymbolRegular.Video24, typeof(Views.Pages.Video.SuperResolution.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowVideoAutoWipe"], SymbolRegular.Video36024, typeof(Views.Pages.Video.AutoWipe.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowVideoCartoonComic"], SymbolRegular.CommunicationPerson24, typeof(Views.Pages.Video.CartoonComic.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowVideoConvert3d"], SymbolRegular.VideoSwitch24, typeof(Views.Pages.Video.Convert3d.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowVideoColorRestoration"], SymbolRegular.VideoPersonSparkle24, typeof(Views.Pages.Video.ColorRestoration.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowVideoFrameInterpolation"], SymbolRegular.VideoClipMultiple24, typeof(Views.Pages.Video.FrameInterpolation.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowVideoMatting"], SymbolRegular.ImageMultiple24, typeof(Views.Pages.Video.Matting.IndexPage)),
                        new NavigationViewItem(Language.Instance["MainWindowVideoOrganization"], SymbolRegular.VideoClip24, typeof(Views.Pages.Video.Organization.IndexPage))
                    },
                    IsExpanded = true
                },
                //new NavigationViewItem()
                //{
                //    Content = Language.Instance["MainWindowAudio"],
                //    Icon = new SymbolIcon { Symbol = SymbolRegular.HeadphonesSoundWave24 },
                //    MenuItemsSource = new ObservableCollection<object>()
                //    {
                //        new NavigationViewItem(Language.Instance["MainWindowAudioVocalSplit"], SymbolRegular.MusicNote224, typeof(Views.Pages.Audio.VocalSplit.IndexPage)),
                //        new NavigationViewItem(Language.Instance["MainWindowAudioTTS"], SymbolRegular.SoundWaveCircle24, typeof(Views.Pages.Audio.VocalSplit.IndexPage)),
                //        new NavigationViewItem(Language.Instance["MainWindowAudioSTT"], SymbolRegular.ClipboardTextLtr24, typeof(Views.Pages.Audio.VocalSplit.IndexPage)),
                //        new NavigationViewItem(Language.Instance["MainWindowAudioDenoise"], SymbolRegular.HeadphonesSoundWave24, typeof(Views.Pages.Audio.Denoise.IndexPage))
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
                    TargetPageType = typeof(Views.Pages.App.SettingPage)
                }
            ];
        }
    }
}
