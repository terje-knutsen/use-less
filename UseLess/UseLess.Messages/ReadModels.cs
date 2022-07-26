using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseLess.Messages
{
    public static class ReadModels
    {
        public class Budget : IEquatable<Budget>
        {
            public Guid BudgetId { get; set; }
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

            public bool Equals(Budget other)
            => BudgetId == other.BudgetId;
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                var budget = obj as Budget ?? new Budget();
                return Equals(budget);
            }

            public override int GetHashCode()
            {
                int hashCode = -1968969412;
                hashCode = hashCode * -1521134295 + BudgetId.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
                hashCode = hashCode * -1521134295 + Start.GetHashCode();
                hashCode = hashCode * -1521134295 + End.GetHashCode();
                hashCode = hashCode * -1521134295 + Income.GetHashCode();
                hashCode = hashCode * -1521134295 + Outgo.GetHashCode();
                hashCode = hashCode * -1521134295 + Expense.GetHashCode();
                hashCode = hashCode * -1521134295 + EntryTime.GetHashCode();
                hashCode = hashCode * -1521134295 + Available.GetHashCode();
                hashCode = hashCode * -1521134295 + Limit.GetHashCode();
                hashCode = hashCode * -1521134295 + Left.GetHashCode();
                return hashCode;
            }
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
        public class IncomeType 
        {
            public string Type { get; set; }
            public override string ToString()
            => Type;
        }
        public class OutgoType
        {
            public string Type { get; set; }
            public override string ToString()
            => Type;
        }
    }
}
