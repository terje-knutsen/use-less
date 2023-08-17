using Akavache;
using System.Collections.ObjectModel;
using Useless.Api;
using Useless.Mobile.Api;
using Useless.Mobile.Extensions;
using Useless.Mobile.Models;
using Useless.Mobile.ViewModels.Base;
using UseLess.Messages;

namespace Useless.Mobile.ViewModels
{
    public sealed class EditOutgoViewModel : EditViewModel<Outgo>
    {
        private readonly ICollectionProjection<ReadModels.OutgoType, QueryModels.GetOutgoTypes> typeQuery;
        private readonly IBlobCache cache;
        private readonly IApplyBudgetCommand applier;

        public EditOutgoViewModel(
            INavigationService navService, 
            ICollectionProjection<ReadModels.OutgoType, QueryModels.GetOutgoTypes> typeQuery,
            IBlobCache cache,
            IApplyBudgetCommand applier) : base(navService)
        {
            this.typeQuery = typeQuery;
            this.cache = cache;
            this.applier = applier;
        }

        public override string Title => "EditOutgo".Translate();

        private decimal OriginalAmount => OriginalItem.Amount;

        private OutgoType OriginalType => OriginalItem.Type;
        private bool AmountChanged
        {
            get
            {
                if (OriginalItem == null) return false;
                return OriginalAmount != Amount;
            }
        }
        private bool TypeChanged
        {
            get
            {
                if(OriginalItem == null || OutgoType == null) return false;
                return OriginalType != OutgoType;
            }
        }
        protected override bool HasChanges =>  TypeChanged || AmountChanged; 

        private decimal amount;

        public decimal Amount
        {
            get { return amount; }
            set 
            { 
                amount = value;
                OnPropertyChanged();
            }
        }
        private OutgoType type;

        public OutgoType OutgoType
        {
            get { return type; }
            set { type = value; }
        }

        private ObservableCollection<OutgoType> collection;
        public ObservableCollection<OutgoType> Collection
        {
            get => collection;
            set
            {
                collection = value;
                OnPropertyChanged();
            }
        }


        internal override async Task DoDelete()
        => await applier.Apply(Id, new BudgetCommands.V1.DeleteOutgo 
        {
            OutgoId = new Guid( OriginalItem.OutgoId)
        });

        internal override async Task DoSave()
        {
            if (AmountChanged)
                await applier.Apply(Id, new BudgetCommands.V1.ChangeOutgoAmount
                {
                    Amount = Amount,
                    OutgoId = new Guid(OriginalItem.OutgoId)
                });
            if (TypeChanged)
                await applier.Apply(Id, new BudgetCommands.V1.ChangeOutgoType
                {
                    Type = OutgoType.Name,
                    OutgoId = new Guid(OriginalItem.OutgoId)
                });
        }

        internal override void InitializeWith(Outgo parameter)
        {
            Amount = parameter.Amount;
            OutgoType = parameter.Type;
            IObservable<IEnumerable<OutgoType>> observable = cache.GetOrFetchObject("outgo-types", 
                async () => 
                { 
                    var result = await typeQuery.GetAsync(new QueryModels.GetOutgoTypes());
                    return result.ToModel();
                });
                observable.Subscribe(x => Collection = new ObservableCollection<OutgoType>(x));
        }
    }
}
