using Apt.Service.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Apt.App.ViewModels.Base
{
    public partial class CommonViewModel : BaseViewModel
    {
        public bool IsInitialized { get; set; } = false;

        public Action<Paragraph> MessageAction { get; set; } = null!;

        [ObservableProperty]
        private string _input = null!;

        partial void OnInputChanged(string value) => OnInputChangedAction(value);

        public virtual void OnInputChangedAction(string value) { }

        [RelayCommand]
        public void SetInput()
        {
            var openFolderDialog = new OpenFolderDialog();
            if (!string.IsNullOrEmpty(Input)) openFolderDialog.InitialDirectory = Input;
            if (openFolderDialog.ShowDialog() is true)
            {
                Input = openFolderDialog.FolderName;
            }
        }

        [ObservableProperty]
        private string _output = null!;

        partial void OnOutputChanged(string value) => OnOutputChangedAction(value);

        public virtual void OnOutputChangedAction(string value) { }

        [RelayCommand]
        public void SetOutput()
        {
            var openFolderDialog = new OpenFolderDialog();
            if (!string.IsNullOrEmpty(Output)) openFolderDialog.InitialDirectory = Output;
            if (openFolderDialog.ShowDialog() is true)
            {
                Output = openFolderDialog.FolderName;
            }
        }

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<string>> _inputSortSource = [];

        [ObservableProperty]
        private ComBoBoxItem<string> _inputSortItem = null!;

        partial void OnInputSortItemChanged(ComBoBoxItem<string> value) => OnInputSortItemChangedAction(value);

        public virtual void OnInputSortItemChangedAction(ComBoBoxItem<string> value) { }

        public string InputSort
        {
            get => InputSortItem.Value;
            set => InputSortItem = InputSortSource.First(e => e.Value == value);
        }

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<string>> _sortRuleSource = [];

        [ObservableProperty]
        private ComBoBoxItem<string> _sortRuleItem = null!;

        partial void OnSortRuleItemChanged(ComBoBoxItem<string> value) => OnSortRuleItemChangedAction(value);

        public virtual void OnSortRuleItemChangedAction(ComBoBoxItem<string> value) { }

        public string SortRule
        {
            get => SortRuleItem.Value;
            set => SortRuleItem = SortRuleSource.First(e => e.Value == value);
        }

        [ObservableProperty]
        private int _progressBarMaximum = 1000000;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ProgressBarText))]
        private int _progressBarValue = 0;

        public string ProgressBarText
        {
            get => (ProgressBarValue / (double)ProgressBarMaximum).ToString("0.00%");
        }

        [ObservableProperty]
        private bool _startEnabled = true;

        [RelayCommand]
        public async Task SetStart() => await Start();


        [ObservableProperty]
        private bool _stopEnabled = false;

        [RelayCommand]
        public void SetStop() => StopEnabled = false;

        [ObservableProperty]
        private bool _openEnabled = false;

        [RelayCommand]
        public void SetOpen() => System.Diagnostics.Process.Start("explorer", Output);

        public override void OnNavigatedTo()
        {
            if (!IsInitialized) InitializeViewModel();
        }

        public virtual void InitializeViewModel()
        {
            IsInitialized = true;
        }

        public virtual Task Start()
        {
            return Task.CompletedTask;
        }

        public Task AddProcess(int process) => DispatchAsync(() => ProgressBarValue = process);
    }
}
