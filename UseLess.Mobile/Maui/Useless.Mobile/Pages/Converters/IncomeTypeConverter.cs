
using System.Globalization;
using Useless.Mobile.Extensions;
using UseLess.Messages;


namespace Useless.Mobile.Pages.Converters
{
    public sealed class IncomeTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not ReadModels.IncomeType v) return value;
            return v.Name.Translate();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
