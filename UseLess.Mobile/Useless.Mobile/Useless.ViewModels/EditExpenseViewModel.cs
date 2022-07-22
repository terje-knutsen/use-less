using Akavache;
using System.Threading.Tasks;
using Useless.Api;
using Useless.Framework;
using Useless.ViewModels.Bases;
using UseLess.Messages;

namespace Useless.ViewModels
{
    public sealed class EditExpenseViewModel : EditViewModel<ReadModels.Expense>
    {
        private readonly IApplyBudgetCommand applier;

        public EditExpenseViewModel(
            INavigationService navService, 
            IProjection<ReadModels.Expense> queryService, 
            IBlobCache cache,
            IApplyBudgetCommand applier) : base(navService, queryService, cache)
        {
            this.applier = applier;
        }

        public override string Title => "EditExpense".Translate();

        public override decimal OriginalAmount => OriginalItem.Amount;

        protected override string OriginalType => OriginalItem.ToString();

        internal override async Task DoDelete()
        => await applier.Apply(Id, new BudgetCommands.V1.DeleteExpense
        {
            ExpenseId = OriginalItem.ExpenseId
        });

        internal override async Task DoSave()
        {
            await applier.Apply(Id, new BudgetCommands.V1.ChangeExpenseAmount
            {
                Amount = Amount,
                ExpenseId = OriginalItem.ExpenseId
            });
        }

        internal override void InitializeWith(ReadModels.Expense parameter)
        {
            Amount = parameter.Amount;
        }
    }
}
