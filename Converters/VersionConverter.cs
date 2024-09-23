using General.Apt.App.Services;
using System.Globalization;
using System.Windows.Data;

namespace General.Apt.App.Converters
{
    public class VersionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return DependencyProperty.UnsetValue;
            }
            return $"{value} V {AppHostService.EntryAssembly.Version}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
