using General.Apt.App.ViewModels.Pages.Video.ColorRestoration;
using Wpf.Ui.Controls;

namespace General.Apt.App.Views.Pages.Video.ColorRestoration
{
    public partial class IndexPage : INavigableView<IndexViewModel>
    {
        public IndexViewModel ViewModel { get; }

        public IndexPage(IndexViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            _ = InitializeData();
        }

        public async Task InitializeData()
        {
            Message.Document.Blocks.Clear();

            ViewModel.MessageAction += (message) =>
            {
                Message.Document.Blocks.Add(message);
                Message.ScrollToEnd();
                while (Message.Document.Blocks.Count > 100)
                {
                    Message.Document.Blocks.Remove(Message.Document.Blocks.FirstBlock);
                }
            };

            await Utility.Message.AddTextInfo(Service.Utility.Language.Instance["VideoColorRestorationHelp"], ViewModel.MessageAction);
        }
    }
}
