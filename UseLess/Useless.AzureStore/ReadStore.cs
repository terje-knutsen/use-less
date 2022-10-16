using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using UseLess.Messages;
using UseLess.Services.Api;

namespace Useless.AzureStore
{
    public partial class ReadStore : IQueryUpdate
    {
        

        public Task Update(IEnumerable<object> events)
        => events switch
        {
            Events.BudgetCreated e =>
                CreateBudget(e),
            Events.IncomeAddedToBudget e =>
                  AddIncome(e),
            Events.OutgoAddedToBudget e =>
                 AddOutgo(e),
            Events.ExpenseAddedToBudget e =>
                  AddExpense(e, 
                      UpdateBudget(e.Id,b => b.Expense += e.Amount)),
            Events.IncomeAmountChanged e =>
                ChangeIncomeAmount(e),
            Events.OutgoAmountChanged e =>
                ChangeOutgoAmount(e),
            Events.ExpenseAmountChanged e =>
                ChangeExpenseAmount(e.ExpenseId, x => x.Amount = e.Amount,
                    UpdateBudget(e.Id, b => b.Expense = (b.Expense - e.OldAmount + e.Amount))),
            Events.BudgetDeleted e =>
                DeleteBudget(e),
            Events.IncomeDeleted e =>
                DeleteIncome(e),
            Events.OutgoDeleted e =>
                DeleteOutgo(e),
            Events.ExpenseDeleted e =>
                DeleteExpense(e),
            Events.PeriodCreated e =>
                AddPeriod(e, 
                    UpdateBudget(e.Id, b => { b.Start = e.Start; b.End = e.Stop; b.EntryTime = e.EntryTime; })),
            Events.PeriodStopChanged e =>
                UpdatePeriod(e.PeriodId, x => x.Stop = e.StopTime, 
                    UpdateBudget(e.Id, b => b.End = e.StopTime)),
            Events.IncomeTypeChanged e =>
                ChangeIncomeType(e),
            Events.OutgoTypeChanged e =>
                ChangeOutgoType(e),
            Events.PeriodTypeChanged e =>
                ChangePeriodType(e),
            Events.PeriodStateChanged e =>
                ChangePeriodState(e),
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
           budgetContainer = await database.CreateContainerIfNotExistsAsync(new ContainerProperties(nameof(ReadModels.Budget), $"/State"));
            await database.CreateContainerIfNotExistsAsync(new ContainerProperties(nameof(ReadModels.Income), $"/Type"));
            await database.CreateContainerIfNotExistsAsync(new ContainerProperties(nameof(ReadModels.Outgo), $"/Type"));
            expense = await database.CreateContainerIfNotExistsAsync(new ContainerProperties(nameof(ReadModels.Expense), $"/EntryTime"));
        }
    }
}
