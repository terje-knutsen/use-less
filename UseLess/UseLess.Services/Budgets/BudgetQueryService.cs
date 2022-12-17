using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Messages;
using UseLess.Services.Api;

namespace UseLess.Services.Budgets
{
    public sealed class BudgetQueryService : IBudgetQueryService
    {
        private readonly IQueryStore<ReadModels.Budget> budget;
        private readonly IQueryStore<ReadModels.Income> income;
        private readonly IQueryStore<ReadModels.Outgo> outgo;
        private readonly IQueryStore<ReadModels.Expense> expense;
        private readonly IQueryStore<ReadModels.Period> period;
        private readonly ICollectionQueryStore<ReadModels.Income> incomes;
        private readonly ICollectionQueryStore<ReadModels.Outgo> outgos;
        private readonly ICollectionQueryStore<ReadModels.Expense> expenses;

        public BudgetQueryService(
            IQueryStore<ReadModels.Budget> budget,
            IQueryStore<ReadModels.Income> income,
            IQueryStore<ReadModels.Outgo> outgo,
            IQueryStore<ReadModels.Expense> expense,
            IQueryStore<ReadModels.Period> period,
            ICollectionQueryStore<ReadModels.Income> incomes,
            ICollectionQueryStore<ReadModels.Outgo> outgos,
            ICollectionQueryStore<ReadModels.Expense> expenses
            )
        {
            this.budget = budget;
            this.income = income;
            this.outgo = outgo;
            this.expense = expense;
            this.period = period;
            this.incomes = incomes;
            this.outgos = outgos;
            this.expenses = expenses;
        }

        public async Task<ReadModels.Budget> GetBudget(QueryModels.GetBudget query)
        => await budget.Get(query.BudgetId);

        public async Task<ReadModels.Expense> GetExpense(QueryModels.GetExpense query)
        => await expense.Get(query.ExpenseId);

        public async Task<IEnumerable<ReadModels.Expense>> GetExpenses(QueryModels.GetExpenses query)
        => await expenses.GetAll(query.BudgetId);

        public async Task<ReadModels.Income> GetIncome(QueryModels.GetIncome query) => await income.Get(query.IncomeId);

        public async Task<IEnumerable<ReadModels.Income>> GetIncomes(QueryModels.GetIncomes query) => await incomes.GetAll(query.BudgetId);

        public async Task<ReadModels.Outgo> GetOutgo(QueryModels.GetOutgo query) => await outgo.Get(query.OutgoId);

        public async Task<IEnumerable<ReadModels.Outgo>> GetOutgos(QueryModels.GetOutgos query) => await outgos.GetAll(query.BudgetId);

        public async Task<ReadModels.Period> GetPeriod(QueryModels.GetPeriod query) => await period.Get(query.PeriodId);
    }
}
