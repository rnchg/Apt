using Apt.Core.Consts;
using Apt.Core.Enums;
using Apt.Core.Exceptions;
using Apt.Core.Models;
using Apt.Core.Services.Pages.Video.CartoonComic;
using Apt.Core.Utility;
using Apt.Service.Adapters.Windows;
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
            set => ProviderItem = ProviderSource.FirstOrDefault(e => e.Value == value) ?? ProviderSource.First();
        }

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<string>> _modeSource = [];

        [ObservableProperty]
        private ComBoBoxItem<string> _modeItem = null!;

        public string Mode
        {
            get => ModeItem.Value;
            set => ModeItem = ModeSource.FirstOrDefault(e => e.Value == value) ?? ModeSource.First();
        }

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<string>> _qualitySource = [];

        [ObservableProperty]
        private ComBoBoxItem<string> _qualityItem = null!;

        public string Quality
        {
            get => QualityItem.Value;
            set => QualityItem = QualitySource.FirstOrDefault(e => e.Value == value) ?? QualitySource.First();
        }

        public override void OnInputChangedAction(string value) => GetFileGrids();

        public override void OnOutputChangedAction(string value) => GetFileGrids();

        public override void OnFileGridInputEnableChangedAction(bool value)
        {
            base.OnFileGridInputEnableChangedAction(value);
            if (value) GetFileGrids();
        }

        public override void OnFileGridOutputEnableChangedAction(bool value)
        {
            base.OnFileGridOutputEnableChangedAction(value);
            if (value) GetFileGrids();
        }

        [ObservableProperty]
        private Uri? _fileViewSource = null!;

        public override void OnFileGridItemChangedAction(Service.Controls.FileGrid.Model? value) => FileViewSource = Source.FileToUri(value?.FullName);

        public IndexPageViewModel(
            IServiceProvider serviceProvider,
            ISnackbarService snackbarService) :
            base(serviceProvider, snackbarService)
        {
            if (!IsInitialized) InitializeViewModel();
        }

        public override void InitializeViewModel()
        {
            InputExts = AppConst.VideoExts;
            OutputExts = AppConst.VideoExts;

            ProviderSource = Adapter.CpuAndDml;

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

            AddMessage(MessageType.Success, Language.Instance["VideoCartoonComicHelp"]);

            _indexService = new IndexService
            {
                GetStop = () => !StopEnabled,
                SetProgress = SetProcess,
                AddMessage = AddMessage
            };

            IsInitialized = true;
        }

        public override async Task Start()
        {
            try
            {
                ProgressBarValue = 0;
                StartEnabled = false;
                StopEnabled = true;

                FileGridInputEnable = true;
                FileGridOutputEnable = false;

                if (!Directory.Exists(Input))
                {
                    throw new Exception(Language.Instance["VideoCartoonComicIndexPageInputError"]);
                }
                if (!Directory.Exists(Output))
                {
                    throw new Exception(Language.Instance["VideoCartoonComicIndexPageOutputError"]);
                }
                var inputFiles = FileGridSource.Select(e => e.FullName).ToArray();
                if (inputFiles.Length == 0)
                {
                    throw new Exception(Language.Instance["VideoCartoonComicIndexPageFileError"]);
                }

                await _indexService.StartAsync(Input, Output, inputFiles, Provider, Mode, Quality);

                SnackbarService.ShowSnackbarSuccess(Language.Instance["VideoCartoonComicIndexPageProcessEnd"]);

                FileGridInputEnable = false;
                FileGridOutputEnable = true;
            }
            catch (ActivationException ex)
            {
                ServiceProvider.ShowLicense(ex.Message);
            }
            catch (Exception ex)
            {
                SnackbarService.ShowSnackbarError(ex.Message);
                AddMessage(MessageType.Error, ex.Message);
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