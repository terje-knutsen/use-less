using UseLess.Messages;

namespace UseLess.Services.Budgets
{
    public interface IBudgetQueryService 
    {
        ReadModels.Budget GetBudget(QueryModels.GetBudget query);
        ReadModels.Income GetIncome(QueryModels.GetIncome query);
        ReadModels.Outgo GetOutgo(QueryModels.GetOutgo query);
        ReadModels.Expense GetExpense(QueryModels.GetExpense query);
        ReadModels.Period GetPeriod(QueryModels.GetPeriod query);
        IEnumerable<ReadModels.Income> GetIncomes(QueryModels.GetIncomes query);
        IEnumerable<ReadModels.Outgo> GetOutgos(QueryModels.GetOutgos query);
        IEnumerable<ReadModels.Expense> GetExpenses(QueryModels.GetExpenses query);
    }
}
