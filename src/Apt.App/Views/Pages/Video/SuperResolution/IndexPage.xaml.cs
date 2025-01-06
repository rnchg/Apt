using Apt.App.ViewModels.Pages.Video.SuperResolution;
using Wpf.Ui.Abstractions.Controls;

namespace Apt.App.Views.Pages.Video.SuperResolution
{
    public partial class IndexPage : INavigableView<IndexPageViewModel>
    {
        public IndexPageViewModel ViewModel { get; }

        public IndexPage(IndexPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            IsVisibleChanged += (s, e) =>
            {
                if (!IsVisible) FileView.Pause();
            };
        }
    }
}
