using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Useless.Framework
{
    [ContentProperty("Text")]
    public sealed class TranslateMarkupExtension : IMarkupExtension
    {
        //const string ResourceId = "Useless.Framework.Resources.AppResource";
        public string Text { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return Text.Translate();
            //if (Text == null)
            //    return null;
            //ResourceManager resourceManager = new ResourceManager(ResourceId, typeof(TranslateMarkupExtension).GetTypeInfo().Assembly);
            //return resourceManager.GetString(Text, CultureInfo.CurrentCulture);
        }
    }
    
}
