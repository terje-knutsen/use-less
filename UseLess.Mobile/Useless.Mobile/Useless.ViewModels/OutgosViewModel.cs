using Akavache;
using System;
using System.Threading.Tasks;
using Useless.Api;
using Useless.Framework;
using Useless.ViewModels.Bases;
using UseLess.Messages;

namespace Useless.ViewModels
{
    public sealed class OutgosViewModel : GenericCollectionViewModel<ReadModels.Outgo,Guid>
    {
        private readonly IProjection<ReadModels.Outgo> queryService;
        private readonly IBlobCache cache;

        public Guid ParentId { get; private set; }

        public OutgosViewModel(
            INavigationService navService,
            IProjection<ReadModels.Outgo> queryService,
            IBlobCache cache) : base(navService)
        {
            this.queryService = queryService;
            this.cache = cache;
        }
        internal override void SetItem(Guid item)
        => cache.InsertObject(nameof(ReadModels.Budget.BudgetId), item);
        protected override async Task OnItemSelected(ReadModels.Outgo item)
        => await NavService.NavigateTo<EditOutgoViewModel, ReadModels.Outgo>(item);

        protected override async Task OnNewItemCommand()
        => await NavService.NavigateTo<AddOutgoViewModel, Guid>(ParentId);

        protected override void SetCollection()
        {
            cache.GetObject<Guid>(nameof(ReadModels.Budget.BudgetId)).Subscribe(x => ParentId = x);
            cache.GetAndFetchLatest($"{ParentId}-outgos", async () => await queryService.GetAllAsync(ParentId))
                 .Subscribe(x => Items = new System.Collections.ObjectModel.ObservableCollection<ReadModels.Outgo>(x));
        }
    }
}
