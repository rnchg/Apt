using APT.App.ViewModels.Pages.Audio.Denoise;
using Wpf.Ui.Abstractions.Controls;

namespace APT.App.Views.Pages.Audio.Denoise
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
