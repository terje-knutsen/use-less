using Moq;
using NUnit.Framework;
using Should;
using SpecsFor.StructureMap;
using UseLess.Domain;
using UseLess.Domain.Enumerations;
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
                SUT = new Budget();
                SUT.Load(new[] { new Events.IncomeAdded(It.IsAny<Guid>(), It.IsAny<decimal>(), IncomeType.Gift.Name, EntryTime.From(new DateTime(2022,2,2))) });
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
                SUT = Budget.Create(BudgetName.From("budget name"));
            }
            protected override void When()
            {
                SUT.AddIncome(IncomeId.From(Guid.NewGuid()), Money.From(32), IncomeType.Bonus, EntryTime.From(new DateTime(2022,2,2)));
            }
            [Test]
            public void Then_income_added_event_should_be_applied()
            {
                SUT.GetChanges().Any(x => x is Events.IncomeAdded).ShouldBeTrue();
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
        }
    }
}
