using System.Globalization;

namespace Useless.Mobile.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToLocalDateString(this DateTime dateTime, CultureInfo cultureInfo)
        {
            var shortDatePattern = cultureInfo.DateTimeFormat.ShortDatePattern;
            var separator = cultureInfo.DateTimeFormat.DateSeparator;
            var format = shortDatePattern.Split(new[] { separator }, StringSplitOptions.None);
            var newPattern = string.Empty;
            for (var i = 0; i < format.Length; i++)
            {
                newPattern += format[i];
                if (format[i].Length == 1)
                    newPattern += format[i];
                if (i != format.Length - 1)
                    newPattern += separator;
            }
            return dateTime.ToString(newPattern, CultureInfo.InvariantCulture);
        }
    }
}
