namespace Useless.Mobile.Models
{
    public sealed class Expense
    {
        public string ExpenseId { get; set; }
        public string ParentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime EntryTime { get; set; }
    }
}
