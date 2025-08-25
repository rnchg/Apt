using APT.Core.Consts;
using APT.Core.Enums;
using APT.Core.Exceptions;
using APT.Core.Services.Pages.Audio.VocalSplit;
using APT.Core.Utility;
using APT.Service.Adapters.Windows;
using APT.Service.Enums;
using APT.Service.Extensions;
using APT.Service.Utility;
using APT.Service.ViewModels.Base;
using Common.WindowsDesktop.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui;

namespace APT.App.ViewModels.Pages.Audio.VocalSplit
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
            InputExts = AppConst.AudioExts;
            OutputExts = AppConst.AudioExts;

            ProviderSource = Adapter.CpuAndDml;

            ModeSource =
            [
                new ComBoBoxItem<string>() {  Text = Language.Instance["Audio.VocalSplit.ModeStandard"], Value = "Standard" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Audio.VocalSplit.ModeQuality"], Value = "Quality" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Audio.VocalSplit.ModeKaraoke"], Value = "Karaoke" }
            ];

            FileGridSwitchItem = FileSwitch.Input;

            AddMessage(MessageType.Success, Language.Instance["Audio.VocalSplit.Help"]);

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
                    throw new Exception(Language.Instance["Audio.VocalSplit.InputError"]);
                }
                if (!Directory.Exists(Output))
                {
                    throw new Exception(Language.Instance["Audio.VocalSplit.OutputError"]);
                }
                var inputFiles = FileGridTableList.Select(e => e.FullName).ToArray();
                if (inputFiles.Length == 0)
                {
                    throw new Exception(Language.Instance["Audio.VocalSplit.FileError"]);
                }

                await _indexService.StartAsync(Input, Output, inputFiles, Provider, Mode);

                SnackbarService.ShowSnackbarSuccess(Language.Instance["Audio.VocalSplit.ProcessEnd"]);

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