using Apt.Core.Consts;
using Apt.Core.Enums;
using Apt.Core.Exceptions;
using Apt.Core.Models;
using Apt.Core.Services.Pages.Video.Organization;
using Apt.Core.Utility;
using Apt.Service.Enums;
using Apt.Service.Extensions;
using Apt.Service.Utility;
using Apt.Service.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui;

namespace Apt.App.ViewModels.Pages.Video.Organization
{
    public partial class IndexPageViewModel : CommonViewModel
    {
        private IndexService _indexService = null!;

        [ObservableProperty]
        private Action _textLoadAction = null!;

        [ObservableProperty]
        private Action _videoLoadAction = null!;

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<string>> _clientSource = [];

        [ObservableProperty]
        private ComBoBoxItem<string> _clientItem = null!;

        public string Client
        {
            get => ClientItem.Value;
            set => ClientItem = ClientSource.FirstOrDefault(e => e.Value == value) ?? ClientSource.First();
        }

        public override void OnInputChangedAction(string value) => GetFileGrids();

        public override void OnOutputChangedAction(string value) => GetFileGrids();

        public override void OnFileGridSwitchItemChangedAction(FileSwitch value)
        {
            GetFileGrids();
            if (value == FileSwitch.Input)
            {
                TextViewVisibility = Visibility.Visible;
                VideoViewVisibility = Visibility.Collapsed;
            }
            else if (value == FileSwitch.Output)
            {
                VideoViewVisibility = Visibility.Visible;
                TextViewVisibility = Visibility.Collapsed;
            }
        }

        [ObservableProperty]
        private string? _textViewItem = null!;

        [ObservableProperty]
        private Uri? _videoViewItem = null!;

        [ObservableProperty]
        private Visibility _textViewVisibility = Visibility.Visible;

        [ObservableProperty]
        private Visibility _videoViewVisibility = Visibility.Collapsed;

        public override void OnFileGridTableItemChangedAction(Service.Controls.FileGrid.Model? value)
        {
            if (FileGridSwitchItem == FileSwitch.Input && value?.FullName is not null)
            {
                TextViewItem = File.ReadAllText(value.FullName);
                TextLoadAction?.Invoke();
            }
            if (FileGridSwitchItem == FileSwitch.Output && value?.FullName is not null)
            {
                VideoViewItem = Source.FileToUri(value.FullName);
                VideoLoadAction?.Invoke();
            }
        }

        public IndexPageViewModel(
            IServiceProvider serviceProvider,
            ISnackbarService snackbarService) :
            base(serviceProvider, snackbarService)
        {
            if (!IsInitialized) InitializeViewModel();
        }

        public override void InitializeViewModel()
        {
            InputExts = [".videoinfo", ".json"];
            OutputExts = AppConst.VideoExts;

            ClientSource =
            [
                new ComBoBoxItem<string>() { Text = Language.Instance["Video.Organization.ClientBilibiliWindows"], Value = "Windows" },
                new ComBoBoxItem<string>() { Text = Language.Instance["Video.Organization.ClientBilibiliAndroid"], Value = "Android" }
            ];

            FileGridSwitchItem = FileSwitch.Input;

            AddMessage(MessageType.Success, Language.Instance["Video.Organization.Help"]);

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
                    throw new Exception(Language.Instance["Video.Organization.InputError"]);
                }
                if (!Directory.Exists(Output))
                {
                    throw new Exception(Language.Instance["Video.Organization.OutputError"]);
                }
                //var inputFiles = FileGridSource.Select(e => e.FullName).ToArray();
                var inputFiles = GetInputFiles(Input).Select(e => e.FullName).ToArray();
                if (inputFiles.Length == 0)
                {
                    throw new Exception(Language.Instance["Video.Organization.FileError"]);
                }

                await _indexService.StartAsync(Input, Output, inputFiles, Client);

                SnackbarService.ShowSnackbarSuccess(Language.Instance["Video.Organization.ProcessEnd"]);

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

        public new ObservableCollection<Service.Controls.FileGrid.Model> GetInputFiles(string input, string[]? exts = null)
        {
            var files = base.GetInputFiles(input, exts).AsEnumerable();
            return new ObservableCollection<Service.Controls.FileGrid.Model>(files);
        }
    }
}