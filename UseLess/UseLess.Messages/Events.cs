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

        public class PeriodSet : Event
        {
            public DateTime StartTime { get; }
            public DateTime StopTime { get; private set; }
            public string PeriodType { get; }
            public bool IsCyclic { get; }
            public PeriodSet(Guid periodId, DateTime startTime,DateTime stopTime, string type, bool isCyclical,DateTime entryDate)
                :base(periodId,entryDate)
            {   
                StartTime = startTime;
                StopTime = stopTime;
                PeriodType = type;
                IsCyclic = isCyclical;
            }

            public void SetStopTime(DateTime stopTime)
            => StopTime = stopTime;
        }
    }
}
