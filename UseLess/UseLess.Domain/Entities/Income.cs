﻿using UseLess.Domain.Enumerations;
using UseLess.Domain.Values;
using UseLess.Framework;
using UseLess.Messages;

namespace UseLess.Domain.Entities
{
    public sealed class Income : Entity<IncomeId>
    {
        private Income(Action<object> applier) : base(applier)
        { }

        public Money Amount { get; private set; }
        public IncomeType Type { get; private set; }
        public EntryTime EntryTime { get; private set; }

        protected override void When(object @event)
        {
            switch (@event) 
            {
                case Events.IncomeAdded e:
                    Id = IncomeId.From(e.Id);
                    Amount = Money.From(e.Amount);
                    Type = Enumeration.FromString<IncomeType>(e.Type);
                    EntryTime = EntryTime.From(e.EntryTime);
                    break;
                case Events.IncomeAmountChanged e:
                    Amount = Money.From(e.Amount);
                    break;
                case Events.IncomeTypeChanged e:
                    Type = Enumeration.FromString<IncomeType>(e.IncomeType);
                    break;
            }
        }
        internal static Income WithApplier(Action<object>applier)
            => new(applier);
    }
}
