using UseLess.Framework;

namespace UseLess.Domain.Values
{
    public sealed class BudgetId : Value<BudgetId>
    {
        private readonly Guid value;
        private BudgetId(Guid value)=> this.value = value;
        protected override bool CompareProperties(BudgetId? other)
        => value == other?.value;
        public static BudgetId From(Guid guid)=> new(guid);
        public static BudgetId From(string value)=> new(Guid.Parse(value));
        public static implicit operator Guid(BudgetId self)=> self?.value ?? Guid.Empty;

        public override string ToString()
        => value.ToString();
    }
}
