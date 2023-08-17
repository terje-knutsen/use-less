namespace Useless.Mobile.Models
{
    public sealed class Income
    {
        public string IncomeId { get; set; }
        public string ParentId { get; set; }
        public decimal Amount { get; set; }
        public IncomeType Type { get; set; }
        public DateTime EntryTime { get; set; }
    }
}
