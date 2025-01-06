using Apt.App.ViewModels.Pages.Video.Convert3d;
using Wpf.Ui.Abstractions.Controls;

namespace Apt.App.Views.Pages.Video.Convert3d
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
