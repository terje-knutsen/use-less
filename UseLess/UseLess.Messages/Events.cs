using System;

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
            public string State { get; }
            public string Name { get; }
            public BudgetCreated(Guid id, string name,string budgetState, DateTime entryDate)
                :base(id,entryDate)
            {
                Name = name;
                State = budgetState;
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
        public class PeriodCreated : Event
        {
            public Guid PeriodId { get; set; }
            public DateTime Start { get; set; }
            public DateTime Stop { get; set; }
            public string State { get; set; }
            public string Type { get; set; }
            public PeriodCreated(Guid id, Guid periodId, DateTime startTime, DateTime stopTime, string state, string type, DateTime entryDate)
                : base(id,entryDate)
            {
                PeriodId = periodId;
                Start = startTime;
                Stop = stopTime;
                State = state;
                Type = type;
            }
        }

        public class PeriodStopChanged : Event
        {
            public Guid PeriodId { get; set; }
            public DateTime StopTime { get; set; }
            public PeriodStopChanged(Guid id,Guid periodId, DateTime stopTime, DateTime entryTime
                )
                : base(id,entryTime)
            {
                PeriodId = periodId;
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
            public decimal OldAmount { get; }
            public Guid IncomeId { get; }

            public IncomeAmountChanged(Guid id, Guid incomeId, decimal amount, decimal oldAmount, DateTime entryTime)
            :base(id,entryTime)
            {
                IncomeId = incomeId;
                Amount = amount;
                OldAmount = oldAmount;
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
            public decimal OldAmount { get; }
            public Guid OutgoId { get; }

            public OutgoAmountChanged(Guid id, Guid outgoId, decimal amount, decimal oldAmount, DateTime entryTime)
            : base(id,entryTime)
            {
                OutgoId = outgoId;
                Amount = amount;
                OldAmount = oldAmount;
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
            public decimal OldAmount { get; }
            public Guid ExpenseId { get; }

            public ExpenseAmountChanged(Guid id, Guid expenseId, decimal amount,decimal oldAmount, DateTime entryTime)
            : base(id, entryTime)
            {
                ExpenseId = expenseId;
                Amount = amount;
                OldAmount = oldAmount;
            }
        }

        public class IncomeDeleted : Event
        {
            public Guid IncomeId { get; }
            public decimal Amount { get; }
            public IncomeDeleted(Guid id, Guid incomeId,decimal amount, DateTime entryTime)
            :base(id,entryTime)
            {
                IncomeId = incomeId;
                Amount = amount;
            }
        }

        public class OutgoDeleted : Event
        {
            public Guid OutgoId { get; }
            public decimal Amount { get; }
            public OutgoDeleted(Guid id, Guid outgoId, decimal amount, DateTime entryTime)
            :base(id,entryTime)
            {
                OutgoId = outgoId;
                Amount = amount;
            }
        }
        public class ExpenseDeleted : Event
        {
            public Guid ExpenseId { get; }
            public decimal Amount { get; }
            public ExpenseDeleted(Guid id, Guid expenseId, decimal amount, DateTime entryTime)
            :base(id,entryTime)
            {
                ExpenseId = expenseId;
                Amount = amount;
            }
        }
        public class BudgetDeleted : Event
        {
            public string State { get; }
            public BudgetDeleted(Guid id, string budgetState, DateTime entryTime)
            :base(id,entryTime)
            {
                State = budgetState;
            }
        }

        public class AmountLeftChanged : Event
        {
            public decimal AmountLeft { get; }
            public AmountLeftChanged(Guid id, decimal amountLeft, DateTime entryTime)
                :base(id,entryTime)
            {
                AmountLeft = amountLeft;
            }
        }

        public class AmountAvailableChanged : Event
        {
            public decimal AmountAvailable { get; }

            public AmountAvailableChanged(Guid id, decimal amountAvailable, DateTime entryDate) 
                : base(id, entryDate)
            {
                AmountAvailable = amountAvailable;
            }
        }

        public class AmountLimitChanged : Event
        {
            public decimal AmountLimit { get; }

            public AmountLimitChanged(Guid id, decimal amountLimit, DateTime entryTime)
            :base(id,entryTime)
            {
                AmountLimit = amountLimit;
            }
        }
    }
}
