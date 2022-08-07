using System;
using System.Collections.Generic;
using System.Linq;
using UseLess.Messages;

namespace Useless.ApplicationService.InMemory
{
    internal class Incomes
    {
        private static readonly Incomes instance = new();
        private static readonly List<ReadModels.Income> incomes;

        static Incomes() 
        {
            incomes = new List<ReadModels.Income>
            {
                new ReadModels.Income
                {
                    Amount = 100,
                    EntryTime = DateTime.Now.AddDays(-1),
                    IncomeId = Guid.NewGuid(),
                    ParentId = Budgets.First,
                    Type = new ReadModels.IncomeType{Id = 1,Name = "SALARY"}
                },
                new ReadModels.Income{
                    Amount = 1000,
                    EntryTime = DateTime.Now.AddHours(-12),
                    IncomeId = Guid.NewGuid(),
                    ParentId = Budgets.Last,
                    Type = new ReadModels.IncomeType{Id = 2,Name = "BONUS"}
                }
            };
        }
        private Incomes() { }
        public static Incomes Instance => instance;
        public IEnumerable<ReadModels.IncomeType> Types => new List<ReadModels.IncomeType>
        {
            new ReadModels.IncomeType{Id = 1,Name = "SALARY"},
            new ReadModels.IncomeType{Id = 2,Name = "BONUS"},
            new ReadModels.IncomeType{Id = 3,Name = "PERKS"},
            new ReadModels.IncomeType{Id = 4,Name = "GAMBLING"},
            new ReadModels.IncomeType{Id = 5,Name = "GIFT"},
        };
        public ReadModels.Income this[Guid id]
            => incomes.Find(x => x.IncomeId == id);
        public IEnumerable<ReadModels.Income> All(Guid budgetId) => incomes.Where(x => x.ParentId == budgetId);
        public void Add(ReadModels.Income item)
            => incomes.Add(item);
        public void Remove(ReadModels.Income item)
            => incomes.Remove(item);
        public void Remove(Guid id)
        {
            var item = this[id];
            if(item != null)
                incomes.Remove(item);
        }
    }
}
