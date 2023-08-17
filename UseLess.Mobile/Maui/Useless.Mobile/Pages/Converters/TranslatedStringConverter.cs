
using System.Globalization;
using Useless.Mobile.Extensions;

namespace Useless.Mobile.Pages.Converters
{
    public sealed class TranslatedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string type) return value;
            return type.Translate();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
