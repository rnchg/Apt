using Apt.App.ViewModels.Pages.Image.AutoWipe;
using System.Windows.Media.Imaging;
using Wpf.Ui.Abstractions.Controls;

namespace Apt.App.Views.Pages.Image.AutoWipe
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

            _ = InitializeMask();
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

            await Utility.Message.AddTextInfo(Service.Utility.Language.Instance["ImageAutoWipeHelp"], ViewModel.MessageAction);
        }

        public async Task InitializeMask()
        {
            await ViewModel.SetMaskFirst();

            ViewModel.MaskAction += () =>
            {
                var maskSource = maskImage.Source as BitmapImage;
                if (maskSource is null)
                {
                    throw new Exception(Service.Utility.Language.Instance["ImageAutoWipeIndexPageMaskEmpty"]);

                }
                var render = new RenderTargetBitmap(maskSource.PixelWidth, maskSource.PixelHeight, maskSource.DpiX, maskSource.DpiY, PixelFormats.Pbgra32);
                var visual = new DrawingVisual();
                using (var context = visual.RenderOpen())
                {
                    var brush = new VisualBrush(maskInkCanvas);
                    context.DrawRectangle(brush, null, new Rect(new Point(), new Size(maskSource.Width, maskSource.Height)));
                }
                render.Render(visual);
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(render));
                using var stream = new MemoryStream();
                encoder.Save(stream);
                return stream.ToArray();
            };
        }

        private void MaskClear_Click(object sender, RoutedEventArgs e)
        {
            maskInkCanvas.Strokes.Clear();
        }

        private void MaskSelect_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = Service.Utility.Language.Instance["ImageAutoWipeIndexPageMaskFileFilter"];
            if (openFileDialog.ShowDialog() is true)
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(openFileDialog.FileName);
                image.EndInit();
                ViewModel.InputImageFirst = image;
            }
        }
    }
}
