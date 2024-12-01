using Apt.App.Utility;
using Apt.App.ViewModels.Base;
using Apt.Service.Exceptions;
using Apt.Service.Models;
using Apt.Service.Services.Pages.Video.Organization;
using Apt.Service.Utility;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Apt.App.ViewModels.Pages.Video.Organization
{
    public partial class IndexViewModel : CommonViewModel
    {
        private IndexService _indexService = null!;

        [ObservableProperty]
        private ObservableCollection<ComBoBoxItem<string>> _clientSource = [];

        [ObservableProperty]
        private ComBoBoxItem<string> _clientItem = null!;

        public string Client
        {
            get => ClientItem.Value;
            set => ClientItem = ClientSource.First(e => e.Value == value);
        }

        public IndexViewModel()
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

                await _indexService.Start(Input, Output, InputSort, SortRule, Client);

                Message.ShowSnackbarSuccess(Language.Instance["VideoOrganizationIndexPageOperationCompleted"]);

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