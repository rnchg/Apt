using APT.Core.Consts;
using APT.Core.Enums;
using APT.Core.Exceptions;
using APT.Core.Services.Pages.Image.CartoonComic;
using APT.Core.Utility;
using APT.Service.Adapters.Windows;
using APT.Service.Controls.FileGrid;
using APT.Service.Enums;
using APT.Service.Extensions;
using APT.Service.Utility;
using APT.Service.ViewModels.Base;
using Common.WindowsDesktop.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui;

namespace APT.App.ViewModels.Pages.Image.CartoonComic
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

        public override void OnFileGridTableItemChangedAction(Model? value) => FileViewItem = Source.FileToUri(value?.FullName);

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
                new ComBoBoxItem<string>() {  Text = Language.Instance["Image.CartoonComic.ModeHayao"], Value = "Hayao" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Image.CartoonComic.ModeCute"], Value = "Cute" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Image.CartoonComic.ModeJPFace"], Value = "JPFace" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Image.CartoonComic.ModeGhibli"], Value = "Ghibli" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Image.CartoonComic.ModeShinkai"], Value = "Shinkai" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Image.CartoonComic.ModeSketch"], Value = "Sketch" }
            ];
            QualitySource =
            [
                new ComBoBoxItem<string>() {  Text = Language.Instance["Image.CartoonComic.QualityAuto"], Value = "Auto" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Image.CartoonComic.QualityHigh"], Value = "High" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Image.CartoonComic.QualityMedium"], Value = "Medium" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["Image.CartoonComic.QualityLow"], Value = "Low" }
            ];

            FileGridSwitchItem = FileSwitch.Input;

            AddMessage(MessageType.Success, Language.Instance["Image.CartoonComic.Help"]);

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
                    throw new Exception(Language.Instance["Image.CartoonComic.InputError"]);
                }
                if (!Directory.Exists(Output))
                {
                    throw new Exception(Language.Instance["Image.CartoonComic.OutputError"]);
                }
                var inputFiles = FileGridTableList.Select(e => e.FullName).ToArray();
                if (inputFiles.Length == 0)
                {
                    throw new Exception(Language.Instance["Image.CartoonComic.FileError"]);
                }

                await _indexService.StartAsync(Input, Output, inputFiles, Provider, Mode, Quality);

                SnackbarService.ShowSnackbarSuccess(Language.Instance["Image.CartoonComic.ProcessEnd"]);

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