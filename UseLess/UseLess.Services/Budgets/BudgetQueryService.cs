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

        public ReadModels.Budget GetBudget(QueryModels.GetBudget query)
        => budget.Get(query.BudgetId);

        public ReadModels.Expense GetExpense(QueryModels.GetExpense query)
        => expense.Get(query.ExpenseId);

        public IEnumerable<ReadModels.Expense> GetExpenses(QueryModels.GetExpenses query)
        => expenses.GetAll(query.BudgetId);

        public ReadModels.Income GetIncome(QueryModels.GetIncome query)
        => income.Get(query.IncomeId);

        public IEnumerable<ReadModels.Income> GetIncomes(QueryModels.GetIncomes query)
        => incomes.GetAll(query.BudgetId);

        public ReadModels.Outgo GetOutgo(QueryModels.GetOutgo query)
        => outgo.Get(query.OutgoId);

        public IEnumerable<ReadModels.Outgo> GetOutgos(QueryModels.GetOutgos query)
        => outgos.GetAll(query.BudgetId);

        public ReadModels.Period GetPeriod(QueryModels.GetPeriod query)
        => period.Get(query.PeriodId);
    }
}
