using System.Collections.Generic;
using System.Threading.Tasks;
using Useless.Api;
using UseLess.Messages;

namespace Useless.ApplicationService.InMemory
{
    public sealed class LocalProjection :
        ICollectionProjection<ReadModels.Budget, QueryModels.GetBudgets>,
        IProjection<ReadModels.Budget, QueryModels.GetBudget>,
        ICollectionProjection<ReadModels.Income, QueryModels.GetIncomes>,
        IProjection<ReadModels.Income, QueryModels.GetIncome>,
        ICollectionProjection<ReadModels.Outgo, QueryModels.GetOutgos>,
        IProjection<ReadModels.Outgo, QueryModels.GetOutgo>,
        ICollectionProjection<ReadModels.Expense, QueryModels.GetExpenses>,
        IProjection<ReadModels.Expense, QueryModels.GetExpense>,
        ICollectionProjection<ReadModels.IncomeType, QueryModels.GetIncomeTypes>,
        ICollectionProjection<ReadModels.OutgoType, QueryModels.GetOutgoTypes>
    {
        Task<IEnumerable<ReadModels.Budget>> ICollectionProjection<ReadModels.Budget, QueryModels.GetBudgets>.GetAsync(QueryModels.GetBudgets queryModel)
        => Task.Run(() => Budgets.Instance.All);
        Task<ReadModels.Budget> IProjection<ReadModels.Budget, QueryModels.GetBudget>.GetAsync(UseLess.Messages.QueryModels.GetBudget queryModel)
        => Task.Run(() => Budgets.Instance[queryModel.BudgetId]);
        Task<IEnumerable<ReadModels.Income>> ICollectionProjection<ReadModels.Income, QueryModels.GetIncomes>.GetAsync(QueryModels.GetIncomes queryModel)
        => Task.Run(() => Incomes.Instance.All(queryModel.BudgetId));
        Task<ReadModels.Income> IProjection<ReadModels.Income, QueryModels.GetIncome>.GetAsync(QueryModels.GetIncome queryModel)
        => Task.Run(() => Incomes.Instance[queryModel.IncomeId]);
        Task<IEnumerable<ReadModels.Outgo>> ICollectionProjection<ReadModels.Outgo, QueryModels.GetOutgos>.GetAsync(QueryModels.GetOutgos queryModel)
        => Task.Run(() => Outgos.Instance.All(queryModel.BudgetId));
        Task<ReadModels.Outgo> IProjection<ReadModels.Outgo, QueryModels.GetOutgo>.GetAsync(QueryModels.GetOutgo queryModel)
        => Task.Run(() => Outgos.Instance[queryModel.OutgoId]);

        Task<ReadModels.Expense> IProjection<ReadModels.Expense, QueryModels.GetExpense>.GetAsync(QueryModels.GetExpense queryModel)
        => Task.Run(() => Expenses.Instance[queryModel.ExpenseId]);

        Task<IEnumerable<ReadModels.Expense>> ICollectionProjection<ReadModels.Expense, QueryModels.GetExpenses>.GetAsync(QueryModels.GetExpenses queryModel)
        => Task.Run(() => Expenses.Instance.All(queryModel.BudgetId));
        Task<IEnumerable<ReadModels.OutgoType>> ICollectionProjection<ReadModels.OutgoType, QueryModels.GetOutgoTypes>.GetAsync(QueryModels.GetOutgoTypes queryModel)
        => Task.Run(() => Outgos.Instance.Types);

        Task<IEnumerable<ReadModels.IncomeType>> ICollectionProjection<ReadModels.IncomeType, QueryModels.GetIncomeTypes>.GetAsync(QueryModels.GetIncomeTypes queryModel)
        => Task.Run(() => Incomes.Instance.Types);
    }
}
