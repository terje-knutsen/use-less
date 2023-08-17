using System.Globalization;
using Useless.Mobile.Extensions;

namespace Useless.Mobile.Pages.Converters
{
    internal sealed class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not DateTime) return value;
            var date = (DateTime)value;
            return date.ToLocalDateString(culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
