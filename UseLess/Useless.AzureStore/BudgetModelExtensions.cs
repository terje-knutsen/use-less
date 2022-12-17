using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
                id = @event.Id.ToString(),
                BudgetId = @event.Id.ToString(),
                Name = @event.Name,
                EntryTime = @event.EntryTime,
            };
        public static ReadModels.Expense ToModel(this Events.ExpenseAddedToBudget @event)
            => new ReadModels.Expense
            {
                id = @event.ExpenseId.ToString(),
                ExpenseId = @event.ExpenseId.ToString(),
                ParentId = @event.Id.ToString(),
                Amount = @event.Amount,
                EntryTime = @event.EntryTime
            };
        public static ReadModels.Income ToModel(this Events.IncomeAddedToBudget @event)
            => new ReadModels.Income
            {
                id= @event.IncomeId.ToString(),
                IncomeId = @event.IncomeId.ToString(),
                ParentId = @event.Id.ToString(),
                Amount = @event.Amount,
                EntryTime = @event.EntryTime,
                Type = new ReadModels.IncomeType {IncomeTypeId = @event.TypeId, Name = @event.Type} 
            };
        public static ReadModels.Outgo ToModel(this Events.OutgoAddedToBudget @event)
            => new ReadModels.Outgo
            {
                id = @event.OutgoId.ToString(),
                OutgoId = @event.OutgoId.ToString(),
                ParentId = @event.Id.ToString(),
                Amount = @event.Amount,
                EntryTime = @event.EntryTime,
                Type = new ReadModels.OutgoType {OutgoTypeId = @event.TypeId, Name = @event.Type }
            };
        public static ReadModels.Period ToModel(this Events.PeriodCreated @event)
            => new ReadModels.Period
            {
                id = @event.PeriodId.ToString(),
                PeriodId = @event.PeriodId.ToString(),
                ParentId = @event.Id.ToString(),
                Start = @event.Start,
                Stop = @event.Stop,
                State = @event.State,
                Type = @event.Type
            };
    }
}
