using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Messages;

namespace Useless.AzureStore
{
    public static class BudgetModelExtensions
    {
        public static ReadModels.Budget ToModel(this Events.BudgetCreated @event)
            => new ReadModels.Budget
            {
                BudgetId = @event.Id,
                Name = @event.Name,
                EntryTime = @event.EntryTime,
            };
        public static ReadModels.Expense ToModel(this Events.ExpenseAddedToBudget @event)
            => new ReadModels.Expense
            {
                ExpenseId = @event.ExpenseId,
                ParentId = @event.Id,
                Amount = @event.Amount,
                EntryTime = @event.EntryTime
            };
    }
}
