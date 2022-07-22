using System;
using System.Threading.Tasks;
using Useless.Api;
using Useless.Framework;
using Useless.ViewModels.Bases;
using UseLess.Messages;

namespace Useless.ViewModels
{
    public sealed class AddOutgoViewModel : InOutRegistrationViewModel<ReadModels.Outgo,Guid>
    {
        private readonly IApplyBudgetCommand applier;

        public AddOutgoViewModel(
            INavigationService navService,
            IApplyBudgetCommand applier) : base(navService)
        {
            this.applier = applier;
        }

        public override string Title => "AddOutgo".Translate();

        protected override async Task Save()
        {
            await applier.Apply(Id, new BudgetCommands.V1.AddOutgo 
            {
                Amount = Amount,
                OutgoId = Guid.NewGuid(),
                Type = "WEEKLY"
            });
        }
    }
}
