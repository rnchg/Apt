using APT.Core.Consts;
using APT.Core.Enums;
using APT.Core.Exceptions;
using APT.Core.Services.Pages.Video.Convert3d;
using APT.Core.Utility;
using APT.Service.Adapters.Windows;
using APT.Service.Enums;
using APT.Service.Extensions;
using APT.Service.Utility;
using APT.Service.ViewModels.Base;
using Common.WindowsDesktop.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui;

namespace APT.App.ViewModels.Pages.Video.Convert3d
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
        private ObservableCollection<ComBoBoxItem<string>> _formatSource = [];

        [ObservableProperty]
        private ComBoBoxItem<string> _formatItem = null!;

        public string Format
        {
            get => FormatItem.Value;
            set => FormatItem = FormatSource.FirstOrDefault(e => e.Value == value) ?? FormatSource.First();
        }

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<int>> _shiftSource = [];
        [ObservableProperty]
        private ComBoBoxItem<int> _shiftItem = null!;

        public int Shift
        {
            get => ShiftItem.Value;
            set => ShiftItem = ShiftSource.FirstOrDefault(e => e.Value == value) ?? ShiftSource.First();
        }

        [ObservableProperty]
        private bool _popOut;

        [ObservableProperty]
        private bool _crossEye;

        [ObservableProperty]
        private Uri? _fileViewItem = null!;

        public override void OnInputChangedAction(string value) => GetFileGrids();

        public override void OnOutputChangedAction(string value) => GetFileGrids();

        public override void OnFileGridSwitchItemChangedAction(FileSwitch value) => GetFileGrids();

        public override void OnFileGridTableItemChangedAction(Service.Controls.FileGrid.Model? value) => FileViewItem = Source.FileToUri(value?.FullName);

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
                new ComBoBoxItem<string>() {  Text = Language.Instance["Video.Convert3d.ModeStandard"], Value = "Standard" }
            ];
            FormatSource =
            [
                new ComBoBoxItem<string>() {  Text = Language.Instance["Video.Convert3d.FormatHalfSbs"], Value = "HalfSbs" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Video.Convert3d.FormatSbs"], Value = "Sbs" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Video.Convert3d.FormatAnaglyph"], Value = "Anaglyph" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Video.Convert3d.FormatDepth"], Value = "Depth" }
            ];
            ShiftSource =
            [
                new ComBoBoxItem<int>() {  Text = Language.Instance["Video.Convert3d.Shift10"], Value = 10 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["Video.Convert3d.Shift20"], Value = 20 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["Video.Convert3d.Shift30"], Value = 30 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["Video.Convert3d.Shift50"], Value = 50 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["Video.Convert3d.Shift100"], Value = 100 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["Video.Convert3d.Shift200"], Value = 200 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["Video.Convert3d.Shift300"], Value = 300 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["Video.Convert3d.Shift500"], Value = 500 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["Video.Convert3d.Shift1000"], Value = 1000 }
            ];

            FileGridSwitchItem = FileSwitch.Input;

            AddMessage(MessageType.Success, Language.Instance["Video.Convert3d.Help"]);

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

                FileGridSwitchItem = FileSwitch.Input;

                if (!Directory.Exists(Input))
                {
                    throw new Exception(Language.Instance["Video.Convert3d.InputError"]);
                }
                if (!Directory.Exists(Output))
                {
                    throw new Exception(Language.Instance["Video.Convert3d.OutputError"]);
                }
                var inputFiles = FileGridTableList.Select(e => e.FullName).ToArray();
                if (inputFiles.Length == 0)
                {
                    throw new Exception(Language.Instance["Video.Convert3d.FileError"]);
                }

                await _indexService.StartAsync(Input, Output, inputFiles, Provider, Mode, Format, Shift, PopOut, CrossEye);

                SnackbarService.ShowSnackbarSuccess(Language.Instance["Video.Convert3d.ProcessEnd"]);

                FileGridSwitchItem = FileSwitch.Output;
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