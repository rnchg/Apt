using Apt.Core.Consts;
using Apt.Core.Enums;
using Apt.Core.Exceptions;
using Apt.Core.Models;
using Apt.Core.Services.Pages.Audio.VocalSplit;
using Apt.Core.Utility;
using Apt.Service.Adapters.Windows;
using Apt.Service.Extensions;
using Apt.Service.Utility;
using Apt.Service.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui;

namespace Apt.App.ViewModels.Pages.Audio.VocalSplit
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

        public override void OnInputChangedAction(string value) => GetFileGrids(AppConst.AudioExts);

        public override void OnOutputChangedAction(string value) => GetFileGrids(AppConst.AudioExts);

        public override void OnFileGridSwitchChangedAction(bool value) => GetFileGrids(AppConst.AudioExts);

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
            ProviderSource = Adapter.CpuAndGpu;

            ModeSource =
            [
                new ComBoBoxItem<string>() {  Text = Language.Instance["AudioVocalSplitIndexPageModeStandard"], Value = "Standard" }
            ];

            AddMessage(MessageType.Success, Language.Instance["AudioVocalSplitHelp"]);

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
                StartEnabled = false;
                StopEnabled = true;
                OpenEnabled = true;

                FileGridSwitch = false;

                if (!Directory.Exists(Input))
                {
                    throw new Exception(Language.Instance["AudioVocalSplitIndexPageInputError"]);
                }
                if (!Directory.Exists(Output))
                {
                    throw new Exception(Language.Instance["AudioVocalSplitIndexPageOutputError"]);
                }
                var inputFiles = FileGridSource.Select(e => e.FullName).ToArray();
                if (inputFiles.Length == 0)
                {
                    throw new Exception(Language.Instance["AudioVocalSplitIndexPageFileError"]);
                }

                await _indexService.Start(Input, Output, inputFiles, Provider, Mode);

                SnackbarService.ShowSnackbarSuccess(Language.Instance["AudioVocalSplitIndexPageProcessEnd"]);

                ProgressBarValue = ProgressBarMaximum;

                FileGridSwitch = true;
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