using UseLess.Framework;

namespace UseLess.Domain.Values
{
    public sealed class Money : Value<Money>
    {
        private readonly decimal value;
        private Money(decimal value) => this.value = value;
      
        public static Money From(decimal value) => new(value);

        public override CompareResult CompareTo(Money? other)
        {
            if (other is null) return CompareResult.GREATER;
            if (value == other.value) return CompareResult.EQUAL;
            return value < other.value ? CompareResult.LESS : CompareResult.GREATER;
        }

        public static implicit operator decimal(Money self)=> self.value;
    }
}
