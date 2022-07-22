using System;
using System.Threading.Tasks;
using Useless.Api;
using Useless.Framework;
using Useless.ViewModels.Bases;
using UseLess.Messages;

namespace Useless.ViewModels
{
    public sealed class AddIncomeViewModel : InOutRegistrationViewModel<ReadModels.Income,Guid>
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
        }
    }
}
