using General.Apt.App.Extensions;
using General.Apt.App.Utility;
using General.Apt.Service.Exceptions;
using General.Apt.Service.Models;
using General.Apt.Service.Services.Pages.Video.AutoWipe;
using General.Apt.Service.Utility;
using Microsoft.Win32;
using OpenCvSharp;
using System.IO;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Media.Imaging;
using Wpf.Ui.Controls;

namespace General.Apt.App.ViewModels.Pages.Video.AutoWipe
{
    public partial class IndexViewModel : ObservableValidator, INavigationAware
    {
        private bool _isInitialized = false;
        private IndexService _indexService;

        public Action<Paragraph> MessageAction { get; set; }

        [ObservableProperty]
        private Func<byte[]> _maskAction;

        [ObservableProperty]
        private string _input;

        partial void OnInputChanged(string value)
        {
            _ = SetMaskFirst();
        }

        [ObservableProperty]
        private ImageSource _inputImageFirst;

        [RelayCommand]
        private void SetInput()
        {
            var openFolderDialog = new OpenFolderDialog();
            if (!string.IsNullOrEmpty(Input)) openFolderDialog.InitialDirectory = Input;
            if (openFolderDialog.ShowDialog() is true)
            {
                Input = openFolderDialog.FolderName;
            }
        }

        public async Task SetMaskFirst()
        {
            try
            {
                if (!Directory.Exists(Input) || InputSortItem == null || SortRuleItem == null)
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
        }

        [ObservableProperty]
        private string _output;

        [RelayCommand]
        private void SetOutput()
        {
            var openFolderDialog = new OpenFolderDialog();
            if (!string.IsNullOrEmpty(Output)) openFolderDialog.InitialDirectory = Output;
            if (openFolderDialog.ShowDialog() is true)
            {
                Output = openFolderDialog.FolderName;
            }
        }

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<string>> _inputSortSource;

        [ObservableProperty]
        private ComBoBoxItem<string> _inputSortItem;

        public string InputSort
        {
            get => InputSortItem.Value;
            set => InputSortItem = InputSortSource.FirstOrDefault(e => e.Value == value);
        }

        partial void OnInputSortItemChanged(ComBoBoxItem<string> value)
        {
            _ = SetMaskFirst();
        }

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<string>> _sortRuleSource;

        [ObservableProperty]
        private ComBoBoxItem<string> _sortRuleItem;

        public string SortRule
        {
            get => SortRuleItem.Value;
            set => SortRuleItem = SortRuleSource.FirstOrDefault(e => e.Value == value);
        }

        partial void OnSortRuleItemChanged(ComBoBoxItem<string> value)
        {
            _ = SetMaskFirst();
        }

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<string>> _providerSource;

        [ObservableProperty]
        private ComBoBoxItem<string> _providerItem;

        public string Provider
        {
            get => ProviderItem.Value;
            set => ProviderItem = ProviderSource.FirstOrDefault(e => e.Value == value);
        }

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<string>> _modeSource;

        [ObservableProperty]
        private ComBoBoxItem<string> _modeItem;

        public string Mode
        {
            get => ModeItem.Value;
            set => ModeItem = ModeSource.FirstOrDefault(e => e.Value == value);
        }

        [ObservableProperty]
        private double _maskDrawingSize;

        partial void OnMaskDrawingSizeChanged(double value)
        {
            MaskDrawingAttributes.Width = value;
            MaskDrawingAttributes.Height = value;
        }

        [ObservableProperty]
        private DrawingAttributes _maskDrawingAttributes;

        [ObservableProperty]
        private int _progressBarMaximum;

        [ObservableProperty]
        private int _progressBarValue;

        partial void OnProgressBarValueChanged(int value)
        {
            OnPropertyChanged(nameof(ProgressBarText));
        }

        public string ProgressBarText
        {
            get => (ProgressBarValue / (double)ProgressBarMaximum).ToString("0.00%");
        }

        [ObservableProperty]
        private bool _startEnabled;

        [RelayCommand]
        private async Task SetStart() => await Start();


        [ObservableProperty]
        private bool _stopEnabled;

        [RelayCommand]
        private void SetStop() => StopEnabled = false;

        [ObservableProperty]
        private bool _openEnabled;

        [RelayCommand]
        private void SetOpen() => System.Diagnostics.Process.Start("explorer", Output);

        public IndexViewModel()
        {
            if (!_isInitialized) InitializeViewModel();
        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized) InitializeViewModel();
        }

        public void OnNavigatedFrom() { }

        public void InitializeViewModel()
        {
            MaskDrawingAttributes = new DrawingAttributes()
            {
                Color = Color.FromArgb(75, 0, 0, 255),
            };

            MaskDrawingSize = 30;

            InputSortSource = new ObservableCollection<ComBoBoxItem<string>>()
            {
                new ComBoBoxItem<string>() { Text = Language.GetString("VideoAutoWipeIndexPageInputSortName"), Value = "Name" },
                new ComBoBoxItem<string>() { Text = Language.GetString("VideoAutoWipeIndexPageInputSortLastWriteTime"), Value = "LastWriteTime" },
                new ComBoBoxItem<string>() { Text = Language.GetString("VideoAutoWipeIndexPageInputSortLength"), Value = "Length" }
            };
            SortRuleSource = new ObservableCollection<ComBoBoxItem<string>>()
            {
                new ComBoBoxItem<string>() { Text = Language.GetString("VideoAutoWipeIndexPageInputSortRuleAsc"), Value = "Asc" },
                new ComBoBoxItem<string>() { Text = Language.GetString("VideoAutoWipeIndexPageInputSortRuleDesc"), Value = "Desc" }
            };
            ProviderSource = Searcher.GetProvider();
            ModeSource = new ObservableCollection<ComBoBoxItem<string>>()
            {
                new ComBoBoxItem<string>() {  Text = Language.GetString("VideoAutoWipeIndexPageModeNormal"), Value = "Normal" }
            };
            ProgressBarMaximum = 1000000;
            ProgressBarValue = 0;
            StartEnabled = true;
            StopEnabled = false;
            OpenEnabled = false;

            _indexService = new IndexService();
            _indexService.ProgressMax = ProgressBarMaximum;
            _indexService.Message = async (type, message) => await Message.AddMessage(type, message, MessageAction);
            _indexService.Progress = async (process) => await AddProcess(process);
            _indexService.IsStop = () => !StopEnabled;

            _isInitialized = true;
        }

        public async Task Start()
        {
            try
            {
                StartEnabled = false;
                StopEnabled = true;
                OpenEnabled = true;

                var mask = MaskAction?.Invoke();

                await _indexService.Start(Input, Output, InputSort, SortRule, Provider, Mode, mask);

                ProgressBarValue = ProgressBarMaximum;

                Message.ShowSnackbarSuccess(Language.GetString("VideoAutoWipeIndexPageOperationCompleted"));

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

        public Task AddProcess(int process)
        {
            return Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ProgressBarValue = process;
                });
            });
        }
    }
}