using Apt.App.ViewModels.Pages.Image.FrameInterpolation;
using Wpf.Ui.Abstractions.Controls;

namespace Apt.App.Views.Pages.Image.FrameInterpolation
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
