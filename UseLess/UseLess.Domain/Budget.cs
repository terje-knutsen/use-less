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

        private Budget(BudgetId budgetId, BudgetName name)
            => Apply(new Events.BudgetCreated(budgetId, name, DateTime.Now));
        public BudgetState State { get; private set; }
        public BudgetName? Name { get; private set; }
        public Period Period { get; private set; }
        public IEnumerable<Income> Incomes => incomes;
        public IEnumerable<Outgo> Outgos => outgos;
        public IEnumerable<Expense> Expenses => expenses;

        public void AddIncome(IncomeId incomeId,Money amount, IncomeType incomeType, EntryTime entryTime)
        => Apply(new Events.IncomeAddedToBudget(Id,incomeId, amount, incomeType.Name,entryTime));
        public void ChangeIncomeAmount(IncomeId id, Money amount, EntryTime entryTime)
        => Incomes.ById(id).ChangeAmount(amount, entryTime);
        public void ChangeIncomeType(IncomeId id, IncomeType incomeType, EntryTime entryTime)
        => Incomes.ById(id).ChangeType(incomeType, entryTime);
        public void AddOutgo(OutgoId outgoId, Money amount, OutgoType unexpected, EntryTime entryTime)
        => Apply(new Events.OutgoAddedToBudget(Id, outgoId, amount, unexpected.Name, entryTime));
        public void ChangeOutgoAmount(OutgoId id, Money amount, EntryTime entryTime)
        => Outgos.ById(id).ChangeAmount(amount, entryTime);
        public void ChangeOutgoType(OutgoId id, OutgoType type, EntryTime entryTime)
        => Outgos.ById(id).ChangeType(type, entryTime);
        public void AddExpense(ExpenseId expenseId, Money amount, EntryTime time)
            => Apply(new Events.ExpenseAddedToBudget(Id,expenseId, amount, time));
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
        public void DeleteIncome(IncomeId incomeId, EntryTime entryTime)
            => Apply(new Events.IncomeDeleted(Id,incomeId, entryTime));
        public void DeleteOutgo(OutgoId outgoId, EntryTime entryTime)
            => Apply(new Events.OutgoDeleted(Id, outgoId, entryTime));
        public void DeleteExpense(ExpenseId expenseId, EntryTime entryTime)
            => Apply(new Events.ExpenseDeleted(Id, expenseId, entryTime));
        public void Delete(EntryTime entryTime)
            => Apply(new Events.BudgetDeleted(Id,entryTime));
        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.BudgetCreated e:
                    Id = BudgetId.From(e.Id);
                    Name = BudgetName.From(e.Name);
                    State = BudgetState.Active;
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
                    ApplyToEntity(Incomes.FirstOrDefault(x => x.Id == e.IncomeId),e);
                    break;
                case Events.IncomeTypeChanged e:
                    ApplyToEntity(Incomes.FirstOrDefault(x => x.Id == e.IncomeId), e);
                    break;
                case Events.OutgoAmountChanged e:
                    ApplyToEntity(Outgos.FirstOrDefault(x => x.Id == e.OutgoId),e);
                    break;
                case Events.OutgoTypeChanged e:
                    ApplyToEntity(Outgos.FirstOrDefault(x => x.Id == e.OutgoId), e);
                    break;
                case Events.ExpenseAmountChanged e:
                    ApplyToEntity(Expenses.FirstOrDefault(x => x.Id == e.ExpenseId), e);
                    break;
                case Events.IncomeDeleted e:
                    var incomeToRemove = incomes.FirstOrDefault(x => x.Id == e.IncomeId);
                    if(incomeToRemove != null)
                        incomes.Remove(incomeToRemove);
                    break;
                case Events.OutgoDeleted e:
                    var outgoToRemove = outgos.FirstOrDefault(x => x.Id == e.OutgoId);
                    if (outgoToRemove != null)
                        outgos.Remove(outgoToRemove);
                    break;
                case Events.ExpenseDeleted e:
                    var expenseToRemove = expenses.FirstOrDefault(x => x.Id == e.ExpenseId);
                    if (expenseToRemove != null)
                        expenses.Remove(expenseToRemove);
                    break;
                case Events.BudgetDeleted e:
                    State = BudgetState.Deleted;
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

        public static Budget Create(BudgetId budgetId,BudgetName name)
            => new(budgetId, name);

    }
}
