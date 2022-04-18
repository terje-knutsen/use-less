using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Framework;

namespace UseLess.Domain.Values
{
    public sealed class AmountLimit : Value<AmountLimit>
    {
        private readonly Money value;
        private AmountLimit(Money value) => this.value = value;
        public static AmountLimit From(Money value) => new AmountLimit(value);
        public static implicit operator decimal(AmountLimit self) => self?.value ?? Money.Zero;
        public static implicit operator Money(AmountLimit self) => self?.value ?? Money.Zero;
        public override CompareResult CompareTo(AmountLimit? other)
        => value.CompareTo(other?.value);
    }
}
