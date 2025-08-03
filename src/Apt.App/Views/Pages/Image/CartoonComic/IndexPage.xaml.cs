using APT.App.ViewModels.Pages.Image.CartoonComic;
using Wpf.Ui.Abstractions.Controls;

namespace APT.App.Views.Pages.Image.CartoonComic
{
    public partial class IndexPage : INavigableView<IndexPageViewModel>
    {
        public IndexPageViewModel ViewModel { get; }

        public IndexPage(IndexPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
