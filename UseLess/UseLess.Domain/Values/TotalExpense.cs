using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Domain.Entities;
using UseLess.Framework;

namespace UseLess.Domain.Values
{
    internal sealed class TotalExpense : Value<TotalExpense>
    {
        private readonly Money value;
        private TotalExpense(Money value)=> this.value = value;
        internal static TotalExpense From(IEnumerable<Expense> expenses) => new(Money.From(expenses.Sum(x => x.Amount)));
        public static implicit operator Money(TotalExpense totalExpense) => totalExpense?.value ?? Money.Zero;
        public static implicit operator decimal(TotalExpense self)=> self?.value ?? Money.Zero;
        public override CompareResult CompareTo(TotalExpense? other)
        => value.CompareTo(other?.value);
    }
}
