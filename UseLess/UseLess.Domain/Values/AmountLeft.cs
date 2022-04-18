using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Framework;

namespace UseLess.Domain.Values
{
    public sealed class AmountLeft : Framework.Value<AmountLeft>
    {
        private readonly Money value;
        private AmountLeft(Money value) => this.value = value;
        public static AmountLeft From(Money value) => new AmountLeft(value);
        public static implicit operator Money(AmountLeft value) => value?.value ?? Money.Zero;
        public override CompareResult CompareTo(AmountLeft? other)
        => value.CompareTo(other?.value);
    }
}
