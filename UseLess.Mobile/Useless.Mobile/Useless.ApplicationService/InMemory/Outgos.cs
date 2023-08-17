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
                new ReadModels.Outgo{Amount = 23, OutgoId = Guid.NewGuid().ToString(),ParentId = Budgets.First.ToString(), Type = new ReadModels.OutgoType{id = "4",Name = "WEEKLY"}},
                new ReadModels.Outgo{Amount = 84, OutgoId = Guid.NewGuid().ToString(), ParentId = Budgets.Last.ToString(), Type = new ReadModels.OutgoType{id = "2",Name = "YEARLY"}}
            };
        }
        private Outgos() { }
        public static Outgos Instance => instance;
        public ReadModels.Outgo this[Guid id]
            => outgos.Find(x => x.OutgoId.ToString() == id.ToString());
        public IEnumerable<ReadModels.OutgoType> Types => new List<ReadModels.OutgoType>
        {
            new ReadModels.OutgoType{id = "1",Name = "UNEXPECTED"},
             new ReadModels.OutgoType{id = "2",Name = "YEARLY"},
              new ReadModels.OutgoType{id = "3",Name = "MONTHLY"},
               new ReadModels.OutgoType{id = "4",Name = "WEEKLY"},
                new ReadModels.OutgoType{id = "5",Name = "HALF_YEARLY"},
                 new ReadModels.OutgoType{id = "6",Name = "ONCE"},
        };
        public IEnumerable<ReadModels.Outgo> All(Guid budgetId) => outgos.Where(x => x.ParentId.ToString() == budgetId.ToString());
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
