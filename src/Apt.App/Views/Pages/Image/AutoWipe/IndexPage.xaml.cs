using Apt.App.ViewModels.Pages.Image.AutoWipe;
using Wpf.Ui.Abstractions.Controls;

namespace Apt.App.Views.Pages.Image.AutoWipe
{
    public partial class IndexPage : INavigableView<IndexPageViewModel>
    {
        public IndexPageViewModel ViewModel { get; }

        public IndexPage(IndexPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            ViewModel.GetMaskAction += FileView.GetImageMask;
            ViewModel.ClearMaskAction += FileView.ClearMask;
        }
    }
}
