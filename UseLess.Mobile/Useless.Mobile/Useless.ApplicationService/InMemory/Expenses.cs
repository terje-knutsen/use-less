using System;
using System.Collections.Generic;
using System.Linq;
using UseLess.Messages;

namespace Useless.ApplicationService.InMemory
{
    internal class Expenses
    {
        private static readonly Expenses instance = new();
        private static readonly List<ReadModels.Expense> expenses;
        static Expenses() 
        {
            expenses = new List<ReadModels.Expense>
            {
                new ReadModels.Expense{Amount = 98, ExpenseId = Guid.NewGuid(),ParentId = Budgets.First, EntryTime = DateTime.Now.AddDays(-1)},
                new ReadModels.Expense{Amount = 32, ExpenseId = Guid.NewGuid(),ParentId = Budgets.First, EntryTime = DateTime.Now.AddHours(-2)},
                new ReadModels.Expense{Amount = 332, ExpenseId = Guid.NewGuid(), ParentId = Budgets.Last, EntryTime = DateTime.Now.AddHours(-12)}
            };

        }
        private Expenses() { }
        public static Expenses Instance => instance;
        public ReadModels.Expense this[Guid id]
            => expenses.Find(x => x.ExpenseId == id);
        public IEnumerable<ReadModels.Expense> All(Guid budgetId) => expenses.Where(x => x.ParentId == budgetId);
        public void Add(ReadModels.Expense item) => expenses.Add(item);
        public void Remove(ReadModels.Expense item) => expenses.Remove(item);
        public void Remove(Guid id)
        {
            var item = this[id];
            if (item != null)
                expenses.Remove(item);
        }
    }
}
