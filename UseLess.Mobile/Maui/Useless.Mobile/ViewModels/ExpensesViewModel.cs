using Akavache;
using Useless.Api;
using Useless.Mobile.Api;
using Useless.Mobile.Extensions;
using Useless.Mobile.Models;
using Useless.Mobile.ViewModels.Base;
using UseLess.Messages;

namespace Useless.Mobile.ViewModels
{
    public sealed class ExpensesViewModel : GenericCollectionViewModel<Expense, Guid>
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
        => cache.InsertObject(nameof(Budget.BudgetId), item);
        protected override async Task OnItemSelected(Expense item)
        => await NavService.NavigateTo<EditExpenseViewModel, Expense>(item);

        protected override async Task OnNewItemCommand()
        => await NavService.NavigateTo<AddExpenseViewModel, Guid>(ParentId);

        protected override void SetCollection()
        {
            cache.GetObject<Guid>(nameof(Budget.BudgetId)).Subscribe(x => ParentId = x);
            cache.GetAndFetchLatest($"{ParentId}-expenses",
                async () => 
                { 
                    var result = await queryService.GetAsync(new QueryModels.GetExpenses { BudgetId = ParentId });
                    return result.ToModel();
                })
            .Subscribe(x => 
            Items = new System.Collections.ObjectModel.ObservableCollection<Expense>(x));
        } }
}
