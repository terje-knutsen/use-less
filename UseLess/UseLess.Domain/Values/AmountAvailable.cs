using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Framework;

namespace UseLess.Domain.Values
{
    public sealed class AmountAvailable : Value<AmountAvailable>
    {
        private readonly Money value;
        private AmountAvailable(Money value)=> this.value = value;
        public static AmountAvailable From(Money value)=> new AmountAvailable(value);
        public static implicit operator Money(AmountAvailable self)=> self?.value??Money.Zero;
        public override CompareResult CompareTo(AmountAvailable? other)
        => value.CompareTo(other?.value);
    }
}
