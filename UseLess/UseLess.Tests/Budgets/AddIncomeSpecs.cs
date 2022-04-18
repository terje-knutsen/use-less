using Moq;
using NUnit.Framework;
using Should;
using SpecsFor.StructureMap;
using UseLess.Domain;
using UseLess.Domain.Enumerations;
using UseLess.Domain.Tests.Budgets;
using UseLess.Domain.Values;
using UseLess.Messages;
using static UseLess.Messages.Exceptions;

namespace UseLess.Tests
{
    internal class AddIncomeSpecs
    {

        public class When_add_income_given_budget_is_not_valid : SpecsFor<Budget>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = new Budget(new object[0]);
                SUT.Load(new[] { new Events.IncomeAddedToBudget(It.IsAny<Guid>(),It.IsAny<Guid>(), It.IsAny<decimal>(), IncomeType.Gift.Name, EntryTime.From(new DateTime(2022,2,2))) });
            }
            [Test]
            public void Then_exception_should_be_thrown()
            {
                Assert.Throws(typeof(InvalidStateException), () => SUT.AddIncome(IncomeId.From(Guid.NewGuid()), Money.From(23m), IncomeType.Bonus, EntryTime.From(new DateTime(2022,2,2))));
            }
        }
        public class When_add_income_given_budget_is_valid : SpecsFor<Budget>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()),BudgetName.From("budget name"));
            }
            protected override void When()
            {
                SUT.AddIncome(IncomeId.From(Guid.NewGuid()), Money.From(32), IncomeType.Bonus, EntryTime.From(new DateTime(2022,2,2)));
            }
            [Test]
            public void Then_income_added_event_should_be_applied()
            {
                SUT.GetChanges().Any(x => x is Events.IncomeAddedToBudget).ShouldBeTrue();
            }
            [Test]
            public void Then_income_amount_should_be_set()
            {
                SUT.Incomes.First().Amount.ShouldEqual(Money.From(32));
            }
            [Test]
            public void Then_income_type_should_be_set()
            {
                SUT.Incomes.First().Type.ShouldEqual(IncomeType.Bonus);
            }
            [Test]
            public void Then_income_id_should_be_set()
            {
                SUT.Incomes.First().Id.ShouldNotEqual(default);
            }
            [Test]
            public void Then_entry_time_should_be_set() 
            {
                SUT.Incomes.First().EntryTime.ShouldNotBeNull();
            }
        }

        public class When_add_income_given_period_is_set : SpecsFor<Budget>
        {
            private readonly StartTime startTime = StartTime.From(new DateTime(2022, 4, 10, 12, 0, 0));

            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()), BudgetName.From("budget name"));
            }
            protected override void Given()
            {
                Given(new CommonContext.PeriodIsSet(startTime));
                base.Given();
            }
            protected override void When()
            {
                SUT.AddIncome(IncomeId.From(Guid.NewGuid()), Money.From(1000m), IncomeType.Gift, EntryTime.From((DateTime)startTime));
            }
            [Test]
            public void Then_amount_left_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.AmountLeftChanged).ShouldBeTrue();
            }
            [Test]
            public void Then_amount_left_should_be_set() 
            {
                Money.From(1000m).Equals(SUT.Details.AmountLeft).ShouldBeTrue();
            }
            [Test]
            public void Then_amount_available_changed_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.AmountAvailableChanged).ShouldBeTrue();
            }
            [Test]
            public void Then_amount_limit_changed_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.AmountLimitChanged).ShouldBeTrue();
            }
            [Test]
            public void Then_available_amount_should_be_set() 
            {
                Money.From(100m).Equals(SUT.Details.AmountAvailable).ShouldBeTrue();
            }
            [Test]
            public void Then_limit_amount_should_be_set() 
            {
                Money.From(100m).Equals(SUT.Details.AmountLimit).ShouldBeTrue();
            }
        }
    }
}
