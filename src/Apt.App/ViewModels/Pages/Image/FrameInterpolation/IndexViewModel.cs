using Apt.App.Adapters.Windows;
using Apt.App.Utility;
using Apt.App.ViewModels.Base;
using Apt.Service.Exceptions;
using Apt.Service.Models;
using Apt.Service.Services.Pages.Image.FrameInterpolation;
using Apt.Service.Utility;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Apt.App.ViewModels.Pages.Image.FrameInterpolation
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
        private ObservableCollection<ComBoBoxItem<string>> _scaleSource = [];

        [ObservableProperty]
        private ComBoBoxItem<string> _scaleItem = null!;

        public string Scale
        {
            get => ScaleItem.Value;
            set => ScaleItem = ScaleSource.First(e => e.Value == value);
        }

        public IndexViewModel()
        {
            if (!IsInitialized) InitializeViewModel();
        }

        public override void InitializeViewModel()
        {
            InputSortSource =
            [
                new ComBoBoxItem<string>() { Text = Language.Instance["ImageFrameInterpolationIndexPageInputSortName"], Value = "Name" },
                new ComBoBoxItem<string>() { Text = Language.Instance["ImageFrameInterpolationIndexPageInputSortLastWriteTime"], Value = "LastWriteTime" },
                new ComBoBoxItem<string>() { Text = Language.Instance["ImageFrameInterpolationIndexPageInputSortLength"], Value = "Length" }
            ];
            SortRuleSource =
            [
                new ComBoBoxItem<string>() { Text = Language.Instance["ImageFrameInterpolationIndexPageInputSortRuleAsc"], Value = "Asc" },
                new ComBoBoxItem<string>() { Text = Language.Instance["ImageFrameInterpolationIndexPageInputSortRuleDesc"], Value = "Desc" }
            ];
            ProviderSource = Adapter.CpuAndGpu;
            ModeSource =
            [
                new ComBoBoxItem<string>() { Text = Language.Instance["ImageFrameInterpolationIndexPageModeStandard"], Value = "Standard" }
            ];
            ScaleSource =
            [
                new ComBoBoxItem<string>() { Text = Language.Instance["ImageFrameInterpolationIndexPageScaleX2"], Value = "X2" },
                new ComBoBoxItem<string>() { Text = Language.Instance["ImageFrameInterpolationIndexPageScaleX4"], Value = "X4" },
                new ComBoBoxItem<string>() { Text = Language.Instance["ImageFrameInterpolationIndexPageScaleX8"], Value = "X8" }
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

                await _indexService.Start(Input, Output, InputSort, SortRule, Provider, Mode, Scale);

                ProgressBarValue = ProgressBarMaximum;

                Message.ShowSnackbarSuccess(Language.Instance["ImageFrameInterpolationIndexPageOperationCompleted"]);

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