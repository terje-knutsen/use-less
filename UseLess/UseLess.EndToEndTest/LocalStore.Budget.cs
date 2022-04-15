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
        private IList<ReadModels.Budget> budgets = new List<ReadModels.Budget>();
        private async Task CreateBudget(Events.BudgetCreated e)
        {
            await Task.Factory.StartNew(() => budgets.Add(e.ToModel()));
        }
        private async Task UpdateBudget(Guid id, Action<ReadModels.Budget> action)
        {
            var budget = budgets.First(x => x.BudgetId == id);
            await Task.Factory.StartNew(() => action(budget));
        }
  
    }
}
