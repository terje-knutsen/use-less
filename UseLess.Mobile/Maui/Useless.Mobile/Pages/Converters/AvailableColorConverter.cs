using System.Globalization;

namespace Useless.Mobile.Pages.Converters
{
    public sealed class AvailableColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not decimal amount) return value;
            if (amount >= 0.0m) return Color.FromArgb("#d0e1f9");
            return Color.FromArgb("#e3255b");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
