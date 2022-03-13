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

        public class IncomeAddedToBudget : Event
        {
            public decimal Amount { get; }
            public string Type { get; }
            public IncomeAddedToBudget(Guid id, decimal amount, string type, DateTime entryDate)
                :base(id,entryDate)
            {
                Amount = amount;
                Type = type;
            }
        }

        public class OutgoAddedToBudget : Event
        {
            public decimal Amount { get; }
            public string Type { get; }
            public OutgoAddedToBudget(Guid id, decimal amount, string type, DateTime entryDate)
                :base(id,entryDate)
            {
                Amount = amount;
                Type = type;
            }
        }

        public class ExpenseAddedToBudget : Event
        {
            public decimal Amount { get; }

            public ExpenseAddedToBudget(Guid id, decimal amount, DateTime entryDate)
                :base(id,entryDate)
            {
                Amount = amount;
            }
        }

        public class PeriodAddedToBudget : Event
        {
            public DateTime StartTime { get; }
            public DateTime StopTime { get; private set; }
            public PeriodAddedToBudget(Guid periodId, DateTime startTime,DateTime entryDate)
                :base(periodId,entryDate)
            {   
                StartTime = startTime;
            }

            public void SetStopTime(DateTime stopTime)
            => StopTime = stopTime;
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

        public class IncomeAmountChanged : Event
        {
            public decimal Amount { get; }
            public IncomeAmountChanged(Guid id, decimal amount, DateTime entryTime)
            :base(id,entryTime)
            {
                Amount = amount;
            }
        }

        public class IncomeTypeChanged : Event
        {
            public string IncomeType { get; }
            public IncomeTypeChanged(Guid id, string incomeType, DateTime entryTime)
            :base(id,entryTime)
            {
                IncomeType = incomeType;
            }
        }

        public class OutgoAmountChanged : Event
        {
            public decimal Amount { get; }
            public OutgoAmountChanged(Guid id, decimal amount, DateTime entryTime)
            : base(id,entryTime)
            {
                Amount = amount;
            }
        }

        public class OutgoTypeChanged : Event
        {
            public string OutgoType { get; }
            public OutgoTypeChanged(Guid id, string type, DateTime entryTime)
            :base(id,entryTime)
            {
                OutgoType = type;
            }
        }
    }
}
