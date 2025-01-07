﻿using Apt.App.ViewModels.Pages.Image.FaceRestoration;
using Wpf.Ui.Abstractions.Controls;

namespace Apt.App.Views.Pages.Image.FaceRestoration
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