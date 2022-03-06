using UseLess.Domain.Entities;
using UseLess.Domain.Enumerations;
using UseLess.Domain.Values;
using UseLess.Framework;
using UseLess.Messages;
using static UseLess.Messages.Exceptions;

namespace UseLess.Domain
{
    public sealed class Budget : AggregateRoot<BudgetId>
    {
        private readonly List<Income> incomes = new();
        private readonly List<Outgo> outgos = new();
        private readonly List<Expense> expenses = new();
        public Budget(){}
        private Budget(BudgetName name)
            => Apply(new Events.BudgetCreated(Guid.NewGuid(), DateTime.Now, name));
        
        public BudgetName? Name { get; private set; }
        public Period Period { get; private set; }
        public IEnumerable<Income> Incomes => incomes;
        public IEnumerable<Outgo> Outgos => outgos;
        public IEnumerable<Expense> Expenses => expenses;



        public void AddIncome(IncomeId id,Money amount, IncomeType incomeType, EntryTime entryTime)
            => Apply(new Events.IncomeAdded(id, amount, incomeType.Name,entryTime));
        public void AddOutgo(OutgoId id, Money amount, OutgoType unexpected, EntryTime entryTime)
        => Apply(new Events.OutgoAdded(id, amount, unexpected.Name, entryTime));
        public void AddExpense(ExpenseId id, Money amount, EntryTime time)
            => Apply(new Events.ExpenseAdded(id, amount, time));
        public void SetPeriod(PeriodId periodId, StartTime startTime,StopTime stopTime, PeriodType type, IsCyclic isCyclic, EntryTime entryTime)
            => Apply(new Events.PeriodSet(periodId, startTime,stopTime, type.Name, isCyclic, entryTime));
        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.BudgetCreated e:
                    Id = BudgetId.From(e.Id);
                    Name = BudgetName.From(e.Name);
                    break;
                case Events.IncomeAdded e:
                    var income = Income.WithApplier(Handle);
                    ApplyToEntity(income, e);
                    incomes.Add(income);
                    break;
                case Events.OutgoAdded e:
                    var outgo = Outgo.WithApplier(Handle);
                    ApplyToEntity(outgo, e);
                    outgos.Add(outgo);
                    break;
                case Events.ExpenseAdded e:
                    var expense = Expense.WithApplier(Handle);
                    ApplyToEntity(expense, e);
                    expenses.Add(expense);
                    break;
                case Events.PeriodSet e:
                    Period = Period.WithApplier(Handle);
                    ApplyToEntity(Period, e);
                    break;
            }
        }
        protected override void EnsureValidState()
        {
            if (Id == default(Guid))
                throw InvalidStateException.WithMessage("Not initialized");
        }

        public static Budget Create(BudgetName name)
            => new(name);
    }
}
