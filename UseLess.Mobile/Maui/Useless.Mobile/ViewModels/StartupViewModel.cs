using Akavache;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Useless.Api;
using Useless.Mobile.Api;
using Useless.Mobile.Extensions;
using Useless.Mobile.Models;
using Useless.Mobile.ViewModels.Base;
using UseLess.Messages;

namespace Useless.Mobile.ViewModels
{
    public sealed class StartupViewModel : CollectionViewModel<Budget>
    {
        private bool newItemRequested;
        private Command saveCommand;
        private readonly ICollectionProjection<ReadModels.Budget,QueryModels.GetBudgets> budgetProjection;
        private readonly IApplyBudgetCommand commandApplier;
        private readonly IBlobCache cache;

        public StartupViewModel(
            INavigationService navService,
            ICollectionProjection<ReadModels.Budget,QueryModels.GetBudgets> budgetProjection,
            IApplyBudgetCommand commandApplier,
            IBlobCache cache) : base(navService)
        {
            this.budgetProjection = budgetProjection;
            this.commandApplier = commandApplier;
            this.cache = cache;
            Init();
        }
        private Budget currentBudget;

        public Budget CurrentBudget
        {
            get { return currentBudget; }
            set 
            {
                currentBudget = value;
                OnPropertyChanged();
            }
        }
        protected override async Task OnItemSelected(Budget item)
        => await NavService.NavigateTo<EditBudgetViewModel, Budget>(item);
        public bool NewItemRequested
        {
            get => newItemRequested;
            set
            {
                newItemRequested = value;
                OnPropertyChanged();
            }
        }
        private int position;
        private static int currentPosition;

        public int Position
        {
            get { return position; }
            set 
            { 
                position = value;
                OnPropertyChanged();
            }
        }

        public ICommand IncomeCommand => new Command<Budget>(async x
        => await NavService.NavigateTo<IncomesViewModel, Guid>(new(x.BudgetId)));
        public ICommand OutgoCommand => new Command<Budget>(async x
        => await NavService.NavigateTo<OutgosViewModel, Guid>(new(x.BudgetId)));
        public ICommand ExpenseCommand => new Command<Budget>(async x
        => await NavService.NavigateTo<ExpensesViewModel, Guid>(new(x.BudgetId)));
        public ICommand SaveCommand =>
            saveCommand ??= new Command<string>(async (x) => await Save(x));
        private async Task Save(string name)
        {
            var id = Guid.NewGuid();
            Items.Add(new Budget { BudgetId = id.ToString(), Name = name });
            await commandApplier.Apply(id, new BudgetCommands.V1.Create { BudgetId = id, Name = name });
            NewItemRequested = false;
            cache.InsertObject<int>("current_position", Items.Count - 1);
            UpdateCollection();
        }

        protected override async Task OnNewItemCommand()
        => await Task.Run(() => NewItemRequested = true);

        protected override void InitCollection()
        {
            cache.GetAndFetchLatest("budgets", 
                async () => 
                { 
                    var result = await budgetProjection.GetAsync(new QueryModels.GetBudgets());
                    return result.ToModel();
                })
            .Subscribe(x => Items = new ObservableCollection<Budget>(x));
           
      
        }
        public void UpdateCollection()
        {
            cache.GetAndFetchLatest("budgets",
                async () =>  await budgetProjection.GetAsync(new QueryModels.GetBudgets()), null, null).Subscribe(
                x =>
                {
                   MainThread.BeginInvokeOnMainThread(()=> 
                   {
                       Replace(x.ToModel().ToArray());
                       cache.GetObject<int>("current_position")
                       .Subscribe(x =>
                       {
                           if(x < Items.Count)
                               CurrentBudget = Items[x];
                       });
                   });
                });
        }

        private bool useLoop;
        public bool UseLoop
        {
            get => useLoop;
            set
            {
                useLoop = value;
                OnPropertyChanged();
            }
        }
    
        private void Replace(Budget[] x)
        {

            var removedItems = Items.Except(x).ToArray();
            for(var i = 0; i < removedItems.Length; i++)
                Items.Remove(removedItems[i]);
            for (var i = 0; i < x.Length; i++)
            {
                var index = Items.IndexOf(x[i]);
                if(index == -1)
                    Items.Add(x[i]);
                else
                    Items[index] = x[i];
            }
            
        }

        internal void SetCurrentPosition(int currentPosition)
        {
            cache.InsertObject("current_position", currentPosition);
        }
    }
}
