using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Domain.Enumerations;
using UseLess.Domain.Values;
using UseLess.Framework;
using UseLess.Messages;

namespace UseLess.Domain.Entities
{
    public sealed class BudgetDetails : Entity<BudgetId>
    {
        public BudgetDetails(Action<object> applier,BudgetId budgetId) : base(applier)
        {
            Id = budgetId;
        }
        public AmountLeft AmountLeft { get; set; }
        public AmountLimit AmountLimit { get; set; }
        public AmountAvailable AmountAvailable { get; set; }

        protected override void When(object @event)
        {
            switch (@event) 
            {
                case Events.AmountLeftChanged e:
                    AmountLeft = AmountLeft.From(Money.From(e.AmountLeft));
                    break;
                case Events.AmountLimitChanged e:
                    AmountLimit = AmountLimit.From(Money.From(e.AmountLimit));
                    break;
                case Events.AmountAvailableChanged e:
                    AmountAvailable = AmountAvailable.From(Money.From(e.AmountAvailable));
                    break;
            }       
        }
        internal static BudgetDetails WithApplier(Action<object> applier, BudgetId id)
            => new(applier,id);

        internal void TryRecalculate(TotalIncome income, TotalOutgo outgo, TotalExpense expense, Period period, ThresholdTime thresholdTime)
        {
            var outgoInPeriod = outgo.InPeriod(period);
            SetAmountLeft(income, outgoInPeriod, expense,period);
            SetAmountLimit(period, income, outgoInPeriod);
            SetAmountAvailable(period,expense,thresholdTime);
        }
        private void SetAmountLeft(TotalIncome income, TotalOutgo outgo, TotalExpense expense,Period period)
        {
            var i = (Money)income - outgo - expense;
            if (i != AmountLeft)
                Apply(new Events.AmountLeftChanged(Id, i, EntryTime.Now));

        }
        private void SetAmountLimit(Period period, TotalIncome income, TotalOutgo outgo)
        {
            //TODO: use outgo in period
            var a = Money.Zero;
            if(period != null) 
            {
                a = Money.From(Math.Round(((decimal)(income - outgo)) / period.TotalDays));
            }
            if (a != AmountLimit)
                Apply(new Events.AmountLimitChanged(Id, a, EntryTime.Now));
        }

        private void SetAmountAvailable(Period period,TotalExpense expense,ThresholdTime thresholdTime)
        {
            var a = Money.Zero;
            if(period != null)
            {
                a = Money.From((period.ElapsedDaysFromStart(thresholdTime) * AmountLimit) - expense);
            }
            if (a != AmountAvailable)
                Apply(new Events.AmountAvailableChanged(Id, a, EntryTime.Now));
        }
    }
}
