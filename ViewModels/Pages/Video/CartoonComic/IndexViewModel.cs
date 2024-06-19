using General.Apt.App.Utility;
using General.Apt.Service.Exceptions;
using General.Apt.Service.Models;
using General.Apt.Service.Services.Pages.Video.CartoonComic;
using General.Apt.Service.Utility;
using Microsoft.Win32;
using System.Windows.Documents;
using Wpf.Ui.Controls;

namespace General.Apt.App.ViewModels.Pages.Video.CartoonComic
{
    public partial class IndexViewModel : ObservableValidator, INavigationAware
    {
        private bool _isInitialized = false;
        private IndexService _indexService;

        public Action<Paragraph> MessageAction { get; set; }

        [ObservableProperty]
        private string _input;

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

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<string>> _sortRuleSource;

        [ObservableProperty]
        private ComBoBoxItem<string> _sortRuleItem;

        public string SortRule
        {
            get => SortRuleItem.Value;
            set => SortRuleItem = SortRuleSource.FirstOrDefault(e => e.Value == value);
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
        private ObservableCollection<ComBoBoxItem<string>> _qualitySource;

        [ObservableProperty]
        private ComBoBoxItem<string> _qualityItem;

        public string Quality
        {
            get => QualityItem.Value;
            set => QualityItem = QualitySource.FirstOrDefault(e => e.Value == value);
        }

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
            InputSortSource = new ObservableCollection<ComBoBoxItem<string>>()
            {
                new ComBoBoxItem<string>() { Text = Language.GetString("VideoCartoonComicIndexPageInputSortName"), Value = "Name" },
                new ComBoBoxItem<string>() { Text = Language.GetString("VideoCartoonComicIndexPageInputSortLastWriteTime"), Value = "LastWriteTime" },
                new ComBoBoxItem<string>() { Text = Language.GetString("VideoCartoonComicIndexPageInputSortLength"), Value = "Length" }
            };
            SortRuleSource = new ObservableCollection<ComBoBoxItem<string>>()
            {
                new ComBoBoxItem<string>() { Text = Language.GetString("VideoCartoonComicIndexPageInputSortRuleAsc"), Value = "Asc" },
                new ComBoBoxItem<string>() { Text = Language.GetString("VideoCartoonComicIndexPageInputSortRuleDesc"), Value = "Desc" }
            };
            ProviderSource = Searcher.GetProvider();
            ModeSource = new ObservableCollection<ComBoBoxItem<string>>()
            {
                new ComBoBoxItem<string>() {  Text = Language.GetString("VideoCartoonComicIndexPageModeHayao"), Value = "Hayao" },
                new ComBoBoxItem<string>() {  Text = Language.GetString("VideoCartoonComicIndexPageModeCute"), Value = "Cute" },
                new ComBoBoxItem<string>() {  Text = Language.GetString("VideoCartoonComicIndexPageModeJPFace"), Value = "JPFace" },
                new ComBoBoxItem<string>() {  Text = Language.GetString("VideoCartoonComicIndexPageModeShinkai"), Value = "Shinkai" },
                new ComBoBoxItem<string>() {  Text = Language.GetString("VideoCartoonComicIndexPageModeSketch"), Value = "Sketch" }
            };
            QualitySource = new ObservableCollection<ComBoBoxItem<string>>()
            {
                new ComBoBoxItem<string>() {  Text = Language.GetString("VideoCartoonComicIndexPageQualityAuto"), Value = "Auto" },
                new ComBoBoxItem<string>() {  Text = Language.GetString("VideoCartoonComicIndexPageQualityHigh"), Value = "High" },
                new ComBoBoxItem<string>() {  Text = Language.GetString("VideoCartoonComicIndexPageQualityMedium"), Value = "Medium" },
                new ComBoBoxItem<string>() {  Text = Language.GetString("VideoCartoonComicIndexPageQualityLow"), Value = "Low" }
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

                await _indexService.Start(Input, Output, InputSort, SortRule, Provider, Mode, Quality);

                ProgressBarValue = ProgressBarMaximum;

                Message.ShowSnackbarSuccess(Language.GetString("VideoCartoonComicIndexPageOperationCompleted"));

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