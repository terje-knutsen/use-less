using Useless.Api;
using Useless.Mobile.Api;
using Useless.Mobile.Extensions;
using Useless.Mobile.Models;
using Useless.Mobile.ViewModels.Base;
using UseLess.Messages;

namespace Useless.Mobile.ViewModels
{
    public sealed class AddIncomeViewModel : InOutRegistrationViewModel<Income,Guid>
    {
        private readonly IApplyBudgetCommand commandApplier;

        public AddIncomeViewModel(
            INavigationService navService,
            IApplyBudgetCommand commandApplier) : base(navService)
        {
            this.commandApplier = commandApplier;
        }

        public override string Title => "AddIncome".Translate();

        protected override async Task Save()
        {
            await commandApplier.Apply(Id,new BudgetCommands.V1.AddIncome
            {
                Amount = Amount,
                IncomeId = Guid.NewGuid(),
                Type = "BONUS" 
            });
            await base.Save();
        }
    }
}
