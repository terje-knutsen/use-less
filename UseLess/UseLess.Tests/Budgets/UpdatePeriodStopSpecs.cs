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
using UseLess.Domain.Values;
using UseLess.Messages;

namespace UseLess.Tests.Budgets
{
    internal class UpdatePeriodStopSpecs
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
                SUT.UpdateStop(PeriodId.From(Guid.NewGuid()), StopTime.From(stopTime), EntryTime.From(new(2022, 2, 13, 12, 0, 0)));
            }
            [Test]
            public void Then_period_stop_updated_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.PeriodStopUpdated).ShouldBeTrue();
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

        }
        private class PeriodWasSet : IContext<Budget>
        {
            public void Initialize(ISpecs<Budget> state)
            {
                state.SUT.SetPeriod(PeriodId.From(Guid.NewGuid()), StartTime.From(new(2022, 2, 1, 12, 0, 0)), StopTime.Empty, Domain.Enumerations.PeriodType.Month, IsCyclic.From(true), EntryTime.From(DateTime.Now));
            }
        }
    }
}
