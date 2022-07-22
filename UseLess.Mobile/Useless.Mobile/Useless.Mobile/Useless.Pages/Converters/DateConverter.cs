using System;
using System.Globalization;
using Useless.Framework;
using Xamarin.Forms;

namespace Useless.Pages.Converters
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
            throw new NotImplementedException();
        }
    }
}
