using System;
using System.Threading.Tasks;
using Useless.Api;
using Useless.Framework;
using Useless.ViewModels.Bases;
using UseLess.Messages;

namespace Useless.ViewModels
{
    public sealed class AddExpenseViewModel : InOutRegistrationViewModel<ReadModels.Expense, Guid>
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
        => await applier.Apply(Id, new BudgetCommands.V1.AddExpense
        {
            Amount = Amount,
            ExpenseId = Guid.NewGuid()
        });
    }
}
