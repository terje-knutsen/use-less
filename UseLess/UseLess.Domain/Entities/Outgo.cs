using UseLess.Domain.Enumerations;
using UseLess.Domain.Values;
using UseLess.Framework;
using UseLess.Messages;

namespace UseLess.Domain.Entities
{
    public sealed class Outgo : Entity<OutgoId>
    {
        private Outgo(Action<object> applier):base(applier)
        {}
        public BudgetId ParentId { get; set; }
        public Money Amount { get; private set; }
        public OutgoType Type { get; private set; }
        internal void ChangeAmount(Money amount, EntryTime entryTime)
        => Apply(new Events.OutgoAmountChanged(ParentId, Id, amount, Amount, entryTime));
        internal void ChangeType(OutgoType type, EntryTime entryTime)
        => Apply(new Events.OutgoTypeChanged(ParentId,Id, type.Name, entryTime));
        internal void Delete(EntryTime entryTime)
        => Apply(new Events.OutgoDeleted(ParentId, Id, Amount, entryTime));
        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.OutgoAddedToBudget e:
                    ParentId = BudgetId.From(e.Id);
                    Id = OutgoId.From(e.OutgoId);
                    Amount = Money.From(e.Amount);
                    Type = Enumeration.FromString<OutgoType>(e.Type);
                    break;
                case Events.OutgoAmountChanged e:
                    Amount = Money.From(e.Amount);
                    break;
                case Events.OutgoTypeChanged e:
                    Type = Enumeration.FromString<OutgoType>(e.OutgoType);
                    break;
            }
        }
        internal static Outgo WithApplier(Action<object> applier)
            => new(applier);

 
    }
}
