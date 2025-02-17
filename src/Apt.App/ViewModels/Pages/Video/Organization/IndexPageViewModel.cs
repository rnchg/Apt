using Apt.Core.Consts;
using Apt.Core.Enums;
using Apt.Core.Exceptions;
using Apt.Core.Models;
using Apt.Core.Services.Pages.Video.Organization;
using Apt.Core.Utility;
using Apt.Service.Controls.FileGrid;
using Apt.Service.Controls.RunMessage;
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
        private ObservableCollection<ComBoBoxItem<string>> _clientSource = [];

        [ObservableProperty]
        private ComBoBoxItem<string> _clientItem = null!;

        public string Client
        {
            get => ClientItem.Value;
            set => ClientItem = ClientSource.First(e => e.Value == value);
        }

        public override void OnInputChangedAction(string value) => GetFileGrids(AppConst.VideoExts);

        public override void OnOutputChangedAction(string value) => GetFileGrids(AppConst.VideoExts);

        public override void OnFileGridSwitchChangedAction(bool value) => GetFileGrids(AppConst.VideoExts);

        [ObservableProperty]
        private Uri? _fileViewSource = null!;

        public override void OnFileGridItemChangedAction(FileModel? value) => FileViewSource = Source.FileToUri(value?.FullName);

        public IndexPageViewModel(
            IServiceProvider serviceProvider,
            ISnackbarService snackbarService) :
            base(serviceProvider, snackbarService)
        {
            if (!IsInitialized) InitializeViewModel();
        }

        public override void InitializeViewModel()
        {
            InputSortSource =
            [
                new ComBoBoxItem<string>() { Text = Language.Instance["VideoOrganizationIndexPageInputSortName"], Value = "Name" },
                new ComBoBoxItem<string>() { Text = Language.Instance["VideoOrganizationIndexPageInputSortLastWriteTime"], Value = "LastWriteTime" },
                new ComBoBoxItem<string>() { Text = Language.Instance["VideoOrganizationIndexPageInputSortLength"], Value = "Length" }
            ];
            SortRuleSource =
            [
                new ComBoBoxItem<string>() { Text = Language.Instance["VideoOrganizationIndexPageInputSortRuleAsc"], Value = "Asc" },
                new ComBoBoxItem<string>() { Text = Language.Instance["VideoOrganizationIndexPageInputSortRuleDesc"], Value = "Desc" }
            ];
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
                StartEnabled = false;
                StopEnabled = true;
                OpenEnabled = true;

                //FileGridSwitch = false;

                if (!Directory.Exists(Input))
                {
                    throw new Exception(Language.Instance["VideoOrganizationIndexPageInputEmpty"]);
                }
                if (!Directory.Exists(Output))
                {
                    throw new Exception(Language.Instance["VideoOrganizationIndexPageOutputEmpty"]);
                }
                var inputFiles = GetInputFiles(Input).Select(e => e.FullName).ToArray();
                if (inputFiles.Length == 0)
                {
                    throw new Exception(Language.Instance["VideoOrganizationIndexPageInputFilesEmpty"]);
                }

                await _indexService.Start(Input, Output, inputFiles, Client);

                ProgressBarValue = ProgressBarMaximum;

                FileGridSwitch = true;

                SnackbarService.ShowSnackbarSuccess(Language.Instance["VideoOrganizationIndexPageProcessEnd"]);
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

        public new ObservableCollection<FileModel> GetInputFiles(string input, string[]? exts = null)
        {
            var files = base.GetInputFiles(input, exts).AsEnumerable();
            if (InputSort == "Name")
            {
                if (SortRule == "Asc")
                {
                    files = files.OrderBy(f => f.Name);
                }
                if (SortRule == "Desc")
                {
                    files = files.OrderByDescending(f => f.Name);
                }
            }
            if (InputSort == "LastWriteTime")
            {
                if (SortRule == "Asc")
                {
                    files = files.OrderBy(f => f.LastWriteTime);
                }
                if (SortRule == "Desc")
                {
                    files = files.OrderByDescending(f => f.LastWriteTime);
                }
            }
            if (InputSort == "Length")
            {
                if (SortRule == "Asc")
                {
                    files = files.OrderBy(f => f.Length);
                }
                if (SortRule == "Desc")
                {
                    files = files.OrderByDescending(f => f.Length);
                }
            }
            return new ObservableCollection<FileModel>(files);
        }
    }
}