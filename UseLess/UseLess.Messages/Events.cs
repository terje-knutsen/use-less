namespace UseLess.Messages
{
    public static class Events
    {
        public abstract class Event 
        {
            public Guid Id { get; }
            public DateTime EntryTime { get; }
            public Event(Guid id, DateTime entryDate)
            {
                Id = id;
                EntryTime = entryDate;
            }
        }
        public class BudgetCreated : Event 
        {
            public string Name { get; }
            public BudgetCreated(Guid id, DateTime entryDate, string name)
                :base(id,entryDate)
            {
                Name = name;
            }
        }

        public class IncomeAdded : Event
        {
            public decimal Amount { get; }
            public string Type { get; }
            public IncomeAdded(Guid id, decimal amount, string type, DateTime entryDate)
                :base(id,entryDate)
            {
                Amount = amount;
                Type = type;
            }
        }

        public class OutgoAdded : Event
        {
            public decimal Amount { get; }
            public string Type { get; }
            public OutgoAdded(Guid id, decimal amount, string type, DateTime entryDate)
                :base(id,entryDate)
            {
                Amount = amount;
                Type = type;
            }
        }

        public class ExpenseAdded : Event
        {
            public decimal Amount { get; }

            public ExpenseAdded(Guid id, decimal amount, DateTime entryDate)
                :base(id,entryDate)
            {
                Amount = amount;
            }
        }

        public class PeriodAdded : Event
        {
            public DateTime StartTime { get; }
            public DateTime StopTime { get; private set; }
            public PeriodAdded(Guid periodId, DateTime startTime,DateTime entryDate)
                :base(periodId,entryDate)
            {   
                StartTime = startTime;
            }

            public void SetStopTime(DateTime stopTime)
            => StopTime = stopTime;
        }

        public class PeriodStopWasSet : Event
        {
            public DateTime StopTime { get; private set; }
            public PeriodStopWasSet(Guid periodId, DateTime stopTime, DateTime entryTime)
                :base(periodId,entryTime)
            {
                StopTime = stopTime;
            }
        }

        public class PeriodStopChanged : Event
        {
            public DateTime StopTime { get; set; }
            public PeriodStopChanged(Guid id, DateTime stopTime, DateTime entryTime
                )
                : base(id,entryTime)
            {
                StopTime = stopTime;
            }
        }

        public class PeriodTypeChanged : Event
        {
            public string PeriodType { get; }
            public PeriodTypeChanged(Guid id, string periodType, DateTime entryTime)
            :base(id,entryTime)
            {
                PeriodType = periodType;
            }
        }

        public class PeriodStateChanged : Event
        {
            public string State { get; }
            public PeriodStateChanged(Guid id, string state, DateTime entryTime)
            :base(id,entryTime)
            {
                State = state;
            }
        }

        public class PeriodTypeWasSet : Event
        {
            public string PeriodType { get; set; }
            public PeriodTypeWasSet(Guid periodId, string periodType, DateTime entryTime)
            :base(periodId,entryTime)
            {
                PeriodType = periodType;
            }
        }

        public class PeriodStateWasSet : Event
        {
            public string State { get; }
            public PeriodStateWasSet(Guid id, string periodState, DateTime entryTime)
            :base(id,entryTime)
            {
                State = periodState;
            }
        }
    }
}
