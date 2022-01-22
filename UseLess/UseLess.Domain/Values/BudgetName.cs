using UseLess.Framework;

namespace UseLess.Domain.Values
{
    public sealed class BudgetName : Value<BudgetName>
    {
        private readonly string value;

        public static BudgetName Empty => new(string.Empty);

        private BudgetName(string value) => this.value = value;
        protected override bool CompareProperties(BudgetName? other)
        => value == other?.value;
        public static BudgetName From(string value) => new(value);
        public static implicit operator string(BudgetName self)=> self.value;
    }
}
