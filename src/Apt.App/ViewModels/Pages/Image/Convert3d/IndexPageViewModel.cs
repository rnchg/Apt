using Apt.Core.Consts;
using Apt.Core.Enums;
using Apt.Core.Exceptions;
using Apt.Core.Models;
using Apt.Core.Services.Pages.Image.Convert3d;
using Apt.Core.Utility;
using Apt.Service.Adapters.Windows;
using Apt.Service.Enums;
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
            InputExts = AppConst.ImageExts;
            OutputExts = AppConst.ImageExts;

            ProviderSource = Adapter.CpuAndDml;

            ModeSource =
            [
                new ComBoBoxItem<string>() {  Text = Language.Instance["Image.Convert3d.ModeStandard"], Value = "Standard" }
            ];
            FormatSource =
            [
                new ComBoBoxItem<string>() {  Text = Language.Instance["Image.Convert3d.FormatHalfSbs"], Value = "HalfSbs" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Image.Convert3d.FormatSbs"], Value = "Sbs" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Image.Convert3d.FormatAnaglyph"], Value = "Anaglyph" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Image.Convert3d.FormatDepth"], Value = "Depth" }
            ];
            ShiftSource =
            [
                new ComBoBoxItem<int>() {  Text = Language.Instance["Image.Convert3d.Shift10"], Value = 10 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["Image.Convert3d.Shift20"], Value = 20 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["Image.Convert3d.Shift30"], Value = 30 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["Image.Convert3d.Shift50"], Value = 50 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["Image.Convert3d.Shift100"], Value = 100 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["Image.Convert3d.Shift200"], Value = 200 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["Image.Convert3d.Shift300"], Value = 300 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["Image.Convert3d.Shift500"], Value = 500 },
                new ComBoBoxItem<int>() {  Text = Language.Instance["Image.Convert3d.Shift1000"], Value = 1000 }
            ];

            FileGridSwitchItem = FileSwitch.Input;

            AddMessage(MessageType.Success, Language.Instance["Image.Convert3d.Help"]);

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
                    throw new Exception(Language.Instance["Image.Convert3d.InputError"]);
                }
                if (!Directory.Exists(Output))
                {
                    throw new Exception(Language.Instance["Image.Convert3d.OutputError"]);
                }
                var inputFiles = FileGridTableList.Select(e => e.FullName).ToArray();
                if (inputFiles.Length == 0)
                {
                    throw new Exception(Language.Instance["Image.Convert3d.FileError"]);
                }

                await _indexService.StartAsync(Input, Output, inputFiles, Provider, Mode, Format, Shift, PopOut, CrossEye);

                SnackbarService.ShowSnackbarSuccess(Language.Instance["Image.Convert3d.ProcessEnd"]);

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