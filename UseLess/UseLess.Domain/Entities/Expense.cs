using UseLess.Domain.Values;
using UseLess.Framework;
using UseLess.Messages;

namespace UseLess.Domain.Entities
{
    public sealed class Expense : Entity<ExpenseId>
    {
        private Expense(Action<object> applier) : base(applier) { }
        public BudgetId ParentId { get; set; }
        public Money Amount { get; private set; }
        public EntryTime EntryTime { get; private set; }
        internal void ChangeAmount(Money amount, EntryTime entryTime)
            => Apply(new Events.ExpenseAmountChanged(ParentId,Id,amount, entryTime));
        protected override void When(object @event)
        {
            switch (@event) 
            {
                case Events.ExpenseAddedToBudget e:
                    ParentId = BudgetId.From(e.Id);
                    Id = ExpenseId.From(e.ExpenseId);
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
