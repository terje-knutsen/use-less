using Akavache;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Useless.Api;
using Useless.Framework;
using Useless.ViewModels.Bases;
using UseLess.Messages;
using Xamarin.Forms;

namespace Useless.ViewModels
{
    public sealed class StartupViewModel : CollectionViewModel<ReadModels.Budget>
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
        }
        private Guid budgetId;

        public Guid BudgetId
        {
            get { return budgetId; }
            set 
            {
                budgetId = value;
                OnPropertyChanged();
            }
        }
        private bool shouldScroll;

        public bool ShouldScroll
        {
            get { return shouldScroll; }
            set 
            { 
                shouldScroll = value;
                OnPropertyChanged();
            }
        }
        protected override async Task OnItemSelected(ReadModels.Budget item)
        {
            await Task.CompletedTask;
        }
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

        public int Position
        {
            get { return position; }
            set 
            { 
                position = value;
                OnPropertyChanged();
            }
        }

        public ICommand IncomeCommand => new Command<Guid>(async x
        => await NavService.NavigateTo<IncomesViewModel, Guid>(x));
        public ICommand OutgoCommand => new Command<Guid>(async x
        => await NavService.NavigateTo<OutgosViewModel, Guid>(x));
        public ICommand ExpenseCommand => new Command<Guid>(async x
        => await NavService.NavigateTo<ExpensesViewModel, Guid>(x));

        public ICommand SaveCommand =>
            saveCommand ??= new Command<string>(async (x) => await Save(x));
        private async Task Save(string name)
        {
            var id = Guid.NewGuid();
            Items.Add(new ReadModels.Budget { BudgetId = id, Name = name });
            await commandApplier.Apply(id, new BudgetCommands.V1.Create { BudgetId = id, Name = name });
            NewItemRequested = false;
            cache.InsertObject(nameof(ReadModels.Budget.BudgetId), id);
            await UpdateCollection();
            await Device.InvokeOnMainThreadAsync(() => Position = Items.Count - 1);
        }

        protected override async Task OnNewItemCommand()
        => await Task.Run(() => NewItemRequested = true);

        protected override void InitCollection()
        {
            cache.GetAndFetchLatest("budgets", async () => { return await budgetProjection.GetAsync(new QueryModels.GetBudgets()); })
            .Subscribe(x => Items = new ObservableCollection<ReadModels.Budget>(x));
        }
        private async Task UpdateCollection()
        {
            var budgets = await budgetProjection.GetAsync(new QueryModels.GetBudgets());
            Items = new ObservableCollection<ReadModels.Budget>(budgets);
        }
        protected override void SetCurrent()
        {
            ShouldScroll = true;
            cache.GetObject<Guid>(nameof(ReadModels.Budget.BudgetId)).Subscribe(x => BudgetId = x);
        }
    }
}
