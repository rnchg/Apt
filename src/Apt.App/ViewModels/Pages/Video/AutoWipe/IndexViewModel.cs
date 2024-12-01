using Apt.App.Adapters.Windows;
using Apt.App.Extensions;
using Apt.App.Utility;
using Apt.App.ViewModels.Base;
using Apt.Service.Exceptions;
using Apt.Service.Models;
using Apt.Service.Services.Pages.Video.AutoWipe;
using Apt.Service.Utility;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenCvSharp;
using System.Windows.Ink;
using System.Windows.Media.Imaging;

namespace Apt.App.ViewModels.Pages.Video.AutoWipe
{
    public partial class IndexViewModel : CommonViewModel
    {
        private IndexService _indexService = null!;

        [ObservableProperty]
        private Func<byte[]> _maskAction = null!;

        [ObservableProperty]
        private double _maskDrawingSize;

        partial void OnMaskDrawingSizeChanged(double value)
        {
            MaskDrawingAttributes.Width = value;
            MaskDrawingAttributes.Height = value;
        }

        [ObservableProperty]
        private DrawingAttributes _maskDrawingAttributes = new()
        {
            Color = Color.FromArgb(75, 0, 0, 255),
        };

        [ObservableProperty]
        private ImageSource? _inputImageFirst = null!;

        public async Task SetMaskFirst()
        {
            try
            {
                if (!Directory.Exists(Input) || InputSortItem is null || SortRuleItem is null)
                {
                    InputImageFirst = null;
                }
                else
                {
                    var file = _indexService.GetFileFirst(Input, InputSort, SortRule);
                    using var video = new VideoCapture();
                    var mat = new Mat();
                    video.Open(file);
                    while (video.Read(mat))
                    {
                        InputImageFirst = mat.ToBytes().ToImage() as BitmapImage;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                InputImageFirst = null;
                await Message.AddMessageError(ex.Message, MessageAction);
            }
            finally
            {
                if (InputImageFirst == null)
                {
                    MaskDrawingSize = 1;
                }
                else
                {
                    MaskDrawingSize = 30;
                }
            }
        }

        public override void OnInputChangedAction(string value) => _ = SetMaskFirst();

        public override void OnInputSortItemChangedAction(ComBoBoxItem<string> value) => _ = SetMaskFirst();

        public override void OnSortRuleItemChangedAction(ComBoBoxItem<string> value) => _ = SetMaskFirst();

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

        public IndexViewModel()
        {
            if (!IsInitialized) InitializeViewModel();
        }

        public override void InitializeViewModel()
        {
            InputSortSource =
            [
                new ComBoBoxItem<string>() { Text = Language.Instance["VideoAutoWipeIndexPageInputSortName"], Value = "Name" },
                new ComBoBoxItem<string>() { Text = Language.Instance["VideoAutoWipeIndexPageInputSortLastWriteTime"], Value = "LastWriteTime" },
                new ComBoBoxItem<string>() { Text = Language.Instance["VideoAutoWipeIndexPageInputSortLength"], Value = "Length" }
            ];
            SortRuleSource =
            [
                new ComBoBoxItem<string>() { Text = Language.Instance["VideoAutoWipeIndexPageInputSortRuleAsc"], Value = "Asc" },
                new ComBoBoxItem<string>() { Text = Language.Instance["VideoAutoWipeIndexPageInputSortRuleDesc"], Value = "Desc" }
            ];
            ProviderSource = Adapter.CpuAndGpu;
            ModeSource =
            [
                new ComBoBoxItem<string>() {  Text = Language.Instance["VideoAutoWipeIndexPageModeStandard"], Value = "Standard" }
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

                await _indexService.Start(Input, Output, InputSort, SortRule, MaskAction.Invoke(), Provider, Mode);

                ProgressBarValue = ProgressBarMaximum;

                Message.ShowSnackbarSuccess(Language.Instance["VideoAutoWipeIndexPageOperationCompleted"]);

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