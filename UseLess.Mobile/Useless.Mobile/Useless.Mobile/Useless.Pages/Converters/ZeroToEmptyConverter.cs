using System;
using System.Globalization;
using Xamarin.Forms;

namespace Useless.Pages.Converters
{
    public sealed class ZeroToEmptyConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double amount)
            {
                double? tmp = amount;
                return tmp == 0d ? default(double?) : amount;
            }
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
