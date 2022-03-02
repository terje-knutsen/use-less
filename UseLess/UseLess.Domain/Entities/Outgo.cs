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
    public sealed class Outgo : Entity<OutgoId>
    {
        public Outgo(Action<object> applier):base(applier)
        {}
        public Money Amount { get; private set; }
        public OutgoType Type { get; private set; }
        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.OutgoAdded e:
                    Id = OutgoId.From(e.Id);
                    Amount = Money.From(e.Amount);
                    Type = Enumeration.FromString<OutgoType>(e.Type);
                    break;
            }
        }
    }
}
