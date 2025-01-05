using Apt.App.ViewModels.Pages.Audio.VocalSplit;
using Wpf.Ui.Abstractions.Controls;

namespace Apt.App.Views.Pages.Audio.VocalSplit
{
    public partial class IndexPage : INavigableView<IndexPageViewModel>
    {
        public IndexPageViewModel ViewModel { get; }

        public IndexPage(IndexPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            _ = InitializeData();
        }

        public async Task InitializeData()
        {
            ViewModel.MessageAction += (message) =>
            {
                Message.Document.Blocks.Add(message);
                Message.ScrollToEnd();
                while (Message.Document.Blocks.Count > 100)
                {
                    Message.Document.Blocks.Remove(Message.Document.Blocks.FirstBlock);
                }
            };

            IsVisibleChanged += (s, e) =>
            {
                if (!IsVisible) ViewFileAudio.Pause();
            };

            await Service.Utility.Message.AddTextInfo(Core.Utility.Language.Instance["AudioVocalSplitHelp"], ViewModel.MessageAction);
        }
    }
}
