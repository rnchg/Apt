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
            Language.Instance.UpdateLanguage(Current.Config.App.Language);

            var imageSuperResolutionViewModel = App.Current.GetService<ViewModels.Pages.Image.SuperResolution.IndexViewModel>();
            imageSuperResolutionViewModel.Input = Current.Config.ImageSuperResolution.Input;
            imageSuperResolutionViewModel.Output = Current.Config.ImageSuperResolution.Output;
            imageSuperResolutionViewModel.InputSort = Current.Config.ImageSuperResolution.InputSort;
            imageSuperResolutionViewModel.SortRule = Current.Config.ImageSuperResolution.SortRule;
            imageSuperResolutionViewModel.Provider = Current.Config.ImageSuperResolution.Provider;
            imageSuperResolutionViewModel.Mode = Current.Config.ImageSuperResolution.Mode;
            imageSuperResolutionViewModel.Scale = Current.Config.ImageSuperResolution.Scale;

            var imageCartoonComicViewModel = App.Current.GetService<ViewModels.Pages.Image.CartoonComic.IndexViewModel>();
            imageCartoonComicViewModel.Input = Current.Config.ImageCartoonComic.Input;
            imageCartoonComicViewModel.Output = Current.Config.ImageCartoonComic.Output;
            imageCartoonComicViewModel.InputSort = Current.Config.ImageCartoonComic.InputSort;
            imageCartoonComicViewModel.SortRule = Current.Config.ImageCartoonComic.SortRule;
            imageCartoonComicViewModel.Provider = Current.Config.ImageCartoonComic.Provider;
            imageCartoonComicViewModel.Mode = Current.Config.ImageCartoonComic.Mode;
            imageCartoonComicViewModel.Quality = Current.Config.ImageCartoonComic.Quality;

            var imageAutoWipeViewModel = App.Current.GetService<ViewModels.Pages.Image.AutoWipe.IndexViewModel>();
            imageAutoWipeViewModel.Input = Current.Config.ImageAutoWipe.Input;
            imageAutoWipeViewModel.Output = Current.Config.ImageAutoWipe.Output;
            imageAutoWipeViewModel.InputSort = Current.Config.ImageAutoWipe.InputSort;
            imageAutoWipeViewModel.SortRule = Current.Config.ImageAutoWipe.SortRule;
            imageAutoWipeViewModel.Provider = Current.Config.ImageAutoWipe.Provider;
            imageAutoWipeViewModel.Mode = Current.Config.ImageAutoWipe.Mode;

            var imageFaceRestorationViewModel = App.Current.GetService<ViewModels.Pages.Image.FaceRestoration.IndexViewModel>();
            imageFaceRestorationViewModel.Input = Current.Config.ImageFaceRestoration.Input;
            imageFaceRestorationViewModel.Output = Current.Config.ImageFaceRestoration.Output;
            imageFaceRestorationViewModel.InputSort = Current.Config.ImageFaceRestoration.InputSort;
            imageFaceRestorationViewModel.SortRule = Current.Config.ImageFaceRestoration.SortRule;
            imageFaceRestorationViewModel.Provider = Current.Config.ImageFaceRestoration.Provider;
            imageFaceRestorationViewModel.Mode = Current.Config.ImageFaceRestoration.Mode;

            var videoOrganizationIndexViewMode = App.Current.GetService<ViewModels.Pages.Video.Organization.IndexViewModel>();
            videoOrganizationIndexViewMode.Input = Current.Config.VideoOrganization.Input;
            videoOrganizationIndexViewMode.Output = Current.Config.VideoOrganization.Output;
            videoOrganizationIndexViewMode.InputSort = Current.Config.VideoOrganization.InputSort;
            videoOrganizationIndexViewMode.SortRule = Current.Config.VideoOrganization.SortRule;
            videoOrganizationIndexViewMode.Client = Current.Config.VideoOrganization.Client;

            var videoFrameInterpolationViewModel = App.Current.GetService<ViewModels.Pages.Video.FrameInterpolation.IndexViewModel>();
            videoFrameInterpolationViewModel.Input = Current.Config.VideoFrameInterpolation.Input;
            videoFrameInterpolationViewModel.Output = Current.Config.VideoFrameInterpolation.Output;
            videoFrameInterpolationViewModel.InputSort = Current.Config.VideoFrameInterpolation.InputSort;
            videoFrameInterpolationViewModel.SortRule = Current.Config.VideoFrameInterpolation.SortRule;
            videoFrameInterpolationViewModel.Provider = Current.Config.VideoFrameInterpolation.Provider;
            videoFrameInterpolationViewModel.Scale = Current.Config.VideoFrameInterpolation.Scale;

            var videoSuperResolutionViewModel = App.Current.GetService<ViewModels.Pages.Video.SuperResolution.IndexViewModel>();
            videoSuperResolutionViewModel.Input = Current.Config.VideoSuperResolution.Input;
            videoSuperResolutionViewModel.Output = Current.Config.VideoSuperResolution.Output;
            videoSuperResolutionViewModel.InputSort = Current.Config.VideoSuperResolution.InputSort;
            videoSuperResolutionViewModel.SortRule = Current.Config.VideoSuperResolution.SortRule;
            videoSuperResolutionViewModel.Provider = Current.Config.VideoSuperResolution.Provider;
            videoSuperResolutionViewModel.Mode = Current.Config.VideoSuperResolution.Mode;
            videoSuperResolutionViewModel.Scale = Current.Config.VideoSuperResolution.Scale;

            var videoCartoonComicViewModel = App.Current.GetService<ViewModels.Pages.Video.CartoonComic.IndexViewModel>();
            videoCartoonComicViewModel.Input = Current.Config.VideoCartoonComic.Input;
            videoCartoonComicViewModel.Output = Current.Config.VideoCartoonComic.Output;
            videoCartoonComicViewModel.InputSort = Current.Config.VideoCartoonComic.InputSort;
            videoCartoonComicViewModel.SortRule = Current.Config.VideoCartoonComic.SortRule;
            videoCartoonComicViewModel.Provider = Current.Config.VideoCartoonComic.Provider;
            videoCartoonComicViewModel.Mode = Current.Config.VideoCartoonComic.Mode;
            videoCartoonComicViewModel.Quality = Current.Config.VideoCartoonComic.Quality;

            var videoAutoWipeViewModel = App.Current.GetService<ViewModels.Pages.Video.AutoWipe.IndexViewModel>();
            videoAutoWipeViewModel.Input = Current.Config.VideoAutoWipe.Input;
            videoAutoWipeViewModel.Output = Current.Config.VideoAutoWipe.Output;
            videoAutoWipeViewModel.InputSort = Current.Config.VideoAutoWipe.InputSort;
            videoAutoWipeViewModel.SortRule = Current.Config.VideoAutoWipe.SortRule;
            videoAutoWipeViewModel.Provider = Current.Config.VideoAutoWipe.Provider;
            videoAutoWipeViewModel.Mode = Current.Config.VideoAutoWipe.Mode;
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

            var imageCartoonComicViewModel = App.Current.GetService<ViewModels.Pages.Image.CartoonComic.IndexViewModel>();
            Current.Config.ImageCartoonComic.Input = imageCartoonComicViewModel.Input;
            Current.Config.ImageCartoonComic.Output = imageCartoonComicViewModel.Output;
            Current.Config.ImageCartoonComic.InputSort = imageCartoonComicViewModel.InputSort;
            Current.Config.ImageCartoonComic.SortRule = imageCartoonComicViewModel.SortRule;
            Current.Config.ImageCartoonComic.Provider = imageCartoonComicViewModel.Provider;
            Current.Config.ImageCartoonComic.Mode = imageCartoonComicViewModel.Mode;
            Current.Config.ImageCartoonComic.Quality = imageCartoonComicViewModel.Quality;

            var imageAutoWipeViewModel = App.Current.GetService<ViewModels.Pages.Image.AutoWipe.IndexViewModel>();
            Current.Config.ImageAutoWipe.Input = imageAutoWipeViewModel.Input;
            Current.Config.ImageAutoWipe.Output = imageAutoWipeViewModel.Output;
            Current.Config.ImageAutoWipe.InputSort = imageAutoWipeViewModel.InputSort;
            Current.Config.ImageAutoWipe.SortRule = imageAutoWipeViewModel.SortRule;
            Current.Config.ImageAutoWipe.Provider = imageAutoWipeViewModel.Provider;
            Current.Config.ImageAutoWipe.Mode = imageAutoWipeViewModel.Mode;

            var imageFaceRestorationViewModel = App.Current.GetService<ViewModels.Pages.Image.FaceRestoration.IndexViewModel>();
            Current.Config.ImageFaceRestoration.Input = imageFaceRestorationViewModel.Input;
            Current.Config.ImageFaceRestoration.Output = imageFaceRestorationViewModel.Output;
            Current.Config.ImageFaceRestoration.InputSort = imageFaceRestorationViewModel.InputSort;
            Current.Config.ImageFaceRestoration.SortRule = imageFaceRestorationViewModel.SortRule;
            Current.Config.ImageFaceRestoration.Provider = imageFaceRestorationViewModel.Provider;
            Current.Config.ImageFaceRestoration.Mode = imageFaceRestorationViewModel.Mode;

            var videoOrganizationIndexViewMode = App.Current.GetService<ViewModels.Pages.Video.Organization.IndexViewModel>();
            Current.Config.VideoOrganization.Input = videoOrganizationIndexViewMode.Input;
            Current.Config.VideoOrganization.Output = videoOrganizationIndexViewMode.Output;
            Current.Config.VideoOrganization.InputSort = videoOrganizationIndexViewMode.InputSort;
            Current.Config.VideoOrganization.SortRule = videoOrganizationIndexViewMode.SortRule;
            Current.Config.VideoOrganization.Client = videoOrganizationIndexViewMode.Client;

            var videoFrameInterpolationViewModel = App.Current.GetService<ViewModels.Pages.Video.FrameInterpolation.IndexViewModel>();
            Current.Config.VideoFrameInterpolation.Input = videoFrameInterpolationViewModel.Input;
            Current.Config.VideoFrameInterpolation.Output = videoFrameInterpolationViewModel.Output;
            Current.Config.VideoFrameInterpolation.InputSort = videoFrameInterpolationViewModel.InputSort;
            Current.Config.VideoFrameInterpolation.SortRule = videoFrameInterpolationViewModel.SortRule;
            Current.Config.VideoFrameInterpolation.Provider = videoFrameInterpolationViewModel.Provider;
            Current.Config.VideoFrameInterpolation.Scale = videoFrameInterpolationViewModel.Scale;

            var videoSuperResolutionViewModel = App.Current.GetService<ViewModels.Pages.Video.SuperResolution.IndexViewModel>();
            Current.Config.VideoSuperResolution.Input = videoSuperResolutionViewModel.Input;
            Current.Config.VideoSuperResolution.Output = videoSuperResolutionViewModel.Output;
            Current.Config.VideoSuperResolution.InputSort = videoSuperResolutionViewModel.InputSort;
            Current.Config.VideoSuperResolution.SortRule = videoSuperResolutionViewModel.SortRule;
            Current.Config.VideoSuperResolution.Provider = videoSuperResolutionViewModel.Provider;
            Current.Config.VideoSuperResolution.Mode = videoSuperResolutionViewModel.Mode;
            Current.Config.VideoSuperResolution.Scale = videoSuperResolutionViewModel.Scale;

            var videoCartoonComicViewModel = App.Current.GetService<ViewModels.Pages.Video.CartoonComic.IndexViewModel>();
            Current.Config.VideoCartoonComic.Input = videoCartoonComicViewModel.Input;
            Current.Config.VideoCartoonComic.Output = videoCartoonComicViewModel.Output;
            Current.Config.VideoCartoonComic.InputSort = videoCartoonComicViewModel.InputSort;
            Current.Config.VideoCartoonComic.SortRule = videoCartoonComicViewModel.SortRule;
            Current.Config.VideoCartoonComic.Provider = videoCartoonComicViewModel.Provider;
            Current.Config.VideoCartoonComic.Mode = videoCartoonComicViewModel.Mode;
            Current.Config.VideoCartoonComic.Quality = videoCartoonComicViewModel.Quality;

            var videoAutoWipeViewModel = App.Current.GetService<ViewModels.Pages.Video.AutoWipe.IndexViewModel>();
            Current.Config.VideoAutoWipe.Input = videoAutoWipeViewModel.Input;
            Current.Config.VideoAutoWipe.Output = videoAutoWipeViewModel.Output;
            Current.Config.VideoAutoWipe.InputSort = videoAutoWipeViewModel.InputSort;
            Current.Config.VideoAutoWipe.SortRule = videoAutoWipeViewModel.SortRule;
            Current.Config.VideoAutoWipe.Provider = videoAutoWipeViewModel.Provider;
            Current.Config.VideoAutoWipe.Mode = videoAutoWipeViewModel.Mode;

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
            _serviceProvider.GetService<SettingsPageViewModel>().InitializeViewModel();
            _ = mainWindow.NavigationView.Navigate(typeof(DashboardPage));
            _ = Validate.ValidateLicense();
        }
    }
}
