using Akavache;
using System;
using System.Threading.Tasks;
using Useless.Api;
using Useless.Framework;
using Useless.ViewModels.Bases;
using UseLess.Messages;

namespace Useless.ViewModels
{
    public sealed class ExpensesViewModel : GenericCollectionViewModel<ReadModels.Expense, Guid>
    {
        private readonly ICollectionProjection<ReadModels.Expense, QueryModels.GetExpenses> queryService;
        private readonly IBlobCache cache;

        public Guid ParentId { get; private set; }

        public ExpensesViewModel(INavigationService navService,
            ICollectionProjection<ReadModels.Expense,QueryModels.GetExpenses> queryService,
            IBlobCache cache) : base(navService)
        {
            this.queryService = queryService;
            this.cache = cache;
        }
        internal override void SetItem(Guid item)
        => cache.InsertObject(nameof(ReadModels.Budget.BudgetId), item);
        protected override async Task OnItemSelected(ReadModels.Expense item)
        => await NavService.NavigateTo<EditExpenseViewModel, ReadModels.Expense>(item);

        protected override async Task OnNewItemCommand()
        => await NavService.NavigateTo<AddExpenseViewModel, Guid>(ParentId);

        protected override void SetCollection()
        {
            cache.GetObject<Guid>(nameof(ReadModels.Budget.BudgetId)).Subscribe(x => ParentId = x);
            cache.GetAndFetchLatest($"{ParentId}-expenses", async () => await queryService.GetAsync(new QueryModels.GetExpenses { BudgetId = ParentId }))
            .Subscribe(x => Items = new System.Collections.ObjectModel.ObservableCollection<ReadModels.Expense>(x));
        } }
}
