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
        private IList<ReadModels.Income> incomes = new List<ReadModels.Income>();
        private async Task AddIncome(Events.IncomeAddedToBudget e)
        {
            await UpdateBudget(e.Id, b => b.Income += e.Amount);
            CreateIncome(e);
        }
        private async Task ChangeIncomeAmount(Events.IncomeAmountChanged e)
        {
            await UpdateBudget(e.Id, b => b.Income = (b.Income - e.OldAmount + e.Amount));
            await UpdateIncome(e.IncomeId, x => x.Amount = e.Amount);
        }
        private async Task DeleteIncome(Events.IncomeDeleted e) 
        {
            await UpdateBudget(e.Id, b => b.Income -= e.Amount);
            DeleteIncome(e.IncomeId);
        }
        private void CreateIncome(Events.IncomeAddedToBudget @event)
        => incomes.Add(new ReadModels.Income
        {
            ParentId = @event.Id,
            IncomeId = @event.IncomeId,
            Amount = @event.Amount,
            Type = @event.Type,
            EntryTime = @event.EntryTime
        });
        private async Task UpdateIncome(Guid id, Action<ReadModels.Income> action)
        {
            var income = incomes.First(x => x.IncomeId == id);
            await Task.Factory.StartNew(() => action(income));
        }
        private void DeleteIncome(Guid incomeId) 
        {
            var income = incomes.First(x => x.IncomeId == incomeId);
            incomes.Remove(income);
        }
    }
}
