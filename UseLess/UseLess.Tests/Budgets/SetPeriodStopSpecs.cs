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
    internal class SetPeriodStopSpecs
    {
        public class When_update_period_stop : SpecsFor<Budget>
        {
            private readonly DateTime stopTime = new(2022, 2, 15, 12, 0, 0);
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetName.From("name"));
            }
            protected override void Given()
            {
                Given<PeriodWasSet>();
                base.Given();
            }
            protected override void When()
            {
                SUT.SetPeriodStop(PeriodId.From(Guid.NewGuid()), StopTime.From(stopTime), EntryTime.From(new(2022, 2, 13, 12, 0, 0)));
            }
            [Test]
            public void Then_period_stop_updated_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.PeriodStopWasSet).ShouldBeTrue();
            }
            [Test]
            public void Then_stop_time_should_be_updated() 
            {
                SUT.Period.Stop.ShouldEqual(StopTime.From(stopTime));
            }
            [Test]
            public void Then_period_type_should_be_undefined() 
            {
                SUT.Period.Type.ShouldEqual(PeriodType.Undefined);
            }
            [Test]
            public void Then_period_state_should_be_non_cyclic() 
            {
                SUT.Period.State.ShouldEqual(PeriodState.NonCyclic);
            }
            [Test]
            public void Then_period_stop_set_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.PeriodStopWasSet).ShouldBeTrue();
            }
            [Test]
            public void Then_period_stop_changed_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.PeriodStopChanged).ShouldBeTrue();
            }
            [Test]
            public void Then_period_type_changed_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.PeriodTypeChanged).ShouldBeTrue();
            }
            [Test]
            public void Then_period_state_changed_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.PeriodStateChanged).ShouldBeTrue();
            }
        }

        public class When_update_period_stop_given_period_type_is_already_undefined : SpecsFor<Budget>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetName.From("name"));
            }
            protected override void Given()
            {
                Given<PeriodWasSet>();
                Given(new PeriodTypeWasSet(PeriodType.Undefined));
                base.Given();
            }
            protected override void When()
            {
                SUT.SetPeriodStop(It.IsAny<PeriodId>(), StopTime.From(new(2022, 4, 12, 12, 0, 0)), It.IsAny<EntryTime>());
            }
            [Test]
            public void Then_period_type_changed_event_should_not_be_applied() 
            {
                SUT.GetChanges().Count(x => x is Events.PeriodTypeChanged).ShouldEqual(1);
            }
        }

        private class PeriodWasSet : IContext<Budget>
        {
            public void Initialize(ISpecs<Budget> state)
            {
                state.SUT.SetPeriod(PeriodId.From(Guid.NewGuid()), StartTime.From(new(2022, 2, 1, 12, 0, 0)), EntryTime.From(DateTime.Now));
            }
        }
        private class PeriodTypeWasSet : IContext<Budget>
        {
            private readonly PeriodType periodType;

            public PeriodTypeWasSet(PeriodType periodType)
            {
                this.periodType = periodType;
            }
            public void Initialize(ISpecs<Budget> state)
            {
                state.SUT.SetPeriodType(It.IsAny<PeriodId>(),periodType,It.IsAny<EntryTime>());
            }
        }
    }
}
