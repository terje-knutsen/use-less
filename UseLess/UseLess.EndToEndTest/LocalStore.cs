using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using UseLess.Framework;
using UseLess.Messages;
using UseLess.Services.Api;

namespace UseLess.EndToEndTest
{
    public partial class LocalStore : IAggregateStore, 
        IQueryUpdate, 
        IQueryStore<ReadModels.Budget>,
        IQueryStore<ReadModels.Income>,
        IQueryStore<ReadModels.Outgo>,
        IQueryStore<ReadModels.Expense>,
        IQueryStore<ReadModels.Period>,
        ICollectionQueryStore<ReadModels.Income>,
        ICollectionQueryStore<ReadModels.Outgo>,
        ICollectionQueryStore<ReadModels.Expense>
    {
        public Task<bool> Exists<T, TId>(TId aggregateId)
        => Task.Factory.StartNew(()=> keyValuePairs.ContainsKey($"{typeof(T).Name}-{aggregateId}"));

        public Task<T> Load<T, TId>(TId aggregateId) where T : AggregateRoot<TId>
        {
            var events = keyValuePairs[$"{typeof(T).Name}-{aggregateId}"];
            var aggregate = Activator.CreateInstance(typeof(T), events) as T;
            if (aggregate == null)
                throw new InvalidDataException("Aggregate does not exist");
            return Task.Factory.StartNew(()=>  aggregate);
        }

        public async Task Save<T, TId>(T aggregate) where T : AggregateRoot<TId>
        {
            var changes = aggregate.GetChanges();
               await Update(aggregate.GetChanges());
               await AddEvents<T, TId>(aggregate.Id, aggregate.GetChanges());
        }

        private Task AddEvents<T, TId>(TId id, IEnumerable<object> events) where T : AggregateRoot<TId>
        {
            var key = $"{typeof(T).Name}-{id}";
            Action action;
            if (keyValuePairs.ContainsKey(key))
            {
                action = () =>
                {
                    var existingEvents = keyValuePairs[key].ToList();
                    existingEvents.AddRange(events);
                    keyValuePairs[key] = existingEvents;
                };
            }
            else 
                action = () => keyValuePairs.Add($"{typeof(T).Name}-{id}", events);
            return Task.Factory.StartNew(() => action());
        }

        public async Task Update(IEnumerable<object> events)
        {
            foreach(var @event in events) 
            {
                await HandleEvent(@event);
            }
        }
        private Task HandleEvent(object @event)
        => @event switch
        {
            Events.BudgetCreated e =>
                 CreateBudget(e),
            Events.IncomeAddedToBudget e =>
                  AddIncome(e),
            Events.OutgoAddedToBudget e => 
                 AddOutgo(e),
            Events.ExpenseAddedToBudget e => 
                 AddExpense(e),
            Events.IncomeAmountChanged e => 
                ChangeIncomeAmount(e),
            Events.OutgoAmountChanged e => 
                ChangeOutgoAmount(e),
            Events.ExpenseAmountChanged e => 
                ChangeExpenseAmount(e),
            Events.BudgetDeleted e => 
                UpdateBudget(e.Id, b => b.State = e.State),
            Events.IncomeDeleted e => 
                DeleteIncome(e),
            Events.OutgoDeleted e => 
                DeleteOutgo(e),
            Events.ExpenseDeleted e => 
                DeleteExpense(e),
            Events.PeriodCreated e =>
                AddPeriod(e, UpdateBudget(e.Id, b => { b.Start = e.Start; b.End = e.Stop; b.EntryTime = e.EntryTime; })),
            Events.PeriodStopChanged e => 
                UpdatePeriod(e.PeriodId,x => x.Stop = e.StopTime, UpdateBudget(e.Id, b => b.End = e.StopTime)),
            Events.IncomeTypeChanged e => 
                ChangeIncomeType(e),
            Events.OutgoTypeChanged e => 
                ChangeOutgoType(e),
            Events.PeriodTypeChanged e => 
                ChangePeriodType(e),
            Events.PeriodStateChanged e =>
                ChangePeriodState(e),
            Events.AmountAvailableChanged e => 
                UpdateBudget(e.Id, b => b.Available = e.AmountAvailable),
            Events.AmountLeftChanged e => 
                UpdateBudget(e.Id, b => b.Left = e.AmountLeft),
            Events.AmountLimitChanged e => 
                UpdateBudget(e.Id, b => b.Limit = e.AmountLimit),
            _ => Task.CompletedTask
        };

        public ReadModels.Budget Get(Guid id)
        => budgets.First(x => x.BudgetId == id);

        public IEnumerable<ReadModels.Income> GetAll(Guid id)
        => incomes.Where(x => x.ParentId == id);

        ReadModels.Income IQueryStore<ReadModels.Income>.Get(Guid id)
        => incomes.First(x => x.IncomeId == id);

        ReadModels.Outgo IQueryStore<ReadModels.Outgo>.Get(Guid id)
        => outgos.First(x => x.OutgoId == id);

        ReadModels.Expense IQueryStore<ReadModels.Expense>.Get(Guid id)
        => expenses.First(x => x.ExpenseId == id);

        ReadModels.Period IQueryStore<ReadModels.Period>.Get(Guid id)
        => periods.First(x => x.PeriodId == id);

        IEnumerable<ReadModels.Outgo> ICollectionQueryStore<ReadModels.Outgo>.GetAll(Guid id)
        => outgos.Where(x => x.ParentId == id);

        IEnumerable<ReadModels.Expense> ICollectionQueryStore<ReadModels.Expense>.GetAll(Guid id)
        => expenses.Where(x => x.ParentId == id);

        private IDictionary<string, IEnumerable<object>> keyValuePairs = new Dictionary<string, IEnumerable<object>>();
    }
}
