using Apt.App.Adapters.Windows;
using Apt.App.Utility;
using Apt.App.ViewModels.Base;
using Apt.Service.Exceptions;
using Apt.Service.Models;
using Apt.Service.Services.Pages.Video.CartoonComic;
using Apt.Service.Utility;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Apt.App.ViewModels.Pages.Video.CartoonComic
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
                new ComBoBoxItem<string>() { Text = Language.Instance["VideoCartoonComicIndexPageInputSortName"], Value = "Name" },
                new ComBoBoxItem<string>() { Text = Language.Instance["VideoCartoonComicIndexPageInputSortLastWriteTime"], Value = "LastWriteTime" },
                new ComBoBoxItem<string>() { Text = Language.Instance["VideoCartoonComicIndexPageInputSortLength"], Value = "Length" }
            ];
            SortRuleSource =
            [
                new ComBoBoxItem<string>() { Text = Language.Instance["VideoCartoonComicIndexPageInputSortRuleAsc"], Value = "Asc" },
                new ComBoBoxItem<string>() { Text = Language.Instance["VideoCartoonComicIndexPageInputSortRuleDesc"], Value = "Desc" }
            ];
            ProviderSource = Adapter.CpuAndGpu;
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

                Message.ShowSnackbarSuccess(Language.Instance["VideoCartoonComicIndexPageOperationCompleted"]);

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