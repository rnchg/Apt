using APT.Core.Consts;
using APT.Core.Enums;
using APT.Core.Exceptions;
using APT.Core.Services.Pages.Video.CartoonComic;
using APT.Core.Utility;
using APT.Service.Adapters.Windows;
using APT.Service.Enums;
using APT.Service.Extensions;
using APT.Service.Utility;
using APT.Service.ViewModels.Base;
using Common.WindowsDesktop.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui;

namespace APT.App.ViewModels.Pages.Video.CartoonComic
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
                new ComBoBoxItem<string>() {  Text = Language.Instance["Video.CartoonComic.ModeHayao"], Value = "Hayao" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Video.CartoonComic.ModeCute"], Value = "Cute" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Video.CartoonComic.ModeJPFace"], Value = "JPFace" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Video.CartoonComic.ModeGhibli"], Value = "Ghibli" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Video.CartoonComic.ModeShinkai"], Value = "Shinkai" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Video.CartoonComic.ModeSketch"], Value = "Sketch" }
            ];
            QualitySource =
            [
                new ComBoBoxItem<string>() {  Text = Language.Instance["Video.CartoonComic.QualityAuto"], Value = "Auto" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Video.CartoonComic.QualityHigh"], Value = "High" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Video.CartoonComic.QualityMedium"], Value = "Medium" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Video.CartoonComic.QualityLow"], Value = "Low" }
            ];

            FileGridSwitchItem = FileSwitch.Input;

            AddMessage(MessageType.Success, Language.Instance["Video.CartoonComic.Help"]);

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
                    throw new Exception(Language.Instance["Video.CartoonComic.InputError"]);
                }
                if (!Directory.Exists(Output))
                {
                    throw new Exception(Language.Instance["Video.CartoonComic.OutputError"]);
                }
                var inputFiles = FileGridTableList.Select(e => e.FullName).ToArray();
                if (inputFiles.Length == 0)
                {
                    throw new Exception(Language.Instance["Video.CartoonComic.FileError"]);
                }

                await _indexService.StartAsync(Input, Output, inputFiles, Provider, Mode, Quality);

                SnackbarService.ShowSnackbarSuccess(Language.Instance["Video.CartoonComic.ProcessEnd"]);

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