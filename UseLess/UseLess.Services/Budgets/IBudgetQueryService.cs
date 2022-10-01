using UseLess.Messages;

namespace UseLess.Services.Budgets
{
    public interface IBudgetQueryService 
    {
        Task<ReadModels.Budget> GetBudget(QueryModels.GetBudget query);
        Task<ReadModels.Income> GetIncome(QueryModels.GetIncome query);
        Task<ReadModels.Outgo> GetOutgo(QueryModels.GetOutgo query);
        Task<ReadModels.Expense> GetExpense(QueryModels.GetExpense query);
        Task<ReadModels.Period> GetPeriod(QueryModels.GetPeriod query);
        Task<IEnumerable<ReadModels.Income>> GetIncomes(QueryModels.GetIncomes query);
        Task<IEnumerable<ReadModels.Outgo>> GetOutgos(QueryModels.GetOutgos query);
        Task<IEnumerable<ReadModels.Expense>> GetExpenses(QueryModels.GetExpenses query);
    }
}
