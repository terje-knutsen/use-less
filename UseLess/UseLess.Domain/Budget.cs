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
        public Budget(IEnumerable<object> events)
        {
            foreach (var e in events)
                When(e);
        }

        private Budget(BudgetName name)
            => Apply(new Events.BudgetCreated(Guid.NewGuid(), name, DateTime.Now));
        
        public BudgetName? Name { get; private set; }
        public Period Period { get; private set; }
        public IEnumerable<Income> Incomes => incomes;
        public IEnumerable<Outgo> Outgos => outgos;
        public IEnumerable<Expense> Expenses => expenses;

        public void AddIncome(IncomeId id,Money amount, IncomeType incomeType, EntryTime entryTime)
        => Apply(new Events.IncomeAddedToBudget(id, amount, incomeType.Name,entryTime));
        public void ChangeIncomeAmount(IncomeId id, Money amount, EntryTime entryTime)
        => Incomes.ById(id).ChangeAmount(amount, entryTime);
        public void ChangeIncomeType(IncomeId id, IncomeType incomeType, EntryTime entryTime)
        => Incomes.ById(id).ChangeType(incomeType, entryTime);
        public void AddOutgo(OutgoId id, Money amount, OutgoType unexpected, EntryTime entryTime)
        => Apply(new Events.OutgoAddedToBudget(id, amount, unexpected.Name, entryTime));
        public void ChangeOutgoAmount(OutgoId id, Money amount, EntryTime entryTime)
        => Outgos.ById(id).ChangeAmount(amount, entryTime);
        public void ChangeOutgoType(OutgoId id, OutgoType type, EntryTime entryTime)
        => Outgos.ById(id).ChangeType(type, entryTime);
        public void AddExpense(ExpenseId id, Money amount, EntryTime time)
            => Apply(new Events.ExpenseAddedToBudget(id, amount, time));
        public void ChangeExpenseAmount(ExpenseId id, Money amount, EntryTime time)
            => Expenses.ById(id).ChangeAmount(amount, time);
        public void AddPeriod(PeriodId periodId, StartTime startTime, EntryTime entryTime)
            => Apply(new Events.PeriodAddedToBudget(periodId, startTime, entryTime));
        public void SetPeriodStop(StopTime stopTime, EntryTime entryTime)
            => Period.UpdateStop(stopTime, entryTime);
        public void SetPeriodType(PeriodType periodType, EntryTime entryTime)
            => Period.UpdateType(periodType, entryTime);
        public void SetPeriodState(PeriodState periodState, EntryTime entryTime)
            => Period.UpdateState(periodState, entryTime);
        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.BudgetCreated e:
                    Id = BudgetId.From(e.Id);
                    Name = BudgetName.From(e.Name);
                    break;
                case Events.IncomeAddedToBudget e:
                    var income = Income.WithApplier(Handle);
                    ApplyToEntity(income, e);
                    incomes.Add(income);
                    break;
                case Events.OutgoAddedToBudget e:
                    var outgo = Outgo.WithApplier(Handle);
                    ApplyToEntity(outgo, e);
                    outgos.Add(outgo);
                    break;
                case Events.ExpenseAddedToBudget e:
                    var expense = Expense.WithApplier(Handle);
                    ApplyToEntity(expense, e);
                    expenses.Add(expense);
                    break;
                case Events.PeriodAddedToBudget e:
                    Period = Period.WithApplier(Handle);
                    ApplyToEntity(Period, e);
                    break;
                case Events.IncomeAmountChanged e:
                    ApplyToEntity(Incomes.FirstOrDefault(x => x.Id == e.Id),e);
                    break;
                case Events.IncomeTypeChanged e:
                    ApplyToEntity(Incomes.FirstOrDefault(x => x.Id == e.Id), e);
                    break;
                case Events.OutgoAmountChanged e:
                    ApplyToEntity(Outgos.FirstOrDefault(x => x.Id == e.Id),e);
                    break;
                case Events.OutgoTypeChanged e:
                    ApplyToEntity(Outgos.FirstOrDefault(x => x.Id == e.Id), e);
                    break;
                case Events.ExpenseAmountChanged e:
                    ApplyToEntity(Expenses.FirstOrDefault(x => x.Id == e.Id), e);
                    break;
            }
        }
        protected override void EnsureValidState()
        {
            if (Id == default(Guid))
                throw InvalidStateException.WithMessage("Not initialized");
            if (Period != null && Period.Stop.IsBefore(Period.Start))
                throw InvalidStateException.WithMessage("Period is invalid");
        }

        public static Budget Create(BudgetName name)
            => new(name);

    }
}
