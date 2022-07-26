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
        private readonly IProjection<ReadModels.Expense, QueryModels.GetExpense> queryService;
        private readonly IBlobCache cache;
        private readonly IApplyBudgetCommand applier;

        public EditExpenseViewModel(
            INavigationService navService, 
            IProjection<ReadModels.Expense, QueryModels.GetExpense> queryService, 
            IBlobCache cache,
            IApplyBudgetCommand applier) : base(navService)
        {
            this.queryService = queryService;
            this.cache = cache;
            this.applier = applier;
        }

        public override string Title => "EditExpense".Translate();

        private decimal amount;
        public decimal Amount 
        {
            get => amount;
            set 
            {
                amount = value;
                OnPropertyChanged();
            } 
        }

        protected override bool HasChanges => OriginalItem.Amount != Amount;

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
