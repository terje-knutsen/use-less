using System;
using System.Globalization;
using Useless.Framework;
using UseLess.Messages;
using Xamarin.Forms;

namespace Useless.Pages.Converters
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
