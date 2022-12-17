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
        private readonly IList<ReadModels.Expense> expenses = new List<ReadModels.Expense>();
        private async Task AddExpense(Events.ExpenseAddedToBudget e) 
        {
            await UpdateBudget(e.Id, b => b.Expense += e.Amount);
            CreateIncome(e);
        }
        private async Task ChangeExpenseAmount(Events.ExpenseAmountChanged e)
        {
            await UpdateBudget(e.Id, b => b.Expense = (b.Expense - e.OldAmount + e.Amount));
            await UpdateExpense(e.ExpenseId, x => x.Amount = e.Amount);
        }
        private async Task DeleteExpense(Events.ExpenseDeleted e) 
        {
            await UpdateBudget(e.Id, b => b.Expense -= e.Amount);
            DeleteExpense(e.ExpenseId);
        }
        private void CreateIncome(Events.ExpenseAddedToBudget e)
            => expenses.Add(new ReadModels.Expense 
            {
                ExpenseId = e.ExpenseId.ToString(),
                ParentId = e.Id.ToString(),
                Amount = e.Amount,
                EntryTime = e.EntryTime
            });
        private async Task UpdateExpense(Guid id, Action<ReadModels.Expense> action)
        {
            var expense = expenses.First(x => x.ExpenseId == id.ToString());
            await Task.Factory.StartNew(()=> action(expense));
        }
        private void DeleteExpense(Guid expenseId)
        {
            var expense = expenses.First(x => x.ExpenseId == expenseId.ToString());
            expenses.Remove(expense);
        }
    }
}
