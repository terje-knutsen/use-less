using UseLess.Domain.Values;
using UseLess.Framework;
using UseLess.Messages;

namespace UseLess.Domain.Entities
{
    public sealed class Expense : Entity<ExpenseId>
    {
        private Expense(Action<object> applier) : base(applier) { }
        public Money Amount { get; private set; }
        public EntryTime EntryTime { get; private set; }
        internal void ChangeAmount(Money amount, EntryTime entryTime)
            => Apply(new Events.ExpenseAmountChanged(Id,amount, entryTime));
        protected override void When(object @event)
        {
            switch (@event) 
            {
                case Events.ExpenseAddedToBudget e:
                    Id = ExpenseId.From(e.Id);
                    Amount = Money.From(e.Amount);
                    EntryTime = EntryTime.From(e.EntryTime);
                    break;
                case Events.ExpenseAmountChanged e:
                    Amount = Money.From(e.Amount);
                    break;
            }
        }
        internal static Expense WithApplier(Action<object> applier)
            => new(applier);
    }
}
