using NUnit.Framework;
using Should;
using SpecsFor.StructureMap;
using UseLess.Domain;
using UseLess.Domain.Enumerations;
using UseLess.Domain.Exceptions;
using UseLess.Domain.Values;
using UseLess.Framework;
using UseLess.Messages;
using static UseLess.Messages.Exceptions;

namespace UseLess.Tests.Budgets
{
    internal class SetPeriodSpecs
    {
        public class When_set_period : SpecsFor<Budget>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetName.From("name"));
            }
            protected override void When()
            {
                SetPeriod(SUT,type: PeriodType.Month.Name, isCyclic: true);
            }
            [Test]
            public void Then_period_set_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.PeriodSet).ShouldBeTrue();
            }
            [Test]
            public void Then_period_should_be_set() 
            {
                SUT.Period.Start.ShouldEqual(StartTime.From(Start));
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
                SUT.Period.Stop.ShouldEqual(StopTime.From(Start.AddMonths(1)));
            }
            [Test]
            public void Then_event_period_stop_should_be_one_month_from_start() 
            {
              var e =  SUT.GetChanges().First(x => x is Events.PeriodSet) as Events.PeriodSet;
              e?.StopTime.ShouldEqual(Start.AddMonths(1));
            }
        }
        public class When_set_period_given_is_not_cyclic : SpecsFor<Budget> 
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetName.From("name"));
            }
            protected override void When()
            {
                SetPeriod(SUT,isCyclic:false);
            }
            [Test]
            public void Then_period_state_should_not_be_cyclic() 
            {
                SUT.Period.State.ShouldEqual(PeriodState.NonCyclic);
            }
        }
        public class When_set_period_given_is_week : SpecsFor<Budget>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetName.From("name"));
            }
            protected override void When()
            {
                SetPeriod(SUT, type: PeriodType.Week.Name);
            }
            [Test]
            public void Then_period_should_be_one_week() 
            {
                SUT.Period.Stop.ShouldEqual(StopTime.From(Start.AddDays(7)));
            }
        }
        public class When_set_period_given_is_year : SpecsFor<Budget>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetName.From("name"));
            }
            protected override void When()
            {
                SetPeriod(SUT, type: PeriodType.Year.Name);
            }
            [Test]
            public void Then_stop_time_should_be_set_a_year_from_start() 
            {
                SUT.Period.Stop.ShouldEqual(StopTime.From(Start.AddYears(1)));
            }
        }
        public class When_set_period_without_stop_time_given_type_is_undefined : SpecsFor<Budget> 
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetName.From("name"));
            }
            [Test]
            public void Then_period_exception_should_be_thrown()
            { 
                Assert.Throws<PeriodException>(()=> SetPeriod(SUT,type:PeriodType.Undefined.Name));
            }
        }
        public class When_set_period_with_stop_before_start : SpecsFor<Budget>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetName.From("name"));
            }
            [Test]
            public void Then_invalid_state_exception_should_be_thrown() 
            {
                Assert.Throws<InvalidStateException>(()=> SetPeriod(SUT,stopTime: StopTime.From(Start.AddDays(-1))));
            }
        }
        private static readonly DateTime Start = StartTime.From(new DateTime(2022, 2, 2, 12, 0, 0));
        private static void SetPeriod(Budget budget, StopTime? stopTime = default,string type = "WEEK",bool isCyclic = false)
        {
            budget.SetPeriod(
                PeriodId.From(Guid.NewGuid()), 
                StartTime.From(Start), 
                stopTime??StopTime.Empty, 
                Enumeration.FromString<PeriodType>(type), 
                IsCyclic.From(isCyclic), 
                EntryTime.From(new(2022, 2, 12, 2, 2, 2, 2)));
        }
    }
}
