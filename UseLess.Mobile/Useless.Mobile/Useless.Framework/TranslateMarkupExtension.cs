using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Useless.Framework
{
    [ContentProperty("Text")]
    public sealed class TranslateMarkupExtension : IMarkupExtension
    {
        public string Text { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return Text.Translate();
        }
    }
    
}
