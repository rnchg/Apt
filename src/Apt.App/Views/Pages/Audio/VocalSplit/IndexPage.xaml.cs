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

            IsVisibleChanged += (s, e) =>
            {
                if (!IsVisible) FileView.Pause();
            };
        }
    }
}
