using Apt.Core.Utility;

namespace Apt.App.Extensions
{
    public static class ProviderExtension
    {
        public static void GetConfig(this IServiceProvider provider)
        {
            var imageSuperResolutionViewModel = provider.GetRequiredService<ViewModels.Pages.Image.SuperResolution.IndexPageViewModel>();
            imageSuperResolutionViewModel.Input = Current.Config.ImageSuperResolution.Input;
            imageSuperResolutionViewModel.Output = Current.Config.ImageSuperResolution.Output;
            imageSuperResolutionViewModel.Provider = Current.Config.ImageSuperResolution.Provider;
            imageSuperResolutionViewModel.Mode = Current.Config.ImageSuperResolution.Mode;
            imageSuperResolutionViewModel.Scale = Current.Config.ImageSuperResolution.Scale;

            var imageAutoWipeViewModel = provider.GetRequiredService<ViewModels.Pages.Image.AutoWipe.IndexPageViewModel>();
            imageAutoWipeViewModel.Input = Current.Config.ImageAutoWipe.Input;
            imageAutoWipeViewModel.Output = Current.Config.ImageAutoWipe.Output;
            imageAutoWipeViewModel.Provider = Current.Config.ImageAutoWipe.Provider;
            imageAutoWipeViewModel.Mode = Current.Config.ImageAutoWipe.Mode;

            var imageCartoonComicViewModel = provider.GetRequiredService<ViewModels.Pages.Image.CartoonComic.IndexPageViewModel>();
            imageCartoonComicViewModel.Input = Current.Config.ImageCartoonComic.Input;
            imageCartoonComicViewModel.Output = Current.Config.ImageCartoonComic.Output;
            imageCartoonComicViewModel.Provider = Current.Config.ImageCartoonComic.Provider;
            imageCartoonComicViewModel.Mode = Current.Config.ImageCartoonComic.Mode;
            imageCartoonComicViewModel.Quality = Current.Config.ImageCartoonComic.Quality;

            var imageConvert3dViewModel = provider.GetRequiredService<ViewModels.Pages.Image.Convert3d.IndexPageViewModel>();
            imageConvert3dViewModel.Input = Current.Config.ImageConvert3d.Input;
            imageConvert3dViewModel.Output = Current.Config.ImageConvert3d.Output;
            imageConvert3dViewModel.Provider = Current.Config.ImageConvert3d.Provider;
            imageConvert3dViewModel.Mode = Current.Config.ImageConvert3d.Mode;
            imageConvert3dViewModel.Format = Current.Config.ImageConvert3d.Format;
            imageConvert3dViewModel.Shift = Current.Config.ImageConvert3d.Shift;
            imageConvert3dViewModel.PopOut = Current.Config.ImageConvert3d.PopOut;
            imageConvert3dViewModel.CrossEye = Current.Config.ImageConvert3d.CrossEye;

            var imageColorRestorationViewModel = provider.GetRequiredService<ViewModels.Pages.Image.ColorRestoration.IndexPageViewModel>();
            imageColorRestorationViewModel.Input = Current.Config.ImageColorRestoration.Input;
            imageColorRestorationViewModel.Output = Current.Config.ImageColorRestoration.Output;
            imageColorRestorationViewModel.Provider = Current.Config.ImageColorRestoration.Provider;
            imageColorRestorationViewModel.Mode = Current.Config.ImageColorRestoration.Mode;
            imageColorRestorationViewModel.Quality = Current.Config.ImageColorRestoration.Quality;

            var imageFrameInterpolationViewModel = provider.GetRequiredService<ViewModels.Pages.Image.FrameInterpolation.IndexPageViewModel>();
            imageFrameInterpolationViewModel.Input = Current.Config.ImageFrameInterpolation.Input;
            imageFrameInterpolationViewModel.Output = Current.Config.ImageFrameInterpolation.Output;
            imageFrameInterpolationViewModel.Provider = Current.Config.ImageFrameInterpolation.Provider;
            imageFrameInterpolationViewModel.Mode = Current.Config.ImageFrameInterpolation.Mode;
            imageFrameInterpolationViewModel.Scale = Current.Config.ImageFrameInterpolation.Scale;

            var imageMattingViewModel = provider.GetRequiredService<ViewModels.Pages.Image.Matting.IndexPageViewModel>();
            imageMattingViewModel.Input = Current.Config.ImageMatting.Input;
            imageMattingViewModel.Output = Current.Config.ImageMatting.Output;
            imageMattingViewModel.Provider = Current.Config.ImageMatting.Provider;
            imageMattingViewModel.Mode = Current.Config.ImageMatting.Mode;

            var imageFaceRestorationViewModel = provider.GetRequiredService<ViewModels.Pages.Image.FaceRestoration.IndexPageViewModel>();
            imageFaceRestorationViewModel.Input = Current.Config.ImageFaceRestoration.Input;
            imageFaceRestorationViewModel.Output = Current.Config.ImageFaceRestoration.Output;
            imageFaceRestorationViewModel.Provider = Current.Config.ImageFaceRestoration.Provider;
            imageFaceRestorationViewModel.Mode = Current.Config.ImageFaceRestoration.Mode;

            var videoSuperResolutionViewModel = provider.GetRequiredService<ViewModels.Pages.Video.SuperResolution.IndexPageViewModel>();
            videoSuperResolutionViewModel.Input = Current.Config.VideoSuperResolution.Input;
            videoSuperResolutionViewModel.Output = Current.Config.VideoSuperResolution.Output;
            videoSuperResolutionViewModel.Provider = Current.Config.VideoSuperResolution.Provider;
            videoSuperResolutionViewModel.Mode = Current.Config.VideoSuperResolution.Mode;
            videoSuperResolutionViewModel.Scale = Current.Config.VideoSuperResolution.Scale;

            var videoAutoWipeViewModel = provider.GetRequiredService<ViewModels.Pages.Video.AutoWipe.IndexPageViewModel>();
            videoAutoWipeViewModel.Input = Current.Config.VideoAutoWipe.Input;
            videoAutoWipeViewModel.Output = Current.Config.VideoAutoWipe.Output;
            videoAutoWipeViewModel.Provider = Current.Config.VideoAutoWipe.Provider;
            videoAutoWipeViewModel.Mode = Current.Config.VideoAutoWipe.Mode;

            var videoCartoonComicViewModel = provider.GetRequiredService<ViewModels.Pages.Video.CartoonComic.IndexPageViewModel>();
            videoCartoonComicViewModel.Input = Current.Config.VideoCartoonComic.Input;
            videoCartoonComicViewModel.Output = Current.Config.VideoCartoonComic.Output;
            videoCartoonComicViewModel.Provider = Current.Config.VideoCartoonComic.Provider;
            videoCartoonComicViewModel.Mode = Current.Config.VideoCartoonComic.Mode;
            videoCartoonComicViewModel.Quality = Current.Config.VideoCartoonComic.Quality;

            var videoConvert3dViewModel = provider.GetRequiredService<ViewModels.Pages.Video.Convert3d.IndexPageViewModel>();
            videoConvert3dViewModel.Input = Current.Config.VideoConvert3d.Input;
            videoConvert3dViewModel.Output = Current.Config.VideoConvert3d.Output;
            videoConvert3dViewModel.Provider = Current.Config.VideoConvert3d.Provider;
            videoConvert3dViewModel.Mode = Current.Config.VideoConvert3d.Mode;
            videoConvert3dViewModel.Format = Current.Config.VideoConvert3d.Format;
            videoConvert3dViewModel.Shift = Current.Config.VideoConvert3d.Shift;
            videoConvert3dViewModel.PopOut = Current.Config.VideoConvert3d.PopOut;
            videoConvert3dViewModel.CrossEye = Current.Config.VideoConvert3d.CrossEye;

            var videoColorRestorationViewModel = provider.GetRequiredService<ViewModels.Pages.Video.ColorRestoration.IndexPageViewModel>();
            videoColorRestorationViewModel.Input = Current.Config.VideoColorRestoration.Input;
            videoColorRestorationViewModel.Output = Current.Config.VideoColorRestoration.Output;
            videoColorRestorationViewModel.Provider = Current.Config.VideoColorRestoration.Provider;
            videoColorRestorationViewModel.Mode = Current.Config.VideoColorRestoration.Mode;
            videoColorRestorationViewModel.Quality = Current.Config.VideoColorRestoration.Quality;

            var videoFrameInterpolationViewModel = provider.GetRequiredService<ViewModels.Pages.Video.FrameInterpolation.IndexPageViewModel>();
            videoFrameInterpolationViewModel.Input = Current.Config.VideoFrameInterpolation.Input;
            videoFrameInterpolationViewModel.Output = Current.Config.VideoFrameInterpolation.Output;
            videoFrameInterpolationViewModel.Provider = Current.Config.VideoFrameInterpolation.Provider;
            videoFrameInterpolationViewModel.Mode = Current.Config.VideoFrameInterpolation.Mode;
            videoFrameInterpolationViewModel.Scale = Current.Config.VideoFrameInterpolation.Scale;

            var videoMattingViewModel = provider.GetRequiredService<ViewModels.Pages.Video.Matting.IndexPageViewModel>();
            videoMattingViewModel.Input = Current.Config.VideoMatting.Input;
            videoMattingViewModel.Output = Current.Config.VideoMatting.Output;
            videoMattingViewModel.Provider = Current.Config.VideoMatting.Provider;
            videoMattingViewModel.Mode = Current.Config.VideoMatting.Mode;

            var videoOrganizationIndexViewMode = provider.GetRequiredService<ViewModels.Pages.Video.Organization.IndexPageViewModel>();
            videoOrganizationIndexViewMode.Input = Current.Config.VideoOrganization.Input;
            videoOrganizationIndexViewMode.Output = Current.Config.VideoOrganization.Output;
            videoOrganizationIndexViewMode.Client = Current.Config.VideoOrganization.Client;
        }

        public static void SetConfig(this IServiceProvider provider)
        {
            var imageSuperResolutionViewModel = provider.GetRequiredService<ViewModels.Pages.Image.SuperResolution.IndexPageViewModel>();
            Current.Config.ImageSuperResolution.Input = imageSuperResolutionViewModel.Input;
            Current.Config.ImageSuperResolution.Output = imageSuperResolutionViewModel.Output;
            Current.Config.ImageSuperResolution.Provider = imageSuperResolutionViewModel.Provider;
            Current.Config.ImageSuperResolution.Mode = imageSuperResolutionViewModel.Mode;
            Current.Config.ImageSuperResolution.Scale = imageSuperResolutionViewModel.Scale;

            var imageAutoWipeViewModel = provider.GetRequiredService<ViewModels.Pages.Image.AutoWipe.IndexPageViewModel>();
            Current.Config.ImageAutoWipe.Input = imageAutoWipeViewModel.Input;
            Current.Config.ImageAutoWipe.Output = imageAutoWipeViewModel.Output;
            Current.Config.ImageAutoWipe.Provider = imageAutoWipeViewModel.Provider;
            Current.Config.ImageAutoWipe.Mode = imageAutoWipeViewModel.Mode;

            var imageCartoonComicViewModel = provider.GetRequiredService<ViewModels.Pages.Image.CartoonComic.IndexPageViewModel>();
            Current.Config.ImageCartoonComic.Input = imageCartoonComicViewModel.Input;
            Current.Config.ImageCartoonComic.Output = imageCartoonComicViewModel.Output;
            Current.Config.ImageCartoonComic.Provider = imageCartoonComicViewModel.Provider;
            Current.Config.ImageCartoonComic.Mode = imageCartoonComicViewModel.Mode;
            Current.Config.ImageCartoonComic.Quality = imageCartoonComicViewModel.Quality;

            var imageConvert3dViewModel = provider.GetRequiredService<ViewModels.Pages.Image.Convert3d.IndexPageViewModel>();
            Current.Config.ImageConvert3d.Input = imageConvert3dViewModel.Input;
            Current.Config.ImageConvert3d.Output = imageConvert3dViewModel.Output;
            Current.Config.ImageConvert3d.Provider = imageConvert3dViewModel.Provider;
            Current.Config.ImageConvert3d.Mode = imageConvert3dViewModel.Mode;
            Current.Config.ImageConvert3d.Format = imageConvert3dViewModel.Format;
            Current.Config.ImageConvert3d.Shift = imageConvert3dViewModel.Shift;
            Current.Config.ImageConvert3d.PopOut = imageConvert3dViewModel.PopOut;
            Current.Config.ImageConvert3d.CrossEye = imageConvert3dViewModel.CrossEye;

            var imageColorRestorationViewModel = provider.GetRequiredService<ViewModels.Pages.Image.ColorRestoration.IndexPageViewModel>();
            Current.Config.ImageColorRestoration.Input = imageColorRestorationViewModel.Input;
            Current.Config.ImageColorRestoration.Output = imageColorRestorationViewModel.Output;
            Current.Config.ImageColorRestoration.Provider = imageColorRestorationViewModel.Provider;
            Current.Config.ImageColorRestoration.Mode = imageColorRestorationViewModel.Mode;
            Current.Config.ImageColorRestoration.Quality = imageColorRestorationViewModel.Quality;

            var imageFrameInterpolationViewModel = provider.GetRequiredService<ViewModels.Pages.Image.FrameInterpolation.IndexPageViewModel>();
            Current.Config.ImageFrameInterpolation.Input = imageFrameInterpolationViewModel.Input;
            Current.Config.ImageFrameInterpolation.Output = imageFrameInterpolationViewModel.Output;
            Current.Config.ImageFrameInterpolation.Provider = imageFrameInterpolationViewModel.Provider;
            Current.Config.ImageFrameInterpolation.Mode = imageFrameInterpolationViewModel.Mode;
            Current.Config.ImageFrameInterpolation.Scale = imageFrameInterpolationViewModel.Scale;

            var imageMattingViewModel = provider.GetRequiredService<ViewModels.Pages.Image.Matting.IndexPageViewModel>();
            Current.Config.ImageMatting.Input = imageMattingViewModel.Input;
            Current.Config.ImageMatting.Output = imageMattingViewModel.Output;
            Current.Config.ImageMatting.Provider = imageMattingViewModel.Provider;
            Current.Config.ImageMatting.Mode = imageMattingViewModel.Mode;

            var imageFaceRestorationViewModel = provider.GetRequiredService<ViewModels.Pages.Image.FaceRestoration.IndexPageViewModel>();
            Current.Config.ImageFaceRestoration.Input = imageFaceRestorationViewModel.Input;
            Current.Config.ImageFaceRestoration.Output = imageFaceRestorationViewModel.Output;
            Current.Config.ImageFaceRestoration.Provider = imageFaceRestorationViewModel.Provider;
            Current.Config.ImageFaceRestoration.Mode = imageFaceRestorationViewModel.Mode;

            var videoSuperResolutionViewModel = provider.GetRequiredService<ViewModels.Pages.Video.SuperResolution.IndexPageViewModel>();
            Current.Config.VideoSuperResolution.Input = videoSuperResolutionViewModel.Input;
            Current.Config.VideoSuperResolution.Output = videoSuperResolutionViewModel.Output;
            Current.Config.VideoSuperResolution.Provider = videoSuperResolutionViewModel.Provider;
            Current.Config.VideoSuperResolution.Mode = videoSuperResolutionViewModel.Mode;
            Current.Config.VideoSuperResolution.Scale = videoSuperResolutionViewModel.Scale;

            var videoAutoWipeViewModel = provider.GetRequiredService<ViewModels.Pages.Video.AutoWipe.IndexPageViewModel>();
            Current.Config.VideoAutoWipe.Input = videoAutoWipeViewModel.Input;
            Current.Config.VideoAutoWipe.Output = videoAutoWipeViewModel.Output;
            Current.Config.VideoAutoWipe.Provider = videoAutoWipeViewModel.Provider;
            Current.Config.VideoAutoWipe.Mode = videoAutoWipeViewModel.Mode;

            var videoCartoonComicViewModel = provider.GetRequiredService<ViewModels.Pages.Video.CartoonComic.IndexPageViewModel>();
            Current.Config.VideoCartoonComic.Input = videoCartoonComicViewModel.Input;
            Current.Config.VideoCartoonComic.Output = videoCartoonComicViewModel.Output;
            Current.Config.VideoCartoonComic.Provider = videoCartoonComicViewModel.Provider;
            Current.Config.VideoCartoonComic.Mode = videoCartoonComicViewModel.Mode;
            Current.Config.VideoCartoonComic.Quality = videoCartoonComicViewModel.Quality;

            var videoConvert3dViewModel = provider.GetRequiredService<ViewModels.Pages.Video.Convert3d.IndexPageViewModel>();
            Current.Config.VideoConvert3d.Input = videoConvert3dViewModel.Input;
            Current.Config.VideoConvert3d.Output = videoConvert3dViewModel.Output;
            Current.Config.VideoConvert3d.Provider = videoConvert3dViewModel.Provider;
            Current.Config.VideoConvert3d.Mode = videoConvert3dViewModel.Mode;
            Current.Config.VideoConvert3d.Format = videoConvert3dViewModel.Format;
            Current.Config.VideoConvert3d.Shift = videoConvert3dViewModel.Shift;
            Current.Config.VideoConvert3d.PopOut = videoConvert3dViewModel.PopOut;
            Current.Config.VideoConvert3d.CrossEye = videoConvert3dViewModel.CrossEye;

            var videoColorRestorationViewModel = provider.GetRequiredService<ViewModels.Pages.Video.ColorRestoration.IndexPageViewModel>();
            Current.Config.VideoColorRestoration.Input = videoColorRestorationViewModel.Input;
            Current.Config.VideoColorRestoration.Output = videoColorRestorationViewModel.Output;
            Current.Config.VideoColorRestoration.Provider = videoColorRestorationViewModel.Provider;
            Current.Config.VideoColorRestoration.Mode = videoColorRestorationViewModel.Mode;
            Current.Config.VideoColorRestoration.Quality = videoColorRestorationViewModel.Quality;

            var videoFrameInterpolationViewModel = provider.GetRequiredService<ViewModels.Pages.Video.FrameInterpolation.IndexPageViewModel>();
            Current.Config.VideoFrameInterpolation.Input = videoFrameInterpolationViewModel.Input;
            Current.Config.VideoFrameInterpolation.Output = videoFrameInterpolationViewModel.Output;
            Current.Config.VideoFrameInterpolation.Provider = videoFrameInterpolationViewModel.Provider;
            Current.Config.VideoFrameInterpolation.Mode = videoFrameInterpolationViewModel.Mode;
            Current.Config.VideoFrameInterpolation.Scale = videoFrameInterpolationViewModel.Scale;

            var videoMattingViewModel = provider.GetRequiredService<ViewModels.Pages.Video.Matting.IndexPageViewModel>();
            Current.Config.VideoMatting.Input = videoMattingViewModel.Input;
            Current.Config.VideoMatting.Output = videoMattingViewModel.Output;
            Current.Config.VideoMatting.Provider = videoMattingViewModel.Provider;
            Current.Config.VideoMatting.Mode = videoMattingViewModel.Mode;

            var videoOrganizationIndexViewMode = provider.GetRequiredService<ViewModels.Pages.Video.Organization.IndexPageViewModel>();
            Current.Config.VideoOrganization.Input = videoOrganizationIndexViewMode.Input;
            Current.Config.VideoOrganization.Output = videoOrganizationIndexViewMode.Output;
            Current.Config.VideoOrganization.Client = videoOrganizationIndexViewMode.Client;
        }
    }
}
