using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseLess.Messages
{
    public static class ReadModels
    {
        public class Budget 
        {
            public Guid BudgetId { get; set; }
            public string  Name { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public decimal Income { get; set; }
            public decimal Outgo { get; set; }
            public decimal Expense { get; set; }
            public DateTime EntryTime { get; set; }
        }
        public class Income
        {
            public Guid IncomeId { get; set; }
            public Guid ParentId { get; set; }
            public decimal Amount { get; set; }
            public string Type { get; set; }
            public DateTime EntryTime { get; set; }
        }
        public class Outgo 
        {
            public Guid OutgoId { get; set; }
            public Guid ParentId { get; set; }
            public decimal Amount { get; set; }
            public string Type { get; set; }
            public DateTime EntryTime { get; set; }
        }
        public class Expense 
        {
            public Guid ExpenseId { get; set; }
            public Guid ParentId { get; set; }
            public decimal Amount { get; set; }
            public DateTime EntryTime { get; set; }
        }
        public class Period
        {
            public Guid PeriodId { get; set; }
            public Guid ParentId { get; set; }
            public DateTime Start { get; set; }
            public DateTime Stop { get; set; }
            public string State { get; set; }
            public string Type { get; set; }
        }
    }
}
