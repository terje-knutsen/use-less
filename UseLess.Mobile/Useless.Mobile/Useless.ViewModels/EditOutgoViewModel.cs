using Akavache;
using System.Threading.Tasks;
using Useless.Api;
using Useless.Framework;
using Useless.ViewModels.Bases;
using UseLess.Messages;

namespace Useless.ViewModels
{
    public sealed class EditOutgoViewModel : EditViewModel<ReadModels.Outgo>
    {
        private readonly IApplyBudgetCommand applier;

        public EditOutgoViewModel(
            INavigationService navService, 
            IProjection<ReadModels.Outgo> queryService, 
            IBlobCache cache,
            IApplyBudgetCommand applier) : base(navService, queryService, cache)
        {
            this.applier = applier;
        }

        public override string Title => "EditOutgo".Translate();

        public override decimal OriginalAmount => OriginalItem.Amount;

        protected override string OriginalType => OriginalItem.Type;

        internal override async Task DoDelete()
        => await applier.Apply(Id, new BudgetCommands.V1.DeleteOutgo 
        {
            OutgoId = OriginalItem.OutgoId
        });

        internal override async Task DoSave()
        {
            if (AmountChanged)
                await applier.Apply(Id, new BudgetCommands.V1.ChangeOutgoAmount
                {
                    Amount = Amount,
                    OutgoId = OriginalItem.OutgoId
                });
            if (TypeChanged)
                await applier.Apply(Id, new BudgetCommands.V1.ChangeOutgoType
                {
                    Type = Type,
                    OutgoId = OriginalItem.OutgoId
                });
        }

        internal override void InitializeWith(ReadModels.Outgo parameter)
        {
            Amount = parameter.Amount;
            Type = parameter.Type;
        }
    }
}
