using System;
using System.Collections.Generic;
using UseLess.Messages;

namespace Useless.ApplicationService.InMemory
{
    internal class Budgets
    {
        internal static Guid First = Guid.NewGuid();
        internal static Guid Last = Guid.NewGuid();
        private static readonly Budgets instance = new Budgets(); 
        static Budgets()
        {
            budgets = new List<ReadModels.Budget> 
            {
                   new ReadModels.Budget{
                        BudgetId = First,
                        Available = 559.22m,
                        End = DateTime.Now.AddDays(13).Date,
                        Start = DateTime.Now.AddDays(-16).Date,
                        EntryTime = DateTime.Now.AddDays(-17),
                        Expense = 2455.00m,
                        Income = 10780.00m,
                        Left = 5305.00m,
                        Limit = 559.00m,
                        Name = "Juni-juli",
                        Outgo = 0.00m
                    },
                    new ReadModels.Budget{
                        BudgetId = Last,
                        Available = 533.22m,
                        End = DateTime.Now.AddDays(13).Date,
                        Start = DateTime.Now.AddDays(-16).Date,
                        EntryTime = DateTime.Now.AddDays(-17),
                        Expense = 344.00m,
                        Income = 1500.00m,
                        Left = 1200.00m,
                        Limit = 55.00m,
                        Name = "Bil",
                        Outgo = 0.00m
                    }
            };
        }
        private Budgets() 
        {}
        public static Budgets Instance => instance;
        public ReadModels.Budget this[Guid id]
            => budgets.Find(x => x.BudgetId == id);
        public IEnumerable<ReadModels.Budget> All => budgets;
        public void Add(ReadModels.Budget item)
            => budgets.Add(item);
        public void Remove(ReadModels.Budget item)
            => budgets.Remove(item);
        public void Remove(Guid id) 
        {
            var item = this[id];
            if (item != null)
                budgets.Remove(item);
        }
        private static readonly List<ReadModels.Budget> budgets;
    }
}
