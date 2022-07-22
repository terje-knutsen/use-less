using Akavache;
using System.Threading.Tasks;
using Useless.Api;
using Useless.Framework;
using Useless.ViewModels.Bases;
using UseLess.Messages;

namespace Useless.ViewModels
{
    public sealed class EditIncomeViewModel : EditViewModel<ReadModels.Income>
    {
        private readonly IApplyBudgetCommand budgetCommandService;

        public EditIncomeViewModel(
            INavigationService navService,
            IProjection<ReadModels.Income> queryService,
            IBlobCache cache,
            IApplyBudgetCommand budgetCommandService) : base(navService, queryService, cache)
        {
            this.budgetCommandService = budgetCommandService;
        }

        public override string Title => "EditIncome".Translate();

        public override decimal OriginalAmount => OriginalItem.Amount;

        protected override string OriginalType => OriginalItem.Type;

        internal override async Task DoDelete()
        {
            //await budgetCommandService.RemoveIncomeAsync(new Messages.DailyBudgets.Commands.V1.RemoveIncome
            //{
            //    DailyBudgetId = OriginalItem.DailyBudgetId,
            //    IncomeId = OriginalItem.IncomeId
            //});
        }

        internal override async Task DoSave()
        {
            //await budgetCommandService.ChangeIncomeAsync(new Messages.DailyBudgets.Commands.V1.ChangeIncome
            //{
            //    IncomeId = OriginalItem.IncomeId,
            //    Amount = Amount,
            //    Description = Description,
            //    DailyBudgetId = OriginalItem.DailyBudgetId
            //});
        }

        internal override void InitializeWith(ReadModels.Income item)
        {
            Type = item.Type;
            Amount = item.Amount;
        }
    }
}
