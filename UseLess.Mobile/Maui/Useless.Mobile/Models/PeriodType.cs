using Useless.Mobile.Extensions;

namespace Useless.Mobile.Models
{
    public sealed class PeriodType
    {
        public string Type { get; set; }
        public string DisplayName => Type.Translate();
    }
}
