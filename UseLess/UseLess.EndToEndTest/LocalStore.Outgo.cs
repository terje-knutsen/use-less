using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Messages;

namespace UseLess.EndToEndTest
{
    public partial class LocalStore
    {
        private readonly IList<ReadModels.Outgo> outgos = new List<ReadModels.Outgo>();
        private async Task AddOutgo(Events.OutgoAddedToBudget e) 
        {
            await UpdateBudget(e.Id, b => b.Outgo += e.Amount);
            CreateOutgo(e);
        }
        private async Task ChangeOutgoAmount(Events.OutgoAmountChanged e) 
        {
            await UpdateBudget(e.Id, b => b.Outgo = (b.Outgo - e.OldAmount + e.Amount));
            await UpdateOutgo(e.OutgoId, x => x.Amount = e.Amount);
        }
        private async Task ChangeOutgoType(Events.OutgoTypeChanged e)
            => await UpdateOutgo(e.OutgoId, x => x.Type = e.OutgoType);
        private async Task DeleteOutgo(Events.OutgoDeleted e) 
        {
            await UpdateBudget(e.Id, b => b.Outgo -= e.Amount);
            DeleteOutgo(e.OutgoId);
        }

        private void CreateOutgo(Events.OutgoAddedToBudget e)
        => outgos.Add(new ReadModels.Outgo 
        {
            OutgoId = e.OutgoId,
            ParentId = e.Id,
            Amount = e.Amount,
            Type = e.Type,
            EntryTime = e.EntryTime
        });
        private async Task UpdateOutgo(Guid id, Action<ReadModels.Outgo> action) 
        {
            var outgo = outgos.First(x => x.OutgoId == id);
            await Task.Factory.StartNew(() => action(outgo));
        }
        private void DeleteOutgo(Guid outgoId) 
        {
            var outgo = outgos.First(x => x.OutgoId == outgoId);
            outgos.Remove(outgo);
        }

    }
}
