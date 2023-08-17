using Akavache;
using Useless.Api;
using Useless.Mobile.Api;
using Useless.Mobile.Extensions;
using Useless.Mobile.Models;
using Useless.Mobile.ViewModels.Base;
using UseLess.Messages;

namespace Useless.Mobile.ViewModels
{
    public sealed class EditExpenseViewModel : EditViewModel<Expense>
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

        protected override bool HasChanges
        {
            get
            {
                if (OriginalItem == null) return false;
                return OriginalItem.Amount != Amount;
            }
        }

        internal override async Task DoDelete()
        => await applier.Apply(Id, new BudgetCommands.V1.DeleteExpense
        {
            ExpenseId = new Guid(OriginalItem.ExpenseId)
        });

        internal override async Task DoSave()
        {
            await applier.Apply(Id, new BudgetCommands.V1.ChangeExpenseAmount
            {
                Amount = Amount,
                ExpenseId = new Guid(OriginalItem.ExpenseId)
            });
        }

        internal override void InitializeWith(Expense parameter)
        {
            Amount = parameter.Amount;
        }
    }
}
