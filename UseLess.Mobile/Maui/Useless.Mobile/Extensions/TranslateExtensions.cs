using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Useless.Mobile.Extensions

{
    public static class TranslateExtensions
    {

        public static string Translate(this string key)
        {
            if (key == null) return string.Empty;
            ResourceManager resourceManager = new("Useless.Mobile.Resources.Strings.AppResource", typeof(TranslateExtensions).GetTypeInfo().Assembly);
            return resourceManager.GetString(key, CultureInfo.CurrentCulture);
        }
    }
}
