using System;
using System.Collections.Generic;
using System.Linq;
using UseLess.Messages;

namespace Useless.ApplicationService.InMemory
{
    internal class Outgos
    {
        private static readonly Outgos instance = new();
        private static readonly List<ReadModels.Outgo> outgos;
        static Outgos()
        {
            outgos = new List<ReadModels.Outgo>
            {
                new ReadModels.Outgo{Amount = 23, OutgoId = Guid.NewGuid(),ParentId = Budgets.First, Type = new ReadModels.OutgoType{Id = 4,Type = "WEEKLY"}},
                new ReadModels.Outgo{Amount = 84, OutgoId = Guid.NewGuid(), ParentId = Budgets.Last, Type = new ReadModels.OutgoType{Id = 2,Type = "YEARLY"}}
            };
        }
        private Outgos() { }
        public static Outgos Instance => instance;
        public ReadModels.Outgo this[Guid id]
            => outgos.Find(x => x.OutgoId == id);
        public IEnumerable<ReadModels.OutgoType> Types => new List<ReadModels.OutgoType>
        {
            new ReadModels.OutgoType{Id = 1,Type = "UNEXPECTED"},
             new ReadModels.OutgoType{Id = 2,Type = "YEARLY"},
              new ReadModels.OutgoType{Id = 3,Type = "MONTHLY"},
               new ReadModels.OutgoType{Id = 4,Type = "WEEKLY"},
                new ReadModels.OutgoType{Id = 5,Type = "HALF_YEARLY"},
                 new ReadModels.OutgoType{Id = 6,Type = "ONCE"},
        };
        public IEnumerable<ReadModels.Outgo> All(Guid budgetId) => outgos.Where(x => x.ParentId == budgetId);
        public void Add(ReadModels.Outgo item) => outgos.Add(item);
        public void Remove(ReadModels.Outgo item)=> outgos.Remove(item);
        public void Remove(Guid id) 
        {
            var item = this[id];
            if (item != null)
                outgos.Remove(item);
        }
    }
}
