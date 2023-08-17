
using System.Globalization;


namespace Useless.Mobile.Pages.Converters
{
    public sealed class ReverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not bool) return value;
            return !(bool)value;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not bool) return value;
            return !(bool)value;
        }
    }
}
