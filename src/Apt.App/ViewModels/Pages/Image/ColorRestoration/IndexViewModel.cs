using Apt.App.Adapters.Windows;
using Apt.App.Utility;
using Apt.App.ViewModels.Base;
using Apt.Service.Exceptions;
using Apt.Service.Models;
using Apt.Service.Services.Pages.Image.ColorRestoration;
using Apt.Service.Utility;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Apt.App.ViewModels.Pages.Image.ColorRestoration
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

        partial void OnModeItemChanged(ComBoBoxItem<string> value)
        {
            if (value?.Value is null) return;
            if (value.Value == "Standard")
            {
                QualitySource =
                [
                    new ComBoBoxItem<string>() {  Text = Language.Instance["ImageColorRestorationIndexPageQualityHigh"], Value = "High" },
                    new ComBoBoxItem<string>() {  Text = Language.Instance["ImageColorRestorationIndexPageQualityMedium"], Value = "Medium" },
                    new ComBoBoxItem<string>() {  Text = Language.Instance["ImageColorRestorationIndexPageQualityLow"], Value = "Low" }
                ];
                Quality = "Medium";
                return;
            }
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
                new ComBoBoxItem<string>() { Text = Language.Instance["ImageColorRestorationIndexPageInputSortName"], Value = "Name" },
                new ComBoBoxItem<string>() { Text = Language.Instance["ImageColorRestorationIndexPageInputSortLastWriteTime"], Value = "LastWriteTime" },
                new ComBoBoxItem<string>() { Text = Language.Instance["ImageColorRestorationIndexPageInputSortLength"], Value = "Length" }
            ];
            SortRuleSource =
            [
                new ComBoBoxItem<string>() { Text = Language.Instance["ImageColorRestorationIndexPageInputSortRuleAsc"], Value = "Asc" },
                new ComBoBoxItem<string>() { Text = Language.Instance["ImageColorRestorationIndexPageInputSortRuleDesc"], Value = "Desc" }
            ];
            ProviderSource = Adapter.CpuAndGpu;
            ModeSource =
            [
                new ComBoBoxItem<string>() {  Text = Language.Instance["ImageColorRestorationIndexPageModeStandard"], Value = "Standard" }
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

                Message.ShowSnackbarSuccess(Language.Instance["ImageColorRestorationIndexPageOperationCompleted"]);

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