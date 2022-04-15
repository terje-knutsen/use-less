using NUnit.Framework;
using Should;
using SpecsFor.StructureMap;
using UseLess.Domain;
using UseLess.Domain.Enumerations;
using UseLess.Domain.Values;
using UseLess.Messages;

namespace UseLess.Tests.Budgets
{
    internal class SetPeriodSpecs
    {
        public class When_set_period : SpecsFor<Budget>
        {
            private readonly DateTime start = StartTime.From(new DateTime(2022, 2, 2, 12, 0, 0));
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()),BudgetName.From("name"));
            }
            protected override void When()
            {
                SUT.AddPeriod(
                    PeriodId.From(Guid.NewGuid()),
                    StartTime.From(start),
                    EntryTime.From(new(2022, 2, 12, 2, 2, 2, 2)));
            }
            [Test]
            public void Then_period_set_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.PeriodCreated).ShouldBeTrue();
            }
            [Test]
            public void Then_period_should_be_set() 
            {
                SUT.Period.Start.ShouldEqual(StartTime.From(start));
            }
            [Test]
            public void Then_period_type_should_be_set() 
            {
                SUT.Period.Type.ShouldEqual(PeriodType.Month);
            }
            [Test]
            public void Then_period_state_should_be_set() 
            {
                SUT.Period.State.ShouldEqual(PeriodState.Cyclic);
            }
            [Test]
            public void Then_period_stop_should_be_one_month_from_start() 
            {
                SUT.Period.Stop.ShouldEqual(StopTime.From(start.AddMonths(1)));
            }
        }
    }
}
