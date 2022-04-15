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
        ICollectionQueryStore<ReadModels.Income>
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
            Events.IncomeDeleted e => 
                DeleteIncome(e),
            Events.OutgoDeleted e => 
                DeleteOutgo(e),
            Events.ExpenseDeleted e => 
                DeleteExpense(e),
            Events.PeriodAddedToBudget e =>
                UpdateBudget(e.Id, b => { b.Start = e.StartTime; b.End = e.StopTime; b.EntryTime = e.EntryTime; }),
            Events.PeriodStopChanged e => 
                UpdateBudget(e.Id, b => b.End = e.StopTime),
            _ => Task.CompletedTask
        };




        public ReadModels.Budget Get(Guid id)
        => budgets.First(x => x.BudgetId == id);

        public IEnumerable<ReadModels.Income> GetAll(Guid id)
        => incomes.Where(x => x.ParentId == id);



        private IDictionary<string, IEnumerable<object>> keyValuePairs = new Dictionary<string, IEnumerable<object>>();
    }
}
