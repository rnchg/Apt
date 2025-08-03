using APT.App.ViewModels.Pages.Image.Matting;
using Wpf.Ui.Abstractions.Controls;

namespace APT.App.Views.Pages.Image.Matting
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
