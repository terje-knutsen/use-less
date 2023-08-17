using Useless.Api;
using Useless.Mobile.Api;
using Useless.Mobile.Extensions;
using Useless.Mobile.Models;
using Useless.Mobile.ViewModels.Base;
using UseLess.Messages;

namespace Useless.Mobile.ViewModels
{
    public sealed class AddExpenseViewModel : InOutRegistrationViewModel<Expense, Guid>
    {
        private readonly IApplyBudgetCommand applier;

        public AddExpenseViewModel(
            INavigationService navService,
            IApplyBudgetCommand applier) : base(navService)
        {
            this.applier = applier;
        }

        public override string Title => "AddExpense".Translate();

        protected override async Task Save()
        {
            await applier.Apply(Id, new BudgetCommands.V1.AddExpense
            {
                Amount = Amount,
                ExpenseId = Guid.NewGuid()
            });
            await base.Save();
        }
        
    }
}
