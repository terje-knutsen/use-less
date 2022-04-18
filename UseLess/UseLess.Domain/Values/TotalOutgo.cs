using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Domain.Entities;
using UseLess.Framework;

namespace UseLess.Domain.Values
{
    internal sealed class TotalOutgo : Value<TotalOutgo>
    {
        private readonly Money value;
        private TotalOutgo(Money value)=> this.value = value;
        internal static TotalOutgo From(IEnumerable<Outgo> outgos) => new(Money.From(outgos.Sum(x => x.Amount)));
        public static implicit operator Money(TotalOutgo value) => value?.value ?? Money.Zero;
        public static implicit operator decimal(TotalOutgo value) => value?.value ?? Money.Zero;
        public override CompareResult CompareTo(TotalOutgo? other)
        => value.CompareTo(other?.value);
    }
}
