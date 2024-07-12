using General.Apt.App.Services.Contracts;
using General.Apt.App.Utility;
using General.Apt.App.ViewModels.Pages.App;
using General.Apt.App.Views.Pages.App;
using General.Apt.App.Views.Windows;
using General.Apt.Service.Helpers;
using General.Apt.Service.Utility;

namespace General.Apt.App.Services
{
    public class AppHostService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public AppHostService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Setting.GetSetting();
            GetSetting();
            return HandleActivationAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            SetSetting();
            Setting.SetSetting();
            return Task.CompletedTask;
        }

        public static void GetSetting()
        {
            var imageSuperResolutionViewModel = App.Current.GetService<ViewModels.Pages.Image.SuperResolution.IndexViewModel>();
            imageSuperResolutionViewModel.Input = Current.Config.ImageSuperResolution.Input;
            imageSuperResolutionViewModel.Output = Current.Config.ImageSuperResolution.Output;
            imageSuperResolutionViewModel.InputSort = Current.Config.ImageSuperResolution.InputSort;
            imageSuperResolutionViewModel.SortRule = Current.Config.ImageSuperResolution.SortRule;
            imageSuperResolutionViewModel.Provider = Current.Config.ImageSuperResolution.Provider;
            imageSuperResolutionViewModel.Mode = Current.Config.ImageSuperResolution.Mode;
            imageSuperResolutionViewModel.Scale = Current.Config.ImageSuperResolution.Scale;

            var imageAutoWipeViewModel = App.Current.GetService<ViewModels.Pages.Image.AutoWipe.IndexViewModel>();
            imageAutoWipeViewModel.Input = Current.Config.ImageAutoWipe.Input;
            imageAutoWipeViewModel.Output = Current.Config.ImageAutoWipe.Output;
            imageAutoWipeViewModel.InputSort = Current.Config.ImageAutoWipe.InputSort;
            imageAutoWipeViewModel.SortRule = Current.Config.ImageAutoWipe.SortRule;
            imageAutoWipeViewModel.Provider = Current.Config.ImageAutoWipe.Provider;
            imageAutoWipeViewModel.Mode = Current.Config.ImageAutoWipe.Mode;

            var imageCartoonComicViewModel = App.Current.GetService<ViewModels.Pages.Image.CartoonComic.IndexViewModel>();
            imageCartoonComicViewModel.Input = Current.Config.ImageCartoonComic.Input;
            imageCartoonComicViewModel.Output = Current.Config.ImageCartoonComic.Output;
            imageCartoonComicViewModel.InputSort = Current.Config.ImageCartoonComic.InputSort;
            imageCartoonComicViewModel.SortRule = Current.Config.ImageCartoonComic.SortRule;
            imageCartoonComicViewModel.Provider = Current.Config.ImageCartoonComic.Provider;
            imageCartoonComicViewModel.Mode = Current.Config.ImageCartoonComic.Mode;
            imageCartoonComicViewModel.Quality = Current.Config.ImageCartoonComic.Quality;

            var imageConvert3dViewModel = App.Current.GetService<ViewModels.Pages.Image.Convert3d.IndexViewModel>();
            imageConvert3dViewModel.Input = Current.Config.ImageConvert3d.Input;
            imageConvert3dViewModel.Output = Current.Config.ImageConvert3d.Output;
            imageConvert3dViewModel.InputSort = Current.Config.ImageConvert3d.InputSort;
            imageConvert3dViewModel.SortRule = Current.Config.ImageConvert3d.SortRule;
            imageConvert3dViewModel.Provider = Current.Config.ImageConvert3d.Provider;
            imageConvert3dViewModel.Mode = Current.Config.ImageConvert3d.Mode;
            imageConvert3dViewModel.Format = Current.Config.ImageConvert3d.Format;
            imageConvert3dViewModel.Shift = Current.Config.ImageConvert3d.Shift;
            imageConvert3dViewModel.PopOut = Current.Config.ImageConvert3d.PopOut;
            imageConvert3dViewModel.CrossEye = Current.Config.ImageConvert3d.CrossEye;

            var imageColorRestorationViewModel = App.Current.GetService<ViewModels.Pages.Image.ColorRestoration.IndexViewModel>();
            imageColorRestorationViewModel.Input = Current.Config.ImageColorRestoration.Input;
            imageColorRestorationViewModel.Output = Current.Config.ImageColorRestoration.Output;
            imageColorRestorationViewModel.InputSort = Current.Config.ImageColorRestoration.InputSort;
            imageColorRestorationViewModel.SortRule = Current.Config.ImageColorRestoration.SortRule;
            imageColorRestorationViewModel.Provider = Current.Config.ImageColorRestoration.Provider;
            imageColorRestorationViewModel.Mode = Current.Config.ImageColorRestoration.Mode;
            imageColorRestorationViewModel.Quality = Current.Config.ImageColorRestoration.Quality;

            //var imageFrameInterpolationViewModel = App.Current.GetService<ViewModels.Pages.Image.FrameInterpolation.IndexViewModel>();
            //imageFrameInterpolationViewModel.Input = Current.Config.ImageFrameInterpolation.Input;
            //imageFrameInterpolationViewModel.Output = Current.Config.ImageFrameInterpolation.Output;
            //imageFrameInterpolationViewModel.InputSort = Current.Config.ImageFrameInterpolation.InputSort;
            //imageFrameInterpolationViewModel.SortRule = Current.Config.ImageFrameInterpolation.SortRule;
            //imageFrameInterpolationViewModel.Provider = Current.Config.ImageFrameInterpolation.Provider;
            //imageFrameInterpolationViewModel.Mode = Current.Config.ImageFrameInterpolation.Mode;
            //imageFrameInterpolationViewModel.Scale = Current.Config.ImageFrameInterpolation.Scale;

            var imageFaceRestorationViewModel = App.Current.GetService<ViewModels.Pages.Image.FaceRestoration.IndexViewModel>();
            imageFaceRestorationViewModel.Input = Current.Config.ImageFaceRestoration.Input;
            imageFaceRestorationViewModel.Output = Current.Config.ImageFaceRestoration.Output;
            imageFaceRestorationViewModel.InputSort = Current.Config.ImageFaceRestoration.InputSort;
            imageFaceRestorationViewModel.SortRule = Current.Config.ImageFaceRestoration.SortRule;
            imageFaceRestorationViewModel.Provider = Current.Config.ImageFaceRestoration.Provider;
            imageFaceRestorationViewModel.Mode = Current.Config.ImageFaceRestoration.Mode;

            var videoSuperResolutionViewModel = App.Current.GetService<ViewModels.Pages.Video.SuperResolution.IndexViewModel>();
            videoSuperResolutionViewModel.Input = Current.Config.VideoSuperResolution.Input;
            videoSuperResolutionViewModel.Output = Current.Config.VideoSuperResolution.Output;
            videoSuperResolutionViewModel.InputSort = Current.Config.VideoSuperResolution.InputSort;
            videoSuperResolutionViewModel.SortRule = Current.Config.VideoSuperResolution.SortRule;
            videoSuperResolutionViewModel.Provider = Current.Config.VideoSuperResolution.Provider;
            videoSuperResolutionViewModel.Mode = Current.Config.VideoSuperResolution.Mode;
            videoSuperResolutionViewModel.Scale = Current.Config.VideoSuperResolution.Scale;

            //var videoAutoWipeViewModel = App.Current.GetService<ViewModels.Pages.Video.AutoWipe.IndexViewModel>();
            //videoAutoWipeViewModel.Input = Current.Config.VideoAutoWipe.Input;
            //videoAutoWipeViewModel.Output = Current.Config.VideoAutoWipe.Output;
            //videoAutoWipeViewModel.InputSort = Current.Config.VideoAutoWipe.InputSort;
            //videoAutoWipeViewModel.SortRule = Current.Config.VideoAutoWipe.SortRule;
            //videoAutoWipeViewModel.Provider = Current.Config.VideoAutoWipe.Provider;
            //videoAutoWipeViewModel.Mode = Current.Config.VideoAutoWipe.Mode;

            var videoCartoonComicViewModel = App.Current.GetService<ViewModels.Pages.Video.CartoonComic.IndexViewModel>();
            videoCartoonComicViewModel.Input = Current.Config.VideoCartoonComic.Input;
            videoCartoonComicViewModel.Output = Current.Config.VideoCartoonComic.Output;
            videoCartoonComicViewModel.InputSort = Current.Config.VideoCartoonComic.InputSort;
            videoCartoonComicViewModel.SortRule = Current.Config.VideoCartoonComic.SortRule;
            videoCartoonComicViewModel.Provider = Current.Config.VideoCartoonComic.Provider;
            videoCartoonComicViewModel.Mode = Current.Config.VideoCartoonComic.Mode;
            videoCartoonComicViewModel.Quality = Current.Config.VideoCartoonComic.Quality;

            var videoConvert3dViewModel = App.Current.GetService<ViewModels.Pages.Video.Convert3d.IndexViewModel>();
            videoConvert3dViewModel.Input = Current.Config.VideoConvert3d.Input;
            videoConvert3dViewModel.Output = Current.Config.VideoConvert3d.Output;
            videoConvert3dViewModel.InputSort = Current.Config.VideoConvert3d.InputSort;
            videoConvert3dViewModel.SortRule = Current.Config.VideoConvert3d.SortRule;
            videoConvert3dViewModel.Provider = Current.Config.VideoConvert3d.Provider;
            videoConvert3dViewModel.Mode = Current.Config.VideoConvert3d.Mode;
            videoConvert3dViewModel.Format = Current.Config.VideoConvert3d.Format;
            videoConvert3dViewModel.Shift = Current.Config.VideoConvert3d.Shift;
            videoConvert3dViewModel.PopOut = Current.Config.VideoConvert3d.PopOut;
            videoConvert3dViewModel.CrossEye = Current.Config.VideoConvert3d.CrossEye;

            var videoColorRestorationViewModel = App.Current.GetService<ViewModels.Pages.Video.ColorRestoration.IndexViewModel>();
            videoColorRestorationViewModel.Input = Current.Config.VideoColorRestoration.Input;
            videoColorRestorationViewModel.Output = Current.Config.VideoColorRestoration.Output;
            videoColorRestorationViewModel.InputSort = Current.Config.VideoColorRestoration.InputSort;
            videoColorRestorationViewModel.SortRule = Current.Config.VideoColorRestoration.SortRule;
            videoColorRestorationViewModel.Provider = Current.Config.VideoColorRestoration.Provider;
            videoColorRestorationViewModel.Mode = Current.Config.VideoColorRestoration.Mode;
            videoColorRestorationViewModel.Quality = Current.Config.VideoColorRestoration.Quality;

            var videoFrameInterpolationViewModel = App.Current.GetService<ViewModels.Pages.Video.FrameInterpolation.IndexViewModel>();
            videoFrameInterpolationViewModel.Input = Current.Config.VideoFrameInterpolation.Input;
            videoFrameInterpolationViewModel.Output = Current.Config.VideoFrameInterpolation.Output;
            videoFrameInterpolationViewModel.InputSort = Current.Config.VideoFrameInterpolation.InputSort;
            videoFrameInterpolationViewModel.SortRule = Current.Config.VideoFrameInterpolation.SortRule;
            videoFrameInterpolationViewModel.Provider = Current.Config.VideoFrameInterpolation.Provider;
            videoFrameInterpolationViewModel.Mode = Current.Config.VideoFrameInterpolation.Mode;
            videoFrameInterpolationViewModel.Scale = Current.Config.VideoFrameInterpolation.Scale;

            var videoOrganizationIndexViewMode = App.Current.GetService<ViewModels.Pages.Video.Organization.IndexViewModel>();
            videoOrganizationIndexViewMode.Input = Current.Config.VideoOrganization.Input;
            videoOrganizationIndexViewMode.Output = Current.Config.VideoOrganization.Output;
            videoOrganizationIndexViewMode.InputSort = Current.Config.VideoOrganization.InputSort;
            videoOrganizationIndexViewMode.SortRule = Current.Config.VideoOrganization.SortRule;
            videoOrganizationIndexViewMode.Client = Current.Config.VideoOrganization.Client;
        }

        public static void SetSetting()
        {
            var imageSuperResolutionViewModel = App.Current.GetService<ViewModels.Pages.Image.SuperResolution.IndexViewModel>();
            Current.Config.ImageSuperResolution.Input = imageSuperResolutionViewModel.Input;
            Current.Config.ImageSuperResolution.Output = imageSuperResolutionViewModel.Output;
            Current.Config.ImageSuperResolution.InputSort = imageSuperResolutionViewModel.InputSort;
            Current.Config.ImageSuperResolution.SortRule = imageSuperResolutionViewModel.SortRule;
            Current.Config.ImageSuperResolution.Provider = imageSuperResolutionViewModel.Provider;
            Current.Config.ImageSuperResolution.Mode = imageSuperResolutionViewModel.Mode;
            Current.Config.ImageSuperResolution.Scale = imageSuperResolutionViewModel.Scale;

            var imageAutoWipeViewModel = App.Current.GetService<ViewModels.Pages.Image.AutoWipe.IndexViewModel>();
            Current.Config.ImageAutoWipe.Input = imageAutoWipeViewModel.Input;
            Current.Config.ImageAutoWipe.Output = imageAutoWipeViewModel.Output;
            Current.Config.ImageAutoWipe.InputSort = imageAutoWipeViewModel.InputSort;
            Current.Config.ImageAutoWipe.SortRule = imageAutoWipeViewModel.SortRule;
            Current.Config.ImageAutoWipe.Provider = imageAutoWipeViewModel.Provider;
            Current.Config.ImageAutoWipe.Mode = imageAutoWipeViewModel.Mode;

            var imageCartoonComicViewModel = App.Current.GetService<ViewModels.Pages.Image.CartoonComic.IndexViewModel>();
            Current.Config.ImageCartoonComic.Input = imageCartoonComicViewModel.Input;
            Current.Config.ImageCartoonComic.Output = imageCartoonComicViewModel.Output;
            Current.Config.ImageCartoonComic.InputSort = imageCartoonComicViewModel.InputSort;
            Current.Config.ImageCartoonComic.SortRule = imageCartoonComicViewModel.SortRule;
            Current.Config.ImageCartoonComic.Provider = imageCartoonComicViewModel.Provider;
            Current.Config.ImageCartoonComic.Mode = imageCartoonComicViewModel.Mode;
            Current.Config.ImageCartoonComic.Quality = imageCartoonComicViewModel.Quality;

            var imageConvert3dViewModel = App.Current.GetService<ViewModels.Pages.Image.Convert3d.IndexViewModel>();
            Current.Config.ImageConvert3d.Input = imageConvert3dViewModel.Input;
            Current.Config.ImageConvert3d.Output = imageConvert3dViewModel.Output;
            Current.Config.ImageConvert3d.InputSort = imageConvert3dViewModel.InputSort;
            Current.Config.ImageConvert3d.SortRule = imageConvert3dViewModel.SortRule;
            Current.Config.ImageConvert3d.Provider = imageConvert3dViewModel.Provider;
            Current.Config.ImageConvert3d.Mode = imageConvert3dViewModel.Mode;
            Current.Config.ImageConvert3d.Format = imageConvert3dViewModel.Format;
            Current.Config.ImageConvert3d.Shift = imageConvert3dViewModel.Shift;
            Current.Config.ImageConvert3d.PopOut = imageConvert3dViewModel.PopOut;
            Current.Config.ImageConvert3d.CrossEye = imageConvert3dViewModel.CrossEye;

            var imageColorRestorationViewModel = App.Current.GetService<ViewModels.Pages.Image.ColorRestoration.IndexViewModel>();
            Current.Config.ImageColorRestoration.Input = imageColorRestorationViewModel.Input;
            Current.Config.ImageColorRestoration.Output = imageColorRestorationViewModel.Output;
            Current.Config.ImageColorRestoration.InputSort = imageColorRestorationViewModel.InputSort;
            Current.Config.ImageColorRestoration.SortRule = imageColorRestorationViewModel.SortRule;
            Current.Config.ImageColorRestoration.Provider = imageColorRestorationViewModel.Provider;
            Current.Config.ImageColorRestoration.Mode = imageColorRestorationViewModel.Mode;
            Current.Config.ImageColorRestoration.Quality = imageColorRestorationViewModel.Quality;

            //var imageFrameInterpolationViewModel = App.Current.GetService<ViewModels.Pages.Image.FrameInterpolation.IndexViewModel>();
            //Current.Config.ImageFrameInterpolation.Input = imageFrameInterpolationViewModel.Input;
            //Current.Config.ImageFrameInterpolation.Output = imageFrameInterpolationViewModel.Output;
            //Current.Config.ImageFrameInterpolation.InputSort = imageFrameInterpolationViewModel.InputSort;
            //Current.Config.ImageFrameInterpolation.SortRule = imageFrameInterpolationViewModel.SortRule;
            //Current.Config.ImageFrameInterpolation.Provider = imageFrameInterpolationViewModel.Provider;
            //Current.Config.ImageFrameInterpolation.Mode = imageFrameInterpolationViewModel.Mode;
            //Current.Config.ImageFrameInterpolation.Scale = imageFrameInterpolationViewModel.Scale;

            var imageFaceRestorationViewModel = App.Current.GetService<ViewModels.Pages.Image.FaceRestoration.IndexViewModel>();
            Current.Config.ImageFaceRestoration.Input = imageFaceRestorationViewModel.Input;
            Current.Config.ImageFaceRestoration.Output = imageFaceRestorationViewModel.Output;
            Current.Config.ImageFaceRestoration.InputSort = imageFaceRestorationViewModel.InputSort;
            Current.Config.ImageFaceRestoration.SortRule = imageFaceRestorationViewModel.SortRule;
            Current.Config.ImageFaceRestoration.Provider = imageFaceRestorationViewModel.Provider;
            Current.Config.ImageFaceRestoration.Mode = imageFaceRestorationViewModel.Mode;

            var videoSuperResolutionViewModel = App.Current.GetService<ViewModels.Pages.Video.SuperResolution.IndexViewModel>();
            Current.Config.VideoSuperResolution.Input = videoSuperResolutionViewModel.Input;
            Current.Config.VideoSuperResolution.Output = videoSuperResolutionViewModel.Output;
            Current.Config.VideoSuperResolution.InputSort = videoSuperResolutionViewModel.InputSort;
            Current.Config.VideoSuperResolution.SortRule = videoSuperResolutionViewModel.SortRule;
            Current.Config.VideoSuperResolution.Provider = videoSuperResolutionViewModel.Provider;
            Current.Config.VideoSuperResolution.Mode = videoSuperResolutionViewModel.Mode;
            Current.Config.VideoSuperResolution.Scale = videoSuperResolutionViewModel.Scale;

            //var videoAutoWipeViewModel = App.Current.GetService<ViewModels.Pages.Video.AutoWipe.IndexViewModel>();
            //Current.Config.VideoAutoWipe.Input = videoAutoWipeViewModel.Input;
            //Current.Config.VideoAutoWipe.Output = videoAutoWipeViewModel.Output;
            //Current.Config.VideoAutoWipe.InputSort = videoAutoWipeViewModel.InputSort;
            //Current.Config.VideoAutoWipe.SortRule = videoAutoWipeViewModel.SortRule;
            //Current.Config.VideoAutoWipe.Provider = videoAutoWipeViewModel.Provider;
            //Current.Config.VideoAutoWipe.Mode = videoAutoWipeViewModel.Mode;

            var videoCartoonComicViewModel = App.Current.GetService<ViewModels.Pages.Video.CartoonComic.IndexViewModel>();
            Current.Config.VideoCartoonComic.Input = videoCartoonComicViewModel.Input;
            Current.Config.VideoCartoonComic.Output = videoCartoonComicViewModel.Output;
            Current.Config.VideoCartoonComic.InputSort = videoCartoonComicViewModel.InputSort;
            Current.Config.VideoCartoonComic.SortRule = videoCartoonComicViewModel.SortRule;
            Current.Config.VideoCartoonComic.Provider = videoCartoonComicViewModel.Provider;
            Current.Config.VideoCartoonComic.Mode = videoCartoonComicViewModel.Mode;
            Current.Config.VideoCartoonComic.Quality = videoCartoonComicViewModel.Quality;

            var videoConvert3dViewModel = App.Current.GetService<ViewModels.Pages.Video.Convert3d.IndexViewModel>();
            Current.Config.VideoConvert3d.Input = videoConvert3dViewModel.Input;
            Current.Config.VideoConvert3d.Output = videoConvert3dViewModel.Output;
            Current.Config.VideoConvert3d.InputSort = videoConvert3dViewModel.InputSort;
            Current.Config.VideoConvert3d.SortRule = videoConvert3dViewModel.SortRule;
            Current.Config.VideoConvert3d.Provider = videoConvert3dViewModel.Provider;
            Current.Config.VideoConvert3d.Mode = videoConvert3dViewModel.Mode;
            Current.Config.VideoConvert3d.Format = videoConvert3dViewModel.Format;
            Current.Config.VideoConvert3d.Shift = videoConvert3dViewModel.Shift;
            Current.Config.VideoConvert3d.PopOut = videoConvert3dViewModel.PopOut;
            Current.Config.VideoConvert3d.CrossEye = videoConvert3dViewModel.CrossEye;

            var videoColorRestorationViewModel = App.Current.GetService<ViewModels.Pages.Video.ColorRestoration.IndexViewModel>();
            Current.Config.VideoColorRestoration.Input = videoColorRestorationViewModel.Input;
            Current.Config.VideoColorRestoration.Output = videoColorRestorationViewModel.Output;
            Current.Config.VideoColorRestoration.InputSort = videoColorRestorationViewModel.InputSort;
            Current.Config.VideoColorRestoration.SortRule = videoColorRestorationViewModel.SortRule;
            Current.Config.VideoColorRestoration.Provider = videoColorRestorationViewModel.Provider;
            Current.Config.VideoColorRestoration.Mode = videoColorRestorationViewModel.Mode;
            Current.Config.VideoColorRestoration.Quality = videoColorRestorationViewModel.Quality;

            var videoFrameInterpolationViewModel = App.Current.GetService<ViewModels.Pages.Video.FrameInterpolation.IndexViewModel>();
            Current.Config.VideoFrameInterpolation.Input = videoFrameInterpolationViewModel.Input;
            Current.Config.VideoFrameInterpolation.Output = videoFrameInterpolationViewModel.Output;
            Current.Config.VideoFrameInterpolation.InputSort = videoFrameInterpolationViewModel.InputSort;
            Current.Config.VideoFrameInterpolation.SortRule = videoFrameInterpolationViewModel.SortRule;
            Current.Config.VideoFrameInterpolation.Provider = videoFrameInterpolationViewModel.Provider;
            Current.Config.VideoFrameInterpolation.Mode = videoFrameInterpolationViewModel.Mode;
            Current.Config.VideoFrameInterpolation.Scale = videoFrameInterpolationViewModel.Scale;

            var videoOrganizationIndexViewMode = App.Current.GetService<ViewModels.Pages.Video.Organization.IndexViewModel>();
            Current.Config.VideoOrganization.Input = videoOrganizationIndexViewMode.Input;
            Current.Config.VideoOrganization.Output = videoOrganizationIndexViewMode.Output;
            Current.Config.VideoOrganization.InputSort = videoOrganizationIndexViewMode.InputSort;
            Current.Config.VideoOrganization.SortRule = videoOrganizationIndexViewMode.SortRule;
            Current.Config.VideoOrganization.Client = videoOrganizationIndexViewMode.Client;

            ProcessHelper.Clear();
        }

        private Task HandleActivationAsync()
        {
            if (Application.Current.Windows.OfType<MainWindow>().Any())
            {
                return Task.CompletedTask;
            }
            var mainWindow = _serviceProvider.GetRequiredService<IWindow>();
            mainWindow.Loaded += OnMainWindowLoaded;
            mainWindow?.Show();
            return Task.CompletedTask;
        }

        private void OnMainWindowLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is not MainWindow mainWindow)
            {
                return;
            }
            var settings = _serviceProvider.GetService<SettingsPageViewModel>();
            settings.Language = Current.Config.App.CurrentLanguage;
            _ = mainWindow.NavigationView.Navigate(typeof(DashboardPage));
            _ = Validate.ValidateLicense();
        }
    }
}
