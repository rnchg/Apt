using Apt.Core.Consts;
using Apt.Core.Exceptions;
using Apt.Core.Models;
using Apt.Core.Services.Pages.Video.Convert3d;
using Apt.Core.Utility;
using Apt.Service.Adapters.Windows;
using Apt.Service.Controls.FileGrid;
using Apt.Service.Extensions;
using Apt.Service.Utility;
using Apt.Service.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui;

namespace Apt.App.ViewModels.Pages.Video.Convert3d
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

        public override void OnInputChangedAction(string value) => GetGridFiles(AppConst.VideoExts);

        public override void OnOutputChangedAction(string value) => GetGridFiles(AppConst.VideoExts);

        public override void OnGridFileSwitchChangedAction(bool value) => GetGridFiles(AppConst.VideoExts);

        [ObservableProperty]
        private Uri? _gridFileView = null!;

        public override void OnGridFileItemChangedAction(FileModel? value) => GridFileView = Source.VideoToUri(value?.FileInfo.FullName);

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
                new ComBoBoxItem<string>() {  Text = Language.Instance["VideoConvert3dIndexPageModeStandard"], Value = "Standard" }
            ];
            FormatSource =
            [
                new ComBoBoxItem<string>() {  Text = Language.Instance["VideoConvert3dIndexPageFormatHalfSbs"], Value = "HalfSbs" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["VideoConvert3dIndexPageFormatSbs"], Value = "Sbs" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["VideoConvert3dIndexPageFormatAnaglyph"], Value = "Anaglyph" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["VideoConvert3dIndexPageFormatDepth"], Value = "Depth" }
            ];
            ShiftSource =
            [
                new ComBoBoxItem<int>() {  Text = Language.Instance["VideoConvert3dIndexPageShift10"], Value = 10 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["VideoConvert3dIndexPageShift20"], Value = 20 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["VideoConvert3dIndexPageShift30"], Value = 30 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["VideoConvert3dIndexPageShift50"], Value = 50 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["VideoConvert3dIndexPageShift100"], Value = 100 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["VideoConvert3dIndexPageShift200"], Value = 200 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["VideoConvert3dIndexPageShift300"], Value = 300 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["VideoConvert3dIndexPageShift500"], Value = 500 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["VideoConvert3dIndexPageShift1000"], Value = 1000 }
            ];

            _indexService = new IndexService
            {
                ProgressMax = ProgressBarMaximum,
                Message = async (type, message) => await Message.AddMessage(type, message, MessageAction),
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

                GridFileSwitch = false;

                if (!Directory.Exists(Input))
                {
                    throw new Exception(Language.Instance["VideoConvert3dIndexPageInputEmpty"]);
                }
                var inputFiles = GridFileSource.Select(e => e.FileInfo.FullName).ToArray();
                if (inputFiles.Length == 0)
                {
                    throw new Exception(Language.Instance["VideoConvert3dIndexPageInputFilesEmpty"]);
                }
                if (!Directory.Exists(Output))
                {
                    throw new Exception(Language.Instance["VideoConvert3dIndexPageOutputEmpty"]);
                }

                await _indexService.Start(Input, Output, inputFiles, Provider, Mode, Format, Shift, PopOut, CrossEye);

                SnackbarService.ShowSnackbarSuccess(Language.Instance["VideoConvert3dIndexPageOperationCompleted"]);

                ProgressBarValue = ProgressBarMaximum;

                GridFileSwitch = true;

                if (Current.Config.App.IsAutoOpenOutput) SetOpen();
            }
            catch (ActivationException ex)
            {
                ServiceProvider.ShowLicense(ex.Message);
            }
            catch (Exception ex)
            {
                SnackbarService.ShowSnackbarError(ex.Message);
                await Message.AddMessageError(ex.Message, MessageAction);
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