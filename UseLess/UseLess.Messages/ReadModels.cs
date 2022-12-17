using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace UseLess.Messages
{
    public static class ReadModels
    {
        public class ReadModel<T> 
        {
            public ReadModel(string id,T body)
            {
                this.id = id;
                this.body = body;
            }
            public string id { get; set; }
            public T body { get; }
        }
        [Serializable]
        public record Budget
        {
            public string id { get; set; }
            public string BudgetId { get; set; }
            public string  Name { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public decimal Income { get; set; }
            public decimal Outgo { get; set; }
            public decimal Expense { get; set; }
            public DateTime EntryTime { get; set; }
            public decimal Available { get; set; }
            public decimal Limit { get; set; }
            public decimal Left { get; set; }
        }
        [Serializable]
        public record Income
        {
            public string id { get; set; }
            public string IncomeId { get; set; }
            public string ParentId { get; set; }
            public decimal Amount { get; set; }
            public ReadModels.IncomeType Type { get; set; }
            public DateTime EntryTime { get; set; }
        }
        [Serializable]
        public record Outgo 
        {
            public string id { get; set; }
            public string OutgoId { get; set; }
            public string ParentId { get; set; }
            public decimal Amount { get; set; }
            public ReadModels.OutgoType Type { get; set; }
            public DateTime EntryTime { get; set; }
        }
        [Serializable]
        public record Expense 
        {
            public string id { get; set; }
            public string ExpenseId { get; set; }
            public string ParentId { get; set; }
            public decimal Amount { get; set; }
            public DateTime EntryTime { get; set; }
        }
        [Serializable]
        public record Period
        {
            public string id { get; set; }
            public string PeriodId { get; set; }
            public string ParentId { get; set; }
            public DateTime Start { get; set; }
            public DateTime Stop { get; set; }
            public string State { get; set; }
            public string Type { get; set; }
        }
        [Serializable]
        public record IncomeType
        {
            public string id { get; set; }
            public int IncomeTypeId { get; set; }
            public string Name { get; set; }
        }
        [Serializable]
        public record OutgoType
        {
            public string id { get; set; }
            public int OutgoTypeId { get; set; }
            public string Name { get; set; }
        }
    }
}
