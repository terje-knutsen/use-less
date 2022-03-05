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
        protected override void When(object @event)
        {
            switch (@event) 
            {
                case Events.ExpenseAdded e:
                    Id = ExpenseId.From(e.Id);
                    Amount = Money.From(e.Amount);
                    EntryTime = EntryTime.From(e.EntryTime);
                    break;
            }
        }
        internal static Expense WithApplier(Action<object> applier)
            => new(applier);
    }
}
