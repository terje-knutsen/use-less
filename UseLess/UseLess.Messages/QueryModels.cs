using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseLess.Messages
{
    public static class QueryModels
    {
        public class GetBudget 
        {
            public Guid BudgetId { get; set; }
        }
        public class GetIncomes 
        {
            public Guid BudgetId { get; set; }
        }
        public class GetIncome 
        {
            public Guid IncomeId { get; set; }
        }
        public class GetOutgos 
        {
            public Guid BudgetId { get; set; }
        }
        public class GetOutgo 
        {
            public Guid OutgoId { get; set; }
        }
        public class GetExpenses 
        {
            public Guid BudgetId { get; set; }
        }
        public class GetExpense 
        {
            public Guid ExpenseId { get; set; }
        }
        public class GetPeriod 
        {
            public Guid PeriodId { get; set; }
        }
    }
}