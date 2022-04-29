using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Domain.Entities;
using UseLess.Framework;

namespace UseLess.Domain.Values
{
    internal sealed class TotalIncome : Value<TotalIncome>
    {

        private readonly Money value;
        public TotalIncome(Money value) => this.value = value;

        public static TotalIncome From(IEnumerable<Income> incomes) => new(Money.From(incomes.Sum(x => x.Amount)));
        public static implicit operator Money(TotalIncome self)=> self?.value ?? Money.Zero;
        public static implicit operator decimal(TotalIncome self) => self?.value ?? Money.Zero;
        public override CompareResult CompareTo(TotalIncome? other)
        => value.CompareTo(other?.value);

        public override string ToString()
        => value.ToString();
    }
}
