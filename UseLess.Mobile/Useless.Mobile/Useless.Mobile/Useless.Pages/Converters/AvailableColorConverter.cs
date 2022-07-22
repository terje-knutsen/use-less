using System;
using System.Globalization;
using Xamarin.Forms;

namespace Useless.Pages.Converters
{
    public sealed class AvailableColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not decimal amount) return value;
            if (amount >= 0.0m) return Color.FromHex("#d0e1f9");
            return Color.FromHex("#e3255b");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
