namespace Useless.Mobile.Models
{
    public sealed class Budget : IEquatable<Budget>
    {
        public string BudgetId { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public decimal Income { get; set; }
        public decimal Outgo { get; set; }
        public decimal Expense { get; set; }
        public DateTime EntryTime { get; set; }
        public decimal Available { get; set; }
        public decimal Limit { get; set; }
        public decimal Left { get; set; }

        public bool Equals(Budget other)
        => BudgetId == other.BudgetId;
        public override bool Equals(object obj)
        {
            if(obj is Budget x)
                return Equals(x);
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BudgetId, Left);
        }
    }
}
