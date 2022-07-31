using Akavache;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Useless.Api;
using Useless.Framework;
using Useless.ViewModels.Bases;
using UseLess.Messages;

namespace Useless.ViewModels
{
    public sealed class EditOutgoViewModel : EditViewModel<ReadModels.Outgo>
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

        private ReadModels.OutgoType OriginalType => OriginalItem.Type;
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
        private ReadModels.OutgoType type;

        public ReadModels.OutgoType OutgoType
        {
            get { return type; }
            set { type = value; }
        }

        private ObservableCollection<ReadModels.OutgoType> collection;
        public ObservableCollection<ReadModels.OutgoType> Collection
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
                    Type = OutgoType.Type,
                    OutgoId = OriginalItem.OutgoId
                });
        }

        internal override void InitializeWith(ReadModels.Outgo parameter)
        {
            Amount = parameter.Amount;
            OutgoType = parameter.Type;
            IObservable<IEnumerable<ReadModels.OutgoType>> observable = cache.GetOrFetchObject("outgo-types", async () => { return await typeQuery.GetAsync(new QueryModels.GetOutgoTypes()); });
                observable.Subscribe(x => Collection = new ObservableCollection<ReadModels.OutgoType>(x));
        }
    }
}
