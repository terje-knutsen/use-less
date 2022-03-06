using UseLess.Domain.Enumerations;
using UseLess.Domain.Values;
using UseLess.Framework;
using UseLess.Messages;

namespace UseLess.Domain.Entities
{
    public sealed class Period : Entity<PeriodId>
    {
        private Period(Action<object> applier) : base(applier)
        { }
        public PeriodState  State { get; private set; }
        public PeriodType Type { get; private set; }
        public StartTime Start { get; private set; }
        public StopTime Stop { get; private set; }

        protected override void When(object @event)
        {
           switch(@event)
            {
                case Events.PeriodSet e:
                    Id = PeriodId.From(e.Id);
                    Start = StartTime.From(e.StartTime);
                    Type = Enumeration.FromString<PeriodType>(e.PeriodType);
                    State = e.IsCyclic ? PeriodState.Cyclic : PeriodState.NonCyclic;
                    TryUpdateStopTime(e);
                    Stop = StopTime.From(e.StopTime);
                    break;
            }
        }

        private void TryUpdateStopTime(Events.PeriodSet e)
        {
            if (StopTime.From(e.StopTime).IsEmpty) 
            {
                e.SetStopTime(StopTime.From(Start, Type));
            }
        }

        public static Period WithApplier(Action<object> applier)
            => new(applier);

    }
}
