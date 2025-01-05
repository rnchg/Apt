using Apt.App.Interfaces;
using Apt.App.Services;
using Apt.App.ViewModels.Windows.App;
using Apt.App.Views.Windows.App;

namespace Apt.App.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            Service.Extensions.ServiceExtension.AddService(services);

            services.AddHostedService<AppHostService>();

            services.AddSingleton<IWindow, MainWindow>();
            services.AddSingleton<MainWindowViewModel>();

            services.AddTransient<Views.Windows.Gen.Chat.ConfigWindow>();

            services.AddSingleton<Views.Pages.App.DashboardPage>();
            services.AddSingleton<Views.Pages.App.InfoPage>();
            services.AddSingleton<Views.Pages.App.SettingsPage>();
            services.AddSingleton<Views.Pages.Gen.Chat.IndexPage>();
            services.AddSingleton<Views.Pages.Image.SuperResolution.IndexPage>();
            services.AddSingleton<Views.Pages.Image.AutoWipe.IndexPage>();
            services.AddSingleton<Views.Pages.Image.CartoonComic.IndexPage>();
            services.AddSingleton<Views.Pages.Image.Convert3d.IndexPage>();
            services.AddSingleton<Views.Pages.Image.ColorRestoration.IndexPage>();
            services.AddSingleton<Views.Pages.Image.FrameInterpolation.IndexPage>();
            services.AddSingleton<Views.Pages.Image.Matting.IndexPage>();
            services.AddSingleton<Views.Pages.Image.FaceRestoration.IndexPage>();
            services.AddSingleton<Views.Pages.Video.SuperResolution.IndexPage>();
            services.AddSingleton<Views.Pages.Video.AutoWipe.IndexPage>();
            services.AddSingleton<Views.Pages.Video.CartoonComic.IndexPage>();
            services.AddSingleton<Views.Pages.Video.Convert3d.IndexPage>();
            services.AddSingleton<Views.Pages.Video.ColorRestoration.IndexPage>();
            services.AddSingleton<Views.Pages.Video.FrameInterpolation.IndexPage>();
            services.AddSingleton<Views.Pages.Video.Matting.IndexPage>();
            services.AddSingleton<Views.Pages.Video.Organization.IndexPage>();
            services.AddSingleton<Views.Pages.Audio.Denoise.IndexPage>();
            services.AddSingleton<Views.Pages.Audio.VocalSplit.IndexPage>();

            services.AddTransient<ViewModels.Windows.Gen.Chat.ConfigWindowViewModel>();

            services.AddSingleton<ViewModels.Pages.App.DashboardPageViewModel>();
            services.AddSingleton<ViewModels.Pages.App.InfoPageViewModel>();
            services.AddSingleton<ViewModels.Pages.App.SettingsPageViewModel>();
            services.AddSingleton<ViewModels.Pages.Gen.Chat.IndexPageViewModel>();
            services.AddSingleton<ViewModels.Pages.Image.SuperResolution.IndexPageViewModel>();
            services.AddSingleton<ViewModels.Pages.Image.AutoWipe.IndexPageViewModel>();
            services.AddSingleton<ViewModels.Pages.Image.CartoonComic.IndexPageViewModel>();
            services.AddSingleton<ViewModels.Pages.Image.Convert3d.IndexPageViewModel>();
            services.AddSingleton<ViewModels.Pages.Image.ColorRestoration.IndexPageViewModel>();
            services.AddSingleton<ViewModels.Pages.Image.FrameInterpolation.IndexPageViewModel>();
            services.AddSingleton<ViewModels.Pages.Image.Matting.IndexPageViewModel>();
            services.AddSingleton<ViewModels.Pages.Image.FaceRestoration.IndexPageViewModel>();
            services.AddSingleton<ViewModels.Pages.Video.SuperResolution.IndexPageViewModel>();
            services.AddSingleton<ViewModels.Pages.Video.AutoWipe.IndexPageViewModel>();
            services.AddSingleton<ViewModels.Pages.Video.CartoonComic.IndexPageViewModel>();
            services.AddSingleton<ViewModels.Pages.Video.Convert3d.IndexPageViewModel>();
            services.AddSingleton<ViewModels.Pages.Video.ColorRestoration.IndexPageViewModel>();
            services.AddSingleton<ViewModels.Pages.Video.FrameInterpolation.IndexPageViewModel>();
            services.AddSingleton<ViewModels.Pages.Video.Matting.IndexPageViewModel>();
            services.AddSingleton<ViewModels.Pages.Video.Organization.IndexPageViewModel>();
            services.AddSingleton<ViewModels.Pages.Audio.Denoise.IndexPageViewModel>();
            services.AddSingleton<ViewModels.Pages.Audio.VocalSplit.IndexPageViewModel>();

            return services;
        }
    }
}
