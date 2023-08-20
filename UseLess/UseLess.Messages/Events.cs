using System;

namespace UseLess.Messages
{
    public static class Events
    {
        public abstract class Event 
        {
            public Guid Id { get; }
            public DateTime EntryTime { get; }
            public Event(Guid id, DateTime entryTime)
            {
                Id = id;
                EntryTime = entryTime;
            }
        }
        [Serializable]
        public class BudgetCreated : Event 
        {
            public string State { get; }
            public string Name { get; }
            public BudgetCreated(Guid id, string name,string state, DateTime entryTime)
                :base(id,entryTime)
            {
                Name = name;
                State = state;
            }
        }
        [Serializable]
        public class IncomeAddedToBudget : Event
        {
            public Guid IncomeId { get; set; }
            public decimal Amount { get; }
            public int TypeId { get; }
            public string Type { get; }
            public IncomeAddedToBudget(Guid id, Guid incomeId, decimal amount, int typeId, string type, DateTime entryTime)
                :base(id,entryTime)
            {
                IncomeId = incomeId;
                Amount = amount;
                TypeId = typeId;
                Type = type;
            }
        }
        [Serializable]
        public class OutgoAddedToBudget : Event
        {
            public Guid OutgoId { get; }
            public decimal Amount { get; }
            public int TypeId { get; }
            public string Type { get; }
            public OutgoAddedToBudget(Guid id,Guid outgoId, decimal amount,int typeId, string type, DateTime entryTime)
                :base(id,entryTime)
            {
                OutgoId = outgoId;
                Amount = amount;
                TypeId = typeId;
                Type = type;
            }
        }
        [Serializable]
        public class ExpenseAddedToBudget : Event
        {
            public Guid ExpenseId { get; }
            public decimal Amount { get; }

            public ExpenseAddedToBudget(Guid id,Guid expenseId, decimal amount, DateTime entryTime)
                :base(id,entryTime)
            {
                ExpenseId = expenseId;
                Amount = amount;
            }
        }
        [Serializable]
        public class PeriodCreated : Event
        {
            public Guid PeriodId { get; set; }
            public DateTime Start { get; set; }
            public DateTime Stop { get; set; }
            public string State { get; set; }
            public string Type { get; set; }
            public PeriodCreated(Guid id, Guid periodId, DateTime start, DateTime stop, string state, string type, DateTime entryTime)
                : base(id,entryTime)
            {
                PeriodId = periodId;
                Start = start;
                Stop = stop;
                State = state;
                Type = type;
            }
        }
        [Serializable]
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
        [Serializable]
        public class PeriodTypeChanged : Event
        {
            public Guid PeriodId { get; }
            public string PeriodType { get; }
            public PeriodTypeChanged(Guid id,Guid periodId, string periodType, DateTime entryTime)
            :base(id,entryTime)
            {
                PeriodId = periodId;
                PeriodType = periodType;
            }
        }
        [Serializable]
        public class PeriodStateChanged : Event
        {
            public Guid PeriodId { get; }
            public string State { get; }
            public PeriodStateChanged(Guid id, Guid periodId, string state, DateTime entryTime)
            :base(id,entryTime)
            {
                PeriodId = periodId;
                State = state;
            }
        }
        [Serializable]
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
        [Serializable]
        public class IncomeTypeChanged : Event
        {
            public string IncomeType { get; }
            public Guid IncomeId { get; }
            public int IncomeTypeId { get; }

            public IncomeTypeChanged(Guid id,Guid incomeId, int incomeTypeId, string incomeType, DateTime entryTime)
            :base(id,entryTime)
            {
                IncomeId=incomeId;
                IncomeTypeId = incomeTypeId;
                IncomeType = incomeType;
            }
        }
        [Serializable]
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
        [Serializable]
        public class OutgoTypeChanged : Event
        {
            public string OutgoType { get; }
            public Guid OutgoId { get; }
            public int TypeId { get; }

            public OutgoTypeChanged(Guid id, Guid outgoId, int typeId, string outgoType, DateTime entryTime)
            :base(id,entryTime)
            {
                OutgoId=outgoId;
                TypeId = typeId;
                OutgoType = outgoType;
            }
        }
        [Serializable]
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
        [Serializable]
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
        [Serializable]
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
        [Serializable]
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
        [Serializable]
        public class BudgetDeleted : Event
        {
            public string State { get; }
            public BudgetDeleted(Guid id, string state, DateTime entryTime)
            :base(id,entryTime)
            {
                State = state;
            }
        }
        [Serializable]
        public class AmountLeftChanged : Event
        {
            public decimal AmountLeft { get; }
            public AmountLeftChanged(Guid id, decimal amountLeft, DateTime entryTime)
                :base(id,entryTime)
            {
                AmountLeft = amountLeft;
            }
        }
        [Serializable]
        public class AmountAvailableChanged : Event
        {
            public decimal AmountAvailable { get; }

            public AmountAvailableChanged(Guid id, decimal amountAvailable, DateTime entryTime) 
                : base(id, entryTime)
            {
                AmountAvailable = amountAvailable;
            }
        }
        [Serializable]
        public class AmountLimitChanged : Event
        {
            public decimal AmountLimit { get; }

            public AmountLimitChanged(Guid id, decimal amountLimit, DateTime entryTime)
            :base(id,entryTime)
            {
                AmountLimit = amountLimit;
            }
        }

        public class BudgetNameChanged
        {

            public BudgetNameChanged(Guid id, string name, DateTime entryTime)
            {
                Id = id;
                Name = name;
                EntryTime = entryTime;
            }

            public Guid Id { get; }
            public string Name { get; }
            public DateTime EntryTime { get; }
        }
    }
}
