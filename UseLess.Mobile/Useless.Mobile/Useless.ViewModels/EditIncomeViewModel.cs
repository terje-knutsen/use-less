using Akavache;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Useless.Api;
using Useless.Framework;
using Useless.ViewModels.Bases;
using UseLess.Messages;

namespace Useless.ViewModels
{
    public sealed class EditIncomeViewModel : EditViewModel<ReadModels.Income>
    {
        private readonly IProjection<ReadModels.Income, QueryModels.GetIncome> queryService;
        private readonly ICollectionProjection<ReadModels.IncomeType, QueryModels.GetIncomeTypes> typeQuery;
        private readonly IBlobCache cache;
        private readonly IApplyBudgetCommand budgetCommandService;

        public EditIncomeViewModel(
            INavigationService navService,
            IProjection<ReadModels.Income, QueryModels.GetIncome> queryService,
            ICollectionProjection<ReadModels.IncomeType, QueryModels.GetIncomeTypes> typeQuery,
            IBlobCache cache,
            IApplyBudgetCommand budgetCommandService) : base(navService)
        {
            this.queryService = queryService;
            this.typeQuery = typeQuery;
            this.cache = cache;
            this.budgetCommandService = budgetCommandService;

        }
        private bool TypeChanged
        {
            get
            {
                if (OriginalItem == null) return false;
                return OriginalType != type;
            }
        }
        private bool AmountChanged
        {
            get
            {
                if (OriginalItem == null) return false;
                return OriginalAmount != amount;
            }
        }

        public override string Title => "EditIncome".Translate();

        private decimal OriginalAmount => OriginalItem.Amount;

        private string OriginalType => OriginalItem.Type;

        protected override bool HasChanges => TypeChanged || AmountChanged;

        private string type;
    
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
        private ObservableCollection<ReadModels.IncomeType> collection;
        public ObservableCollection<ReadModels.IncomeType> Collection
        {
            get => collection;
            set
            {
                collection = value;
                OnPropertyChanged();
            }
        }

        internal override async Task DoDelete()
        {
            //await budgetCommandService.RemoveIncomeAsync(new Messages.DailyBudgets.Commands.V1.RemoveIncome
            //{
            //    DailyBudgetId = OriginalItem.DailyBudgetId,
            //    IncomeId = OriginalItem.IncomeId
            //});
        }

        internal override async Task DoSave()
        {
            //await budgetCommandService.ChangeIncomeAsync(new Messages.DailyBudgets.Commands.V1.ChangeIncome
            //{
            //    IncomeId = OriginalItem.IncomeId,
            //    Amount = Amount,
            //    Description = Description,
            //    DailyBudgetId = OriginalItem.DailyBudgetId
            //});
        }

        internal override void InitializeWith(ReadModels.Income item)
        {
            type = item.Type;
            amount = item.Amount;
            IObservable<IEnumerable<ReadModels.IncomeType>> observable = 
                cache.GetAndFetchLatest("income_types", async () => 
                { 
                    return await typeQuery.GetAsync(new QueryModels.GetIncomeTypes()); 
                });
            observable.Subscribe(x => Collection = TranslateTypes(x, x => new ReadModels.IncomeType { Type = x.Type.Translate() }));
        }
    }
}
