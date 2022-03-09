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
    internal class SetPeriodStateSpecs
    {
        public class When_set_period_state : SpecsFor<Budget>
        {
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
                SUT.SetPeriodState(PeriodState.NonCyclic, It.IsAny<EntryTime>());
            }
            [Test]
            public void Then_period_state_should_be_set()
            {
                SUT.Period.State.ShouldEqual(PeriodState.NonCyclic);
            }
        }
        private class PeriodWasSet : IContext<Budget>
        {
            public void Initialize(ISpecs<Budget> state)
            {
                state.SUT.AddPeriod(It.IsAny<PeriodId>(), StartTime.From(new(2022, 2, 28, 12, 0, 0)), It.IsAny<EntryTime>());
            }
        }
    }
}
