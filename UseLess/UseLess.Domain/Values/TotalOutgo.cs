using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Domain.Entities;
using UseLess.Domain.Enumerations;
using UseLess.Framework;

namespace UseLess.Domain.Values
{
    internal sealed class TotalOutgo : Value<TotalOutgo>
    {
        private readonly Money value;
        private readonly (Money money, OutgoType type, EntryTime entryTime)[] values;
        private TotalOutgo((Money money, OutgoType type, EntryTime entryTime)[] values)
        : this(Money.From(values.Sum(x => x.money)))
        {
            this.values = values;
        }
        private TotalOutgo(Money value) => this.value = value;
        internal static TotalOutgo From(IEnumerable<Outgo> outgos) => new(outgos.Select(x => (x.Amount, x.Type, x.EntryTime)).ToArray());
        internal static TotalOutgo From(Money value) => new TotalOutgo(value);
        public static implicit operator Money(TotalOutgo value) => value?.value ?? Money.Zero;
        public static implicit operator decimal(TotalOutgo value) => value?.value ?? Money.Zero;
        public override CompareResult CompareTo(TotalOutgo? other)
        => value.CompareTo(other?.value);

        internal TotalOutgo InPeriod(Period period)
        {
            var amount = Money.Zero;
            if(period != null)
            foreach(var outgo in values) 
            {
                switch (outgo.Item2.Name) 
                {
                  case "MONTHLY":
                      var monthlyCount = period.MonthlyCount(outgo.entryTime);
                      amount = outgo.money.Multiply(monthlyCount);
                      break;
                  case "WEEKLY":
                      var weeklyCount = period.WeeklyCount(outgo.entryTime);
                      amount = outgo.money.Multiply(weeklyCount);
                      break;
                  case "HALF_YEARLY":
                            var halfYearlyCount = period.HalfYearCount(outgo.entryTime);
                            amount = outgo.money.Multiply(halfYearlyCount);
                            break;
                        case "YEARLY":
                            var yearlyCount = period.YearlyCount(outgo.entryTime);
                            amount = outgo.money.Multiply(yearlyCount);
                            break;

                }
            }
            return TotalOutgo.From(Max(amount,value));
        }

        private Money Max(Money amount, Money value)
        => amount > value ? amount : value;

        public override string ToString()
        => value.ToString();
    }
}
