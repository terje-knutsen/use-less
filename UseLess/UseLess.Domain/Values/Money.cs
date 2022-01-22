using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Framework;

namespace UseLess.Domain.Values
{
    public sealed class Money : Value<Money>
    {
        private readonly decimal value;
        private Money(decimal value) => this.value = value;
        protected override bool CompareProperties(Money? other)
        => this.value == other?.value;
        public static Money From(decimal value) => new(value);
        public static implicit operator decimal(Money self)=> self.value;
        public static bool operator ==(Money a, Money b) => a.Equals(b);
        public static bool operator !=(Money a, Money b)=> !a.Equals(b);
    }
}
