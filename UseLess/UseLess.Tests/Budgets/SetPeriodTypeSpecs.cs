using Moq;
using NUnit.Framework;
using Should;
using SpecsFor.Core;
using SpecsFor.StructureMap;
using UseLess.Domain;
using UseLess.Domain.Enumerations;
using UseLess.Domain.Values;
using UseLess.Messages;

namespace UseLess.Tests.Budgets
{
    internal class SetPeriodTypeSpecs
    {
        public class When_set_period_type_given_period_exist : SpecsFor<Budget>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()),BudgetName.From("name"));
            }
            protected override void Given()
            {
                Given<PeriodWasAdded>();
                base.Given();
            }
            protected override void When()
            {
                SUT.SetPeriodType(PeriodType.Week, It.IsAny<EntryTime>());
            }
            [Test]
            public void Then_period_type_updated_event_should_be_applied()
            {
                SUT.GetChanges().Any(x => x is Events.PeriodTypeChanged).ShouldBeTrue();
            }
            [Test]
            public void Then_period_type_should_be_set()
            {
                SUT.Period.Type.ShouldEqual(PeriodType.Week);
            }
            [Test]
            public void Then_period_stop_changed_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.PeriodStopChanged).ShouldBeTrue();
            }
        }

        public class When_set_period_type_given_is_undefined : SpecsFor<Budget>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()),BudgetName.From("name"));
            }
            protected override void Given()
            {
                Given<PeriodWasAdded>();
                base.Given();
            }
            protected override void When()
            {
                SUT.SetPeriodType(PeriodType.Undefined, It.IsAny<EntryTime>());
            }
            [Test]
            public void Then_period_stop_changed_event_should_not_be_applied() 
            {
                SUT.GetChanges().Count(x => x is Events.PeriodStopChanged).ShouldEqual(0);
            }
        }

        public class When_set_period_type_given_is_set_to_same_type : SpecsFor<Budget>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()),BudgetName.From("name"));
            }
            protected override void Given()
            {
                Given<PeriodWasAdded>();
                base.Given();
            }
            protected override void When()
            {
                SUT.SetPeriodType(PeriodType.Month,It.IsAny<EntryTime>());
            }
            [Test]
            public void Then_period_type_changed_should_not_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.PeriodTypeChanged).ShouldBeFalse();
            }
            [Test]
            public void Then_period_stop_changed_should_not_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.PeriodStopChanged).ShouldBeFalse();
            }

        }
        private class PeriodWasAdded : IContext<Budget>
        {
            public void Initialize(ISpecs<Budget> state)
            {
                state.SUT.AddPeriod(PeriodId.From(Guid.NewGuid()), StartTime.From(new(2022, 2, 12, 12, 0, 0)), EntryTime.From(new(2022, 2, 12, 0, 0, 0)));
            }
        }
    }
}
