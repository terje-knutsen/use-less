using Useless.Api;
using Useless.Mobile.Api;
using Useless.Mobile.Extensions;
using Useless.Mobile.Models;
using Useless.Mobile.ViewModels.Base;
using UseLess.Messages;

namespace Useless.Mobile.ViewModels
{
    public sealed class AddOutgoViewModel : InOutRegistrationViewModel<Outgo,Guid>
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
            await base.Save();
        }
    }
}
