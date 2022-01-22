using UseLess.Domain.Enumerations;
using UseLess.Domain.Values;
using UseLess.Framework;
using UseLess.Messages;
using static UseLess.Messages.Exceptions;

namespace UseLess.Domain
{
    public sealed class Budget : AggregateRoot<BudgetId>
    {
        private readonly List<Money> incomes = new();
        private BudgetName? name;
        public Budget(){}
        private Budget(BudgetName name)
            => Apply(new Events.BudgetCreated(Guid.NewGuid(), DateTime.Now, name));
        
        public BudgetName? Name { get => name; private set => name = value ?? BudgetName.Empty; }
        public IEnumerable<Money> Incomes => incomes;
        
        public void AddIncome(Money amount, IncomeType incomeType)
            => Apply(new Events.IncomeAdded(Id, amount, incomeType.Name));
        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.BudgetCreated e:
                    Id = BudgetId.From(e.Id);
                    Name = BudgetName.From(e.Name);
                    break;
                case Events.IncomeAdded e:
                    incomes.Add(Money.From(e.Amount));
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
