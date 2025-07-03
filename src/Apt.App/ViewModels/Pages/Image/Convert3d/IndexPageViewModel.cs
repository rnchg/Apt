using Apt.Core.Consts;
using Apt.Core.Enums;
using Apt.Core.Exceptions;
using Apt.Core.Models;
using Apt.Core.Services.Pages.Image.Convert3d;
using Apt.Core.Utility;
using Apt.Service.Adapters.Windows;
using Apt.Service.Extensions;
using Apt.Service.Utility;
using Apt.Service.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui;

namespace Apt.App.ViewModels.Pages.Image.Convert3d
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
        private ObservableCollection<ComBoBoxItem<string>> _formatSource = [];
        [ObservableProperty]
        private ComBoBoxItem<string> _formatItem = null!;

        public string Format
        {
            get => FormatItem.Value;
            set => FormatItem = FormatSource.First(e => e.Value == value);
        }

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<int>> _shiftSource = [];
        [ObservableProperty]
        private ComBoBoxItem<int> _shiftItem = null!;

        public int Shift
        {
            get => ShiftItem.Value;
            set => ShiftItem = ShiftSource.First(e => e.Value == value);
        }

        [ObservableProperty]
        private bool _popOut;

        [ObservableProperty]
        private bool _crossEye;

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
            InputExts = AppConst.ImageExts;
            OutputExts = AppConst.ImageExts;

            ProviderSource = Adapter.CpuAndGpu;

            ModeSource =
            [
                new ComBoBoxItem<string>() {  Text = Language.Instance["ImageConvert3dIndexPageModeStandard"], Value = "Standard" }
            ];
            FormatSource =
            [
                new ComBoBoxItem<string>() {  Text = Language.Instance["ImageConvert3dIndexPageFormatHalfSbs"], Value = "HalfSbs" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["ImageConvert3dIndexPageFormatSbs"], Value = "Sbs" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["ImageConvert3dIndexPageFormatAnaglyph"], Value = "Anaglyph" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["ImageConvert3dIndexPageFormatDepth"], Value = "Depth" }
            ];
            ShiftSource =
            [
                new ComBoBoxItem<int>() {  Text = Language.Instance["ImageConvert3dIndexPageShift10"], Value = 10 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["ImageConvert3dIndexPageShift20"], Value = 20 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["ImageConvert3dIndexPageShift30"], Value = 30 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["ImageConvert3dIndexPageShift50"], Value = 50 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["ImageConvert3dIndexPageShift100"], Value = 100 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["ImageConvert3dIndexPageShift200"], Value = 200 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["ImageConvert3dIndexPageShift300"], Value = 300 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["ImageConvert3dIndexPageShift500"], Value = 500 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["ImageConvert3dIndexPageShift1000"], Value = 1000 }
            ];

            AddMessage(MessageType.Success, Language.Instance["ImageConvert3dHelp"]);

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
                    throw new Exception(Language.Instance["ImageConvert3dIndexPageInputError"]);
                }
                if (!Directory.Exists(Output))
                {
                    throw new Exception(Language.Instance["ImageConvert3dIndexPageOutputError"]);
                }
                var inputFiles = FileGridSource.Select(e => e.FullName).ToArray();
                if (inputFiles.Length == 0)
                {
                    throw new Exception(Language.Instance["ImageConvert3dIndexPageFileError"]);
                }

                await _indexService.StartAsync(Input, Output, inputFiles, Provider, Mode, Format, Shift, PopOut, CrossEye);

                SnackbarService.ShowSnackbarSuccess(Language.Instance["ImageConvert3dIndexPageProcessEnd"]);

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