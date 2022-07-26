using Akavache;
using System;
using System.Threading.Tasks;
using Useless.Api;
using Useless.Framework;
using Useless.ViewModels.Bases;
using UseLess.Messages;

namespace Useless.ViewModels
{
    public sealed class IncomesViewModel : GenericCollectionViewModel<ReadModels.Income,Guid>
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
        protected override async Task OnItemSelected(ReadModels.Income item)
         => await NavService.NavigateTo<EditIncomeViewModel,ReadModels.Income>(item);
        internal override void SetItem(Guid item)
        => cache.InsertObject(nameof(ReadModels.Budget.BudgetId), item);
        protected override async Task OnNewItemCommand()
        => await NavService.NavigateTo<AddIncomeViewModel,Guid>(ParentId);

        protected override void SetCollection()
        {
            cache.GetObject<Guid>(nameof(ReadModels.Budget.BudgetId)).Subscribe(x => ParentId = x);
            cache.GetAndFetchLatest($"{ParentId}-incomes", async () =>
            {
                return await queryService.GetAsync(new QueryModels.GetIncomes { BudgetId = ParentId });
            }).Subscribe(observer => Items = new System.Collections.ObjectModel.ObservableCollection<ReadModels.Income>(observer));
        }
    }
}
