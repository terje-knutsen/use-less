using Akavache;
using Useless.Api;
using Useless.Mobile.Api;
using Useless.Mobile.Extensions;
using Useless.Mobile.Models;
using Useless.Mobile.ViewModels.Base;
using UseLess.Messages;

namespace Useless.Mobile.ViewModels
{
    public sealed class OutgosViewModel : GenericCollectionViewModel<Outgo,Guid>
    {
        private readonly ICollectionProjection<ReadModels.Outgo, QueryModels.GetOutgos> queryService;
        private readonly IBlobCache cache;

        public Guid ParentId { get; private set; }

        public OutgosViewModel(
            INavigationService navService,
            ICollectionProjection<ReadModels.Outgo,QueryModels.GetOutgos> queryService,
            IBlobCache cache) : base(navService)
        {
            this.queryService = queryService;
            this.cache = cache;
        }
        internal override void SetItem(Guid item)
        => cache.InsertObject(nameof(ReadModels.Budget.BudgetId), item);
        protected override async Task OnItemSelected(Outgo item)
        => await NavService.NavigateTo<EditOutgoViewModel, Outgo>(item);

        protected override async Task OnNewItemCommand()
        => await NavService.NavigateTo<AddOutgoViewModel, Guid>(ParentId);

        protected override void SetCollection()
        {
            cache.GetObject<Guid>(nameof(ReadModels.Budget.BudgetId)).Subscribe(x => ParentId = x);
            cache.GetAndFetchLatest($"{ParentId}-outgos", 
                async () => 
                {
                    var result = await queryService.GetAsync(new QueryModels.GetOutgos { BudgetId = ParentId });
                    return result.ToModel();
                } )
                 .Subscribe(x => Items = new System.Collections.ObjectModel.ObservableCollection<Outgo>(x));
        }
    }
}
