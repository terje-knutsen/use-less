using UseLess.Domain.Enumerations;
using UseLess.Domain.Values;
using UseLess.Framework;
using UseLess.Messages;

namespace UseLess.Domain.Entities
{
    public sealed class Period : Entity<PeriodId>
    {
        private Period(Action<object> applier) : base(applier) { }

        public BudgetId ParentId { get; set; }
        public PeriodState  State { get; private set; }
        public PeriodType Type { get; private set; }
        public StartTime Start { get; private set; }
        public StopTime Stop { get; private set; }
        public int ElapsedDaysFromStart(ThresholdTime thresholdTime) 
        => (int)(((DateTime)thresholdTime - Start).TotalDays) + 1;
        public int TotalDays => (int)((DateTime)Stop - Start).TotalDays;

        internal void UpdateStop(StopTime stopTime,EntryTime entryTime)
        {
            Apply(new Events.PeriodStopChanged(ParentId,Id, stopTime,entryTime));
            Apply(new Events.PeriodStateChanged(Id, PeriodState.NonCyclic.Name, entryTime));
            if (Type != PeriodType.Undefined)
                Apply(new Events.PeriodTypeChanged(Id, PeriodType.Undefined.Name, entryTime));
        }
        internal void UpdateType(PeriodType periodType, EntryTime entryTime)
        {
            if(Type != periodType) 
            {
                Apply(new Events.PeriodTypeChanged(Id, periodType.Name, entryTime));
                if (periodType != PeriodType.Undefined)
                    Apply(new Events.PeriodStopChanged(ParentId,Id, StopTime.From(Start, periodType), entryTime));
            }
        }
        internal void UpdateState(PeriodState periodState, EntryTime entryTime)
        {
            if (State != periodState)
                Apply(new Events.PeriodStateChanged(Id, periodState.Name, entryTime));
        }

        protected override void When(object @event)
        {
           switch(@event)
            {
                case Events.PeriodCreated e:
                    Id = PeriodId.From(e.PeriodId);
                    ParentId = BudgetId.From(e.Id);
                    Start = StartTime.From(e.Start);
                    Type = Enumeration.FromString<PeriodType>(e.Type);
                    State = Enumeration.FromString<PeriodState>(e.State);
                    Stop = StopTime.From(e.Stop);
                    break;
                case Events.PeriodStopChanged e:
                    Stop = StopTime.From(e.StopTime);
                    break;
                case Events.PeriodTypeChanged e:
                    Type = Enumeration.FromString<PeriodType>(e.PeriodType);
                    break;
                case Events.PeriodStateChanged e:
                    State = Enumeration.FromString<PeriodState>(e.State);
                    break;
            }
        }
        public static Period WithApplier(Action<object> applier)
            => new(applier);
    }
}
