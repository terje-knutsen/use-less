using Akavache;
using Useless.Api;
using Useless.Mobile.Api;
using Useless.Mobile.Extensions;
using Useless.Mobile.Models;
using Useless.Mobile.ViewModels.Base;
using UseLess.Messages;

namespace Useless.Mobile.ViewModels
{
    public sealed class IncomesViewModel : GenericCollectionViewModel<Income,Guid>
    {
        private readonly ICollectionProjection<ReadModels.Income,QueryModels.GetIncomes> queryService;
        private readonly IBlobCache cache;

        public Guid ParentId { get; private set; }

        public IncomesViewModel(
        INavigationService navService,
        ICollectionProjection<ReadModels.Income,QueryModels.GetIncomes> queryService,
        IBlobCache cache) : base(navService)
        {
            this.queryService = queryService;
            this.cache = cache;
        }
        protected override async Task OnItemSelected(Income item)
         => await NavService.NavigateTo<EditIncomeViewModel,Income>(item);
        internal override void SetItem(Guid item)
        => cache.InsertObject(nameof(Budget.BudgetId), item);
        protected override async Task OnNewItemCommand()
        => await NavService.NavigateTo<AddIncomeViewModel,Guid>(ParentId);

        protected override void SetCollection()
        {
            cache.GetObject<Guid>(nameof(Budget.BudgetId)).Subscribe(x => ParentId = x);
            cache.GetAndFetchLatest($"{ParentId}-incomes", async () =>
            {
                var result = await queryService.GetAsync(new QueryModels.GetIncomes { BudgetId = ParentId });
                return result.ToModel();
            }).Subscribe(observer => Items = new System.Collections.ObjectModel.ObservableCollection<Income>(observer));
        }
    }
}
