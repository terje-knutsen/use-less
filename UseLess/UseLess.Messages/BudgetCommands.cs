using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseLess.Messages
{
    public static class BudgetCommands
    {
        public static class V1 
        {
            public abstract class Command 
            {
                public Guid BudgetId { get; set; } 
            }
            public class Create : Command
            {
                public string Name { get; set; }
            }
            public class AddIncome :Command
            {
                public Guid IncomeId { get; set; }
                public decimal Amount { get; set; }
                public string Type { get; set; }
            }
            public class AddOutgo :Command
            {
                public Guid OutgoId { get; set; }
                public decimal Amount { get; set; }
                public string Type { get; set; }
            }
            public class AddExpense : Command
            {
                public Guid ExpenseId { get; set; }
                public decimal Amount { get; set; }
            }
            public class ChangeIncomeAmount :Command
            {
                public Guid IncomeId { get; set; }
                public decimal Amount { get; set; }
            }
            public class ChangeIncomeType : Command
            {
                public Guid IncomeId { get; set; }
                public string Type { get; set; }
            }
            public class ChangeOutgoAmount : Command 
            {
                public Guid OutgoId { get; set; }
                public decimal Amount { get; set; }
            }
            public class ChangeOutgoType : Command
            {
                public Guid OutgoId { get; set; }
                public string Type { get; set; }
            }
            public class ChangeExpenseAmount : Command
            {
                public Guid ExpenseId { get; set; }
                public decimal Amount { get; set; }
            }
            public class AddPeriod : Command
            {
                public Guid PeriodId { get; set; }
                public DateTime StartTime { get; set; }
            }
            public class SetPeriodStopTime : Command 
            {
                public Guid PeriodId { get; set; }
                public DateTime StopTime { get; set; }
            }
            public class SetPeriodType : Command
            {
                public Guid PeriodId { get; set; }
                public string PeriodType { get; set; }
            }
            public class SetPeriodState : Command
            {
                public Guid PeriodId { get; set; }
                public string PeriodState { get; set; }
            }
        }
    }
}
