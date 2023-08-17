namespace Useless.Mobile.Models
{
    public sealed class Outgo
    {
        public string OutgoId { get; set; }
        public string ParentId { get; set; }
        public decimal Amount { get; set; }
        public OutgoType Type { get; set; }
        public DateTime EntryTime { get; set; }
    }
}
