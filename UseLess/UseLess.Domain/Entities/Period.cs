﻿using UseLess.Domain.Enumerations;
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
        internal void UpdateStop(StopTime stopTime,EntryTime entryTime)
        {
            Apply(new Events.PeriodStopChanged(Id, stopTime,entryTime));
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
                    Apply(new Events.PeriodStopChanged(Id, StopTime.From(Start, periodType), entryTime));
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
                case Events.PeriodAddedToBudget e:
                    Id = PeriodId.From(e.Id);
                    Start = StartTime.From(e.StartTime);
                    Type = PeriodType.Month;
                    State = PeriodState.Cyclic;
                    Stop = StopTime.From(Start, Type);
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
