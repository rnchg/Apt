using General.Apt.Service.Utility;
using Microsoft.Extensions.Logging;
using System.IO;
using Wpf.Ui.Controls;

namespace General.Apt.App.ViewModels.Pages.App
{
    public partial class HelpPageViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private string _language;

        [ObservableProperty]
        private string _message;

        public HelpPageViewModel()
        {
            if (!_isInitialized) InitializeViewModel();
        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized) InitializeViewModel();
            if (_language != Language.Instance.Name)
            {
                try
                {
                    var path = Language.Instance.Name == "zh-CN" ? "README.zh-CN.md" : "README.md";
                    Message = File.ReadAllText(path);
                    _language = Language.Instance.Name;
                }
                catch (Exception ex)
                {
                    Apt.App.App.Current.Logger.LogError(ex.Message);
                }
            }
        }

        public void OnNavigatedFrom() { }

        private void InitializeViewModel()
        {
            _isInitialized = true;
        }
    }
}
