namespace Useless.Mobile.Models
{
    internal sealed class Period
    {
        public string PeriodId { get; set; }
        public string ParentId { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public string State { get; set; }
        public string Type { get; set; }
    }
}
