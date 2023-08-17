
using System.Globalization;


namespace Useless.Mobile.Pages.Converters
{
    public sealed class ZeroToEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(double.TryParse(value.ToString(),out var amount))
            return amount == 0d ? default(double?) : amount;
            return default(double?);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 0d;
            var parsed = double.TryParse(value.ToString(), out var amount);
            return parsed ? amount : 0d;
        }
    }
}
