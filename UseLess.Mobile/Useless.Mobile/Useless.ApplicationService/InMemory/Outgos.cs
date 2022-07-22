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
                new ReadModels.Outgo{Amount = 23, OutgoId = Guid.NewGuid(),ParentId = Budgets.First, Type = "WEEKLY"},
                new ReadModels.Outgo{Amount = 84, OutgoId = Guid.NewGuid(), ParentId = Budgets.Last, Type = "YEARLY"}
            };
        }
        private Outgos() { }
        public static Outgos Instance => instance;
        public ReadModels.Outgo this[Guid id]
            => outgos.Find(x => x.OutgoId == id);
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
