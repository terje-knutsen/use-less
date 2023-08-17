namespace Useless.Mobile.Extensions
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
