using UseLess.Framework;
using UseLess.Messages;

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
                throw Exceptions.BudgetNameException.WithMessage("Name cannot be null");
            var name = value.Trim();
            if (name.Length > maxLength)
                throw Exceptions.BudgetNameException.WithMessage($"Number of chars in Name cannot exceed {maxLength}");
            this.value = name;
        }

        public static BudgetName From(string value) => new(value);

        public override CompareResult CompareTo(BudgetName? other)
        => value.CompareTo(other?.value) switch
            {
                -1 => CompareResult.LESS,
                0 => CompareResult.EQUAL,
                1 => CompareResult.GREATER,
                _ => CompareResult.NOT_EQUAL,
            };
        public static implicit operator string(BudgetName self) => self.value;
    }
}
