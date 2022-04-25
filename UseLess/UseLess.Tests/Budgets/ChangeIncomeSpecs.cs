using Moq;
using NUnit.Framework;
using Should;
using SpecsFor.Core;
using SpecsFor.StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Domain;
using UseLess.Domain.Enumerations;
using UseLess.Domain.Tests.Budgets;
using UseLess.Domain.Values;
using UseLess.Messages;
using static UseLess.Messages.Exceptions;

namespace UseLess.Tests.Budgets
{
    internal class ChangeIncomeSpecs
    {
        public class When_change_income : SpecsFor<Budget>
        {
            private readonly IncomeId incomeId = IncomeId.From(Guid.NewGuid());
            private readonly StartTime startTime = StartTime.From(new DateTime(2022, 4, 10, 12, 0, 0));
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()),BudgetName.From("name"));
            }
            protected override void Given()
            {
                Given(new IncomeWasSet(incomeId));
                Given(new CommonContext.PeriodIsSet(startTime));
                base.Given();
            }
            protected override void When()
            {
                SUT.ChangeIncomeAmount(incomeId, Money.From(100), EntryTime.From(((DateTime)startTime).AddDays(2)));
            }
            [Test]
            public void Then_income_amount_changed_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.IncomeAmountChanged).ShouldBeTrue();
            }
            [Test]
            public void Then_income_amount_should_be_set() 
            {
                SUT.Incomes.First().Amount.ShouldEqual(Money.From(100));
            }
            [Test]
            public void Then_amount_left_limit_and_available_events_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.AmountAvailableChanged).ShouldBeTrue();
            }
        }
        public class When_change_income_given_income_does_not_exist : SpecsFor<Budget>
        {
            private readonly IncomeId incomeId = IncomeId.From(Guid.NewGuid());
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()),BudgetName.From("name"));
            }
            protected override void Given()
            {
                Given(new IncomeWasSet(It.IsAny<IncomeId>()));
                base.Given();
            }
            [Test]
            public void Then_exception_should_be_thrown() 
            {
                Assert.Throws<InvalidOperationException>(() => SUT.ChangeIncomeAmount(incomeId, Money.From(100), It.IsAny<EntryTime>()));
            }
        }
        public class When_change_income_type : SpecsFor<Budget> 
        {
            private readonly IncomeId incomeId = IncomeId.From(Guid.NewGuid());
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()),BudgetName.From("name"));
            }
            protected override void Given()
            {
                Given(new IncomeWasSet(incomeId));
                base.Given();
            }
            protected override void When()
            {
                SUT.ChangeIncomeType(incomeId, IncomeType.Perks, It.IsAny<EntryTime>());
            }
            [Test]
            public void Then_income_type_changed_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.IncomeTypeChanged).ShouldBeTrue();
            }
            [Test]
            public void Then_income_type_should_be_set() 
            {
                SUT.Incomes.First().Type.ShouldEqual(IncomeType.Perks);
            }
        }
        public class When_change_income_type_given_income_does_not_exist : SpecsFor<Budget>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()),BudgetName.From("name"));
            }
            [Test]
            public void Then_invalid_state_exeption_should_be_thrown() 
            {
                Assert.Throws<InvalidOperationException>(()=> SUT.ChangeIncomeType(It.IsAny<IncomeId>(),IncomeType.Gift, It.IsAny<EntryTime>()));
            }
        }

        private class IncomeWasSet : IContext<Budget>
        {
            private readonly IncomeId incomeId;

            public IncomeWasSet(IncomeId incomeId)
            {
                this.incomeId = incomeId;
            }
            public void Initialize(ISpecs<Budget> state)
            {
                state.SUT.AddIncome(incomeId, Money.From(200), IncomeType.Bonus, It.IsAny<EntryTime>());
            }
        }
    }
}
