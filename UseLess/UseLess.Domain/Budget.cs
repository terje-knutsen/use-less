﻿using UseLess.Domain.Entities;
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
        public Budget(){}
        private Budget(BudgetName name)
            => Apply(new Events.BudgetCreated(Guid.NewGuid(), DateTime.Now, name));
        
        public BudgetName? Name { get; private set; }
        public IEnumerable<Income> Incomes => incomes;
        public IEnumerable<Outgo> Outgos => outgos;



        public void AddIncome(IncomeId id,Money amount, IncomeType incomeType)
            => Apply(new Events.IncomeAdded(id, amount, incomeType.Name));
        public void AddOutgo(OutgoId id, Money amount, OutgoType unexpected)
        => Apply(new Events.OutgoAdded(id, amount, unexpected.Name));
        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.BudgetCreated e:
                    Id = BudgetId.From(e.Id);
                    Name = BudgetName.From(e.Name);
                    break;
                case Events.IncomeAdded e:
                    var income = new Income(Handle);
                    ApplyToEntity(income, e);
                    incomes.Add(income);
                    break;
                case Events.OutgoAdded e:
                    var outgo = new Outgo(Handle);
                    ApplyToEntity(outgo, e);
                    outgos.Add(outgo);
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
