using Apt.App.Adapters.Windows;
using Apt.App.Utility;
using Apt.App.ViewModels.Base;
using Apt.Service.Exceptions;
using Apt.Service.Models;
using Apt.Service.Services.Pages.Image.CartoonComic;
using Apt.Service.Utility;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Apt.App.ViewModels.Pages.Image.CartoonComic
{
    public partial class IndexViewModel : CommonViewModel
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

        public IndexViewModel()
        {
            if (!IsInitialized) InitializeViewModel();
        }

        public override void InitializeViewModel()
        {
            InputSortSource =
            [
                new ComBoBoxItem<string>() { Text = Language.Instance["ImageCartoonComicIndexPageInputSortName"], Value = "Name" },
                new ComBoBoxItem<string>() { Text = Language.Instance["ImageCartoonComicIndexPageInputSortLastWriteTime"], Value = "LastWriteTime" },
                new ComBoBoxItem<string>() { Text = Language.Instance["ImageCartoonComicIndexPageInputSortLength"], Value = "Length" }
            ];
            SortRuleSource =
            [
                new ComBoBoxItem<string>() { Text = Language.Instance["ImageCartoonComicIndexPageInputSortRuleAsc"], Value = "Asc" },
                new ComBoBoxItem<string>() { Text = Language.Instance["ImageCartoonComicIndexPageInputSortRuleDesc"], Value = "Desc" }
            ];
            ProviderSource = Adapter.CpuAndGpu;
            ModeSource =
            [
                new ComBoBoxItem<string>() {  Text = Language.Instance["ImageCartoonComicIndexPageModeHayao"], Value = "Hayao" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["ImageCartoonComicIndexPageModeCute"], Value = "Cute" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["ImageCartoonComicIndexPageModeJPFace"], Value = "JPFace" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["ImageCartoonComicIndexPageModeShinkai"], Value = "Shinkai" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["ImageCartoonComicIndexPageModeSketch"], Value = "Sketch" }
            ];
            QualitySource =
            [
                new ComBoBoxItem<string>() {  Text = Language.Instance["ImageCartoonComicIndexPageQualityAuto"], Value = "Auto" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["ImageCartoonComicIndexPageQualityHigh"], Value = "High" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["ImageCartoonComicIndexPageQualityMedium"], Value = "Medium" },
                new ComBoBoxItem<string>() {  Text = Language.Instance["ImageCartoonComicIndexPageQualityLow"], Value = "Low" }
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

                await _indexService.Start(Input, Output, InputSort, SortRule, Provider, Mode, Quality);

                ProgressBarValue = ProgressBarMaximum;

                Message.ShowSnackbarSuccess(Language.Instance["ImageCartoonComicIndexPageOperationCompleted"]);

                if (Current.Config.App.IsAutoOpenOutput) SetOpen();
            }
            catch (ActivationException ex)
            {
                await Validate.ShowLicense(ex.Message);
            }
            catch (Exception ex)
            {
                Message.ShowSnackbarError(ex.Message);

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