using Useless.Mobile.Models;
using UseLess.Messages;

namespace Useless.Mobile.Extensions
{
    public static class ModelExtensions
    {
        public static IEnumerable<IncomeType> ToModel(this IEnumerable<ReadModels.IncomeType> readModels)
            => readModels.Select(x => new IncomeType { IncomeTypeId = x.IncomeTypeId, Name = x.Name });
        public static IEnumerable<OutgoType> ToModel(this IEnumerable<ReadModels.OutgoType> readModels)
            => readModels.Select(x => new OutgoType { OutgoTypeId = x.OutgoTypeId, Name = x.Name });
        public static IEnumerable<Expense> ToModel(this IEnumerable<ReadModels.Expense> readModels)
         => readModels.Select(x => new Expense
         {
             Amount = x.Amount,
             EntryTime = x.EntryTime,
             ExpenseId = x.ExpenseId,
             ParentId = x.ParentId,
         });
        public static IEnumerable<Income> ToModel(this IEnumerable<ReadModels.Income> readModels)
            => readModels.Select(x => new Income
            {
                Amount = x.Amount,
                EntryTime = x.EntryTime,
                IncomeId = x.IncomeId,
                ParentId = x.ParentId,
                Type = x.Type.ToModel(),
            });
        public static IEnumerable<Outgo> ToModel(this IEnumerable<ReadModels.Outgo> readModels)
            => readModels.Select(x => new Outgo
            {
                Amount = x.Amount,
                EntryTime = x.EntryTime,
                OutgoId = x.OutgoId,
                ParentId = x.ParentId,
                Type = x.Type.ToModel()
            });
        public static IEnumerable<Budget> ToModel(this IEnumerable<ReadModels.Budget> readModels)
            => readModels.Select(x => new Budget
            {
                Available = x.Available,
                BudgetId = x.BudgetId,
                End = x.End,
                EntryTime = x.EntryTime,
                Expense = x.Expense,
                Income = x.Income,
                Left = x.Left,
                Limit = x.Limit,
                Name = x.Name,
                Outgo = x.Outgo,
                Start = x.Start
            });

        public static IncomeType ToModel(this ReadModels.IncomeType readModel)
            => new()
            { IncomeTypeId = readModel.IncomeTypeId, Name = readModel.Name };
        public static OutgoType ToModel(this ReadModels.OutgoType readModel)
            => new()
            {
                OutgoTypeId = readModel.OutgoTypeId,
                Name = readModel.Name
            };
    }
}
