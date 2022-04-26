using UseLess.Domain.Entities;
using UseLess.Domain.Enumerations;
using UseLess.Domain.Values;
using UseLess.Framework;
using UseLess.Messages;
using static UseLess.Messages.Events;
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
        internal TotalIncome TotalIncome => TotalIncome.From(Incomes);
        internal TotalOutgo TotalOutgo => TotalOutgo.From(Outgos);
        internal TotalExpense TotalExpense => TotalExpense.From(Expenses);
        public BudgetDetails Details { get; private set; }
        public void AddIncome(IncomeId incomeId, Money amount, IncomeType incomeType, EntryTime entryTime)
        => ApplyWithCalculation(()=> ThrowIfIncomeAlreadyExist(incomeId), entryTime,
            new Events.IncomeAddedToBudget(Id, incomeId, amount, incomeType.Name, entryTime));
        public void ChangeIncomeAmount(IncomeId id, Money amount, EntryTime entryTime)
        => ApplyWithCalculation(()=> Incomes.ById(id).ChangeAmount(amount, entryTime),entryTime);
        public void ChangeIncomeType(IncomeId id, IncomeType incomeType, EntryTime entryTime)
        => Incomes.ById(id).ChangeType(incomeType, entryTime);
        public void AddOutgo(OutgoId outgoId, Money amount, OutgoType unexpected, EntryTime entryTime)
        => ApplyWithCalculation(()=>
            ThrowIfOutgoAlreadyExist(outgoId),entryTime,
            new Events.OutgoAddedToBudget(Id, outgoId, amount, unexpected.Name, entryTime));
        public void ChangeOutgoAmount(OutgoId id, Money amount, EntryTime entryTime)
        => ApplyWithCalculation(()=> Outgos.ById(id).ChangeAmount(amount, entryTime),entryTime);
        public void ChangeOutgoType(OutgoId id, OutgoType type, EntryTime entryTime)
        => Outgos.ById(id).ChangeType(type, entryTime);
        public void AddExpense(ExpenseId expenseId, Money amount, EntryTime entryTime)
        => ApplyWithCalculation(()=> 
            ThrowIfExpenseAlreadyExist(expenseId),entryTime,
            new Events.ExpenseAddedToBudget(Id, expenseId, amount, entryTime));
        public void ChangeExpenseAmount(ExpenseId id, Money amount, EntryTime time)
        => ApplyWithCalculation(() =>
            Expenses.ById(id).ChangeAmount(amount, time), time);
            
        public void AddPeriod(PeriodId periodId, StartTime startTime, EntryTime entryTime)
        => ApplyWithCalculation(()=> { },entryTime, new Events.PeriodCreated
            (Id, periodId, startTime, StopTime.From(startTime, PeriodType.Month),
                PeriodState.Cyclic.Name,
                PeriodType.Month.Name,
                entryTime));

        public void SetPeriodStop(StopTime stopTime, EntryTime entryTime)
            => ApplyWithCalculation(()=> Period.UpdateStop(stopTime, entryTime),entryTime);
        public void SetPeriodType(PeriodType periodType, EntryTime entryTime)
            => Period.UpdateType(periodType, entryTime);
        public void SetPeriodState(PeriodState periodState, EntryTime entryTime)
            => Period.UpdateState(periodState, entryTime);
        public void DeleteIncome(IncomeId incomeId, EntryTime entryTime)
            => ApplyWithCalculation(() => Incomes.ById(incomeId).Delete(entryTime),entryTime);
        public void DeleteOutgo(OutgoId outgoId, EntryTime entryTime)
            => ApplyWithCalculation(()=> Outgos.ById(outgoId).Delete(entryTime),entryTime);
        public void DeleteExpense(ExpenseId expenseId, EntryTime entryTime)
            => ApplyWithCalculation(()=> Expenses.ById(expenseId).Delete(entryTime),entryTime);
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
                    Details = BudgetDetails.WithApplier(Handle,Id);
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
                case Events.PeriodCreated e:
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
                case Events.AmountLeftChanged e:
                    ApplyToEntity(Details, e);
                    break;
                case Events.AmountAvailableChanged e:
                    ApplyToEntity(Details, e);
                    break;
                case Events.AmountLimitChanged e:
                    ApplyToEntity(Details, e);
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
        private void ApplyWithCalculation(Action action, EntryTime entryTime, Event? @event = null)
        {
            Apply(action, @event);
            Details.TryRecalculate(TotalIncome, TotalOutgo, TotalExpense, Period, ThresholdTime.From(entryTime));
        }

        private void ThrowIfOutgoAlreadyExist(OutgoId outgoId)
        {
            if (outgos.Any(x => x.Id == outgoId))
                throw Exceptions.OutgoAlreadyExistException.New;
        }
        private void ThrowIfIncomeAlreadyExist(IncomeId incomeId) 
        {
            if (incomes.Any(x => x.Id == incomeId))
                throw Exceptions.IncomeAlreadyExistException.New;
        }
        private void ThrowIfExpenseAlreadyExist(ExpenseId expenseId)
        {
            if (expenses.Any(x => x.Id == expenseId))
                throw Exceptions.ExpenseAlreadyExistException.New;
        }
    }
}
