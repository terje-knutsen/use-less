namespace Useless.Mobile.Models
{
    public sealed class Period
    {
        public string PeriodId { get; set; }
        public string ParentId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string State { get; set; }
        public string Type { get; set; }
    }
}
