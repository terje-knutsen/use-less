using System;
using UseLess.Framework;

namespace UseLess.Domain.Values
{
    public sealed class Money : Value<Money>
    {
        private readonly decimal value;
        private Money(decimal value) => this.value = value;
        protected override bool CompareProperties(Money other)
        => value == other.value;
        public static Money From(decimal value) => new(value);
        public static implicit operator decimal(Money self)=> self.value;
    }
}
