using Apt.Core.Consts;
using Apt.Core.Enums;
using Apt.Core.Exceptions;
using Apt.Core.Models;
using Apt.Core.Services.Pages.Video.CartoonComic;
using Apt.Core.Utility;
using Apt.Service.Adapters.Windows;
using Apt.Service.Controls.FileGrid;
using Apt.Service.Controls.RunMessage;
using Apt.Service.Extensions;
using Apt.Service.Utility;
using Apt.Service.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui;

namespace Apt.App.ViewModels.Pages.Video.CartoonComic
{
    public partial class IndexPageViewModel : CommonViewModel
    {
        private IndexService _indexService = null!;

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<string>> _providerSource = [];

        [ObservableProperty]
        private ComBoBoxItem<string> _providerItem = null!;

        public string Provider
        {
            get => ProviderItem.Value;
            set => ProviderItem = ProviderSource.First(e => e.Value == value);
        }

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<string>> _modeSource = [];

        [ObservableProperty]
        private ComBoBoxItem<string> _modeItem = null!;

        public string Mode
        {
            get => ModeItem.Value;
            set => ModeItem = ModeSource.First(e => e.Value == value);
        }

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<string>> _qualitySource = [];

        [ObservableProperty]
        private ComBoBoxItem<string> _qualityItem = null!;

        public string Quality
        {
            get => QualityItem.Value;
            set => QualityItem = QualitySource.First(e => e.Value == value);
        }

        public override void OnInputChangedAction(string value) => GetFileGrids(AppConst.VideoExts);

        public override void OnOutputChangedAction(string value) => GetFileGrids(AppConst.VideoExts);

        public override void OnFileGridSwitchChangedAction(bool value) => GetFileGrids(AppConst.VideoExts);

        [ObservableProperty]
        private Uri? _fileViewSource = null!;

        public override void OnFileGridItemChangedAction(FileModel? value) => FileViewSource = Source.FileToUri(value?.FileInfo.FullName);

        public IndexPageViewModel(
            IServiceProvider serviceProvider,
            ISnackbarService snackbarService) :
            base(serviceProvider, snackbarService)
        {
            if (!IsInitialized) InitializeViewModel();
        }

        public override void InitializeViewModel()
        {
            ProviderSource = Adapter.CpuAndGpu;

            ModeSource =
            [
                new ComBoBoxItem<string>() {  Text = Language.Instance["VideoCartoonComicIndexPageModeHayao"], Value = "Hayao" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["VideoCartoonComicIndexPageModeCute"], Value = "Cute" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["VideoCartoonComicIndexPageModeJPFace"], Value = "JPFace" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["VideoCartoonComicIndexPageModeShinkai"], Value = "Shinkai" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["VideoCartoonComicIndexPageModeSketch"], Value = "Sketch" }
            ];
            QualitySource =
            [
                new ComBoBoxItem<string>() {  Text = Language.Instance["VideoCartoonComicIndexPageQualityAuto"], Value = "Auto" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["VideoCartoonComicIndexPageQualityHigh"], Value = "High" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["VideoCartoonComicIndexPageQualityMedium"], Value = "Medium" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["VideoCartoonComicIndexPageQualityLow"], Value = "Low" }
            ];

            CurrentMessage = new MessageModel(MessageType.Info, Language.Instance["VideoCartoonComicHelp"]);

            _indexService = new IndexService
            {
                ProgressMax = ProgressBarMaximum,
                Message = (type, message) => CurrentMessage = new MessageModel(type, message),
                Progress = async (process) => await AddProcess(process),
                IsStop = () => !StopEnabled
            };

            IsInitialized = true;
        }

        public override async Task Start()
        {
            try
            {
                StartEnabled = false;
                StopEnabled = true;
                OpenEnabled = true;

                FileGridSwitch = false;

                if (!Directory.Exists(Input))
                {
                    throw new Exception(Language.Instance["VideoCartoonComicIndexPageInputEmpty"]);
                }
                var inputFiles = FileGridSource.Select(e => e.FileInfo.FullName).ToArray();
                if (inputFiles.Length == 0)
                {
                    throw new Exception(Language.Instance["VideoCartoonComicIndexPageInputFilesEmpty"]);
                }
                if (!Directory.Exists(Output))
                {
                    throw new Exception(Language.Instance["VideoCartoonComicIndexPageOutputEmpty"]);
                }

                await _indexService.Start(Input, Output, inputFiles, Provider, Mode, Quality);

                SnackbarService.ShowSnackbarSuccess(Language.Instance["VideoCartoonComicIndexPageOperationCompleted"]);

                ProgressBarValue = ProgressBarMaximum;

                FileGridSwitch = true;

                if (Current.Config.App.IsAutoOpenOutput) SetOpen();
            }
            catch (ActivationException ex)
            {
                ServiceProvider.ShowLicense(ex.Message);
            }
            catch (Exception ex)
            {
                SnackbarService.ShowSnackbarError(ex.Message);
                CurrentMessage = new MessageModel(MessageType.Error, ex.Message);
            }
            finally
            {
                ProgressBarValue = 0;
                StartEnabled = true;
                StopEnabled = false;
            }
        }
    }
}