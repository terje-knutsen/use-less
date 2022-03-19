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
            public BudgetCreated(Guid id, string name, DateTime entryDate)
                :base(id,entryDate)
            {
                Name = name;
            }
        }

        public class IncomeAddedToBudget : Event
        {
            public Guid IncomeId { get; set; }
            public decimal Amount { get; }
            public string Type { get; }
            public IncomeAddedToBudget(Guid id, Guid incomeId, decimal amount, string type, DateTime entryDate)
                :base(id,entryDate)
            {
                IncomeId = incomeId;
                Amount = amount;
                Type = type;
            }
        }

        public class OutgoAddedToBudget : Event
        {
            public Guid OutgoId { get; }
            public decimal Amount { get; }
            public string Type { get; }
            public OutgoAddedToBudget(Guid id,Guid outgoId, decimal amount, string type, DateTime entryDate)
                :base(id,entryDate)
            {
                OutgoId = outgoId;
                Amount = amount;
                Type = type;
            }
        }

        public class ExpenseAddedToBudget : Event
        {
            public Guid ExpenseId { get; }
            public decimal Amount { get; }

            public ExpenseAddedToBudget(Guid id,Guid expenseId, decimal amount, DateTime entryDate)
                :base(id,entryDate)
            {
                ExpenseId = expenseId;
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
            public Guid IncomeId { get; }

            public IncomeAmountChanged(Guid id, Guid incomeId, decimal amount, DateTime entryTime)
            :base(id,entryTime)
            {
                IncomeId = incomeId;
                Amount = amount;
            }
        }

        public class IncomeTypeChanged : Event
        {
            public string IncomeType { get; }
            public Guid IncomeId { get; }

            public IncomeTypeChanged(Guid id,Guid incomeId, string incomeType, DateTime entryTime)
            :base(id,entryTime)
            {
                IncomeId=incomeId;
                IncomeType = incomeType;
            }
        }

        public class OutgoAmountChanged : Event
        {
            public decimal Amount { get; }
            public Guid OutgoId { get; }

            public OutgoAmountChanged(Guid id, Guid outgoId, decimal amount, DateTime entryTime)
            : base(id,entryTime)
            {
                OutgoId = outgoId;
                Amount = amount;
            }
        }

        public class OutgoTypeChanged : Event
        {
            public string OutgoType { get; }
            public Guid OutgoId { get; }

            public OutgoTypeChanged(Guid id, Guid outgoId, string type, DateTime entryTime)
            :base(id,entryTime)
            {
                OutgoId=outgoId;
                OutgoType = type;
            }
        }

        public class ExpenseAmountChanged : Event
        {
            public decimal Amount { get; }
            public Guid ExpenseId { get; set; }

            public ExpenseAmountChanged(Guid id, Guid expenseId, decimal amount, DateTime entryTime)
            : base(id, entryTime)
            {
                ExpenseId = expenseId;
                Amount = amount;
            }
        }

        public class IncomeDeleted : Event
        {
            public Guid IncomeId { get; set; }
            public IncomeDeleted(Guid id, Guid incomeId, DateTime entryTime)
            :base(id,entryTime)
            {
                IncomeId = incomeId;
            }
        }

        public class OutgoDeleted : Event
        {
            public Guid OutgoId { get; set; }
            public OutgoDeleted(Guid id, Guid outgoId, DateTime entryTime)
            :base(id,entryTime)
            {
                OutgoId = outgoId;
            }
        }
        public class ExpenseDeleted : Event
        {
            public Guid ExpenseId { get; set; }
            public ExpenseDeleted(Guid id, Guid expenseId, DateTime entryTime)
            :base(id,entryTime)
            {
                ExpenseId = expenseId;
            }
        }
    }
}
