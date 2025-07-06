using Apt.Core.Consts;
using Apt.Core.Enums;
using Apt.Core.Exceptions;
using Apt.Core.Models;
using Apt.Core.Services.Pages.Video.Organization;
using Apt.Core.Utility;
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

        public override void OnFileGridInputEnableChangedAction(bool value)
        {
            base.OnFileGridInputEnableChangedAction(value);
            if (value)
            {
                GetFileGrids();
                TextViewVisibility = Visibility.Visible;
                VideoViewVisibility = Visibility.Collapsed;
            }
        }

        public override void OnFileGridOutputEnableChangedAction(bool value)
        {
            base.OnFileGridOutputEnableChangedAction(value);
            if (value)
            {
                GetFileGrids();
                TextViewVisibility = Visibility.Collapsed;
                VideoViewVisibility = Visibility.Visible;
            }
        }

        [ObservableProperty]
        private Uri? _fileViewSource = null!;

        [ObservableProperty]
        private string? _textViewSource = null!;

        [ObservableProperty]
        private Visibility _textViewVisibility = Visibility.Visible;

        [ObservableProperty]
        private Visibility _videoViewVisibility = Visibility.Collapsed;

        public override void OnFileGridItemChangedAction(Service.Controls.FileGrid.Model? value)
        {
            if (FileGridInputEnable && value?.FullName is not null)
            {
                TextViewSource = File.ReadAllText(value.FullName);
                TextLoadAction?.Invoke();
            }
            if (FileGridOutputEnable && value?.FullName is not null)
            {
                FileViewSource = Source.FileToUri(value.FullName);
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
                new ComBoBoxItem<string>() { Text = Language.Instance["VideoOrganizationIndexPageClientBilibiliWindows"], Value = "Windows" },
                new ComBoBoxItem<string>() { Text = Language.Instance["VideoOrganizationIndexPageClientBilibiliAndroid"], Value = "Android" }
            ];

            AddMessage(MessageType.Success, Language.Instance["VideoOrganizationHelp"]);

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

                FileGridInputEnable = true;
                FileGridOutputEnable = false;

                if (!Directory.Exists(Input))
                {
                    throw new Exception(Language.Instance["VideoOrganizationIndexPageInputError"]);
                }
                if (!Directory.Exists(Output))
                {
                    throw new Exception(Language.Instance["VideoOrganizationIndexPageOutputError"]);
                }
                //var inputFiles = FileGridSource.Select(e => e.FullName).ToArray();
                var inputFiles = GetInputFiles(Input).Select(e => e.FullName).ToArray();
                if (inputFiles.Length == 0)
                {
                    throw new Exception(Language.Instance["VideoOrganizationIndexPageFileError"]);
                }

                await _indexService.StartAsync(Input, Output, inputFiles, Client);

                SnackbarService.ShowSnackbarSuccess(Language.Instance["VideoOrganizationIndexPageProcessEnd"]);

                FileGridInputEnable = false;
                FileGridOutputEnable = true;
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