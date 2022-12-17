using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using UseLess.Domain.Enumerations;
using UseLess.Messages;
using UseLess.Services.Api;

namespace Useless.AzureStore
{
    public partial class ReadStore : IQueryUpdate
    {
        public async Task Update(IEnumerable<object> events) 
        {
            foreach(var e in events)
            {
                await ApplyEvent(e);
            }
        }
        private Task ApplyEvent(object events)
        => events switch
        {
            Events.BudgetCreated e =>
                CreateBudget(e),
            Events.IncomeAddedToBudget e =>
                  AddIncome(e, 
                      UpdateBudget(e.Id, x => x.Income += e.Amount)),
            Events.OutgoAddedToBudget e =>
                 AddOutgo(e, 
                     UpdateBudget(e.Id, x => x.Outgo += e.Amount)),
            Events.ExpenseAddedToBudget e =>
                  AddExpense(e, 
                      UpdateBudget(e.Id,b => b.Expense += e.Amount)),
            Events.IncomeAmountChanged e =>
                UpdateIncome(e.Id, e.IncomeId, x => x.Amount = e.Amount,
                    UpdateBudget(e.Id, x => x.Income = (x.Income - e.OldAmount + e.Amount))),
            Events.OutgoAmountChanged e =>
                UpdateOutgoAsync(e.Id,e.OutgoId, x => x.Amount = e.Amount,
                    UpdateBudget(e.Id, x => x.Outgo = (x.Outgo - e.OldAmount + e.Amount))),
            Events.ExpenseAmountChanged e =>
                UpdateExpense(e.Id,e.ExpenseId, x => x.Amount = e.Amount,
                    UpdateBudget(e.Id, b => b.Expense = (b.Expense - e.OldAmount + e.Amount))),
            Events.BudgetDeleted e =>
                DeleteBudget(e),
            Events.IncomeDeleted e =>
                DeleteIncome(e,
                    UpdateBudget(e.Id,x => x.Income -= e.Amount)),
            Events.OutgoDeleted e =>
                DeleteOutgo(e,
                    UpdateBudget(e.Id, x => x.Outgo -= e.Amount)),
            Events.ExpenseDeleted e =>
                DeleteExpense(e,
                    UpdateBudget(e.Id, x => x.Expense -= e.Amount)),
            Events.PeriodCreated e =>
                AddPeriod(e, 
                    UpdateBudget(e.Id, b => { b.Start = e.Start; b.End = e.Stop; b.EntryTime = e.EntryTime; })),
            Events.PeriodStopChanged e =>
                UpdatePeriod(e.Id, e.PeriodId, x => x.Stop = e.StopTime, 
                    UpdateBudget(e.Id, b => b.End = e.StopTime)),
            Events.IncomeTypeChanged e =>
                UpdateIncome(e.Id, e.IncomeId, x => x.Type = new ReadModels.IncomeType {IncomeTypeId = e.IncomeTypeId, Name = e.IncomeType }),
            Events.OutgoTypeChanged e =>
                UpdateOutgoAsync(e.Id, e.OutgoId, x => x.Type = new ReadModels.OutgoType {OutgoTypeId = e.TypeId, Name = e.OutgoType}),
            Events.PeriodTypeChanged e =>
                UpdatePeriod(e.Id,e.PeriodId, x => x.Type = e.PeriodType),
            Events.PeriodStateChanged e =>
                UpdatePeriod(e.Id,e.PeriodId, x => x.State = e.State),
            Events.AmountAvailableChanged e =>
                UpdateBudget(e.Id, b => b.Available = e.AmountAvailable),
            Events.AmountLeftChanged e =>
                UpdateBudget(e.Id, b => b.Left = e.AmountLeft),
            Events.AmountLimitChanged e =>
                UpdateBudget(e.Id, b => b.Limit = e.AmountLimit),
            _ => Task.CompletedTask
        };

    

        public async Task Initialize(Database database)
        {
           budgetContainer = await database.CreateContainerIfNotExistsAsync(new ContainerProperties(nameof(ReadModels.Budget), $"/BudgetId"));
           income = await database.CreateContainerIfNotExistsAsync(new ContainerProperties(nameof(ReadModels.Income), $"/IncomeId"));
           _outgo =  await database.CreateContainerIfNotExistsAsync(new ContainerProperties(nameof(ReadModels.Outgo), $"/OutgoId"));
            expense = await database.CreateContainerIfNotExistsAsync(new ContainerProperties(nameof(ReadModels.Expense), $"/ExpenseId"));
            _period = await database.CreateContainerIfNotExistsAsync(new ContainerProperties(nameof(ReadModels.Period), $"/PeriodId"));
        }
        private static async Task Update<T>(Guid id, Action<T> operation, Container container,PartitionKey partitionKey)
        {
            var x = await container.ReadItemAsync<T>(id.ToString(), partitionKey);
            if (x != null)
            {
                operation(x);
                await container.UpsertItemAsync<T>(x);
            }
        }
    }
}
