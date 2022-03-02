using UseLess.Domain.Exceptions;
using UseLess.Framework;

namespace UseLess.Domain.Values
{
    public sealed class BudgetName : Value<BudgetName>
    {
        private const int maxLength = 45;
        private readonly string value;

        public static BudgetName Empty => new(string.Empty);

        private BudgetName(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new BudgetNameException("Name cannot be null");
            var name = value.Trim();
            if (name.Length > maxLength)
                throw new BudgetNameException($"Number of chars in Name cannot exceed {maxLength}");
            this.value = name;
        }
        protected override bool CompareProperties(BudgetName? other)
        => value == other?.value;
        public static BudgetName From(string value) => new(value);
        public static implicit operator string(BudgetName self) => self.value;
    }
}
