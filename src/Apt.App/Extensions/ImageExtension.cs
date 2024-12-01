using System.Windows.Media.Imaging;

namespace Apt.App.Extensions
{
    public static class ImageExtension
    {
        public static BitmapSource ToImage(this string path) => new BitmapImage(new Uri(path));

        public static BitmapSource ToImage(this Stream stream)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

        public static BitmapSource ToImage(this byte[] byteArray)
        {
            using var stream = new MemoryStream(byteArray);
            return stream.ToImage();
        }

        public static void ToPng(this BitmapSource image, string path)
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using var fileStream = new FileStream(path, FileMode.Create);
            encoder.Save(fileStream);
        }
    }
}
