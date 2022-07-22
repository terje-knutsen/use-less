using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Useless.Framework
{
    public static class TranslateExtensions
    {

        public static string Translate(this string key)
        {
            if (key == null) return string.Empty;
            ResourceManager resourceManager = new("Useless.Framework.Resources.AppResource", typeof(TranslateExtensions).GetTypeInfo().Assembly);
            return resourceManager.GetString(key, CultureInfo.CurrentCulture);
        }
    }
}
