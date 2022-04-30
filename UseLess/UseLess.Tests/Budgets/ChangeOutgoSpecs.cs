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

namespace UseLess.Tests.Budgets
{
    internal class ChangeOutgoSpecs
    {
        public class When_change_outgo_amount : SpecsFor<Budget>
        {
            private readonly OutgoId outgoId = OutgoId.From(Guid.NewGuid());
            private readonly StartTime startTime = StartTime.From(new DateTime(2022, 4, 10, 12, 0, 0));
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()),BudgetName.From("name"));
            }
            protected override void Given()
            {
                Given(new OutgoExist(outgoId,EntryTime.From(startTime)));
                Given(new CommonContext.PeriodIsSet(startTime));
                base.Given();
            }
            protected override void When()
            {
                SUT.ChangeOutgoAmount(outgoId, Money.From(1000), It.IsAny<EntryTime>());
            }
            [Test]
            public void Then_outgo_amount_changed_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.OutgoAmountChanged).ShouldBeTrue();
            }
            [Test]
            public void Then_budget_amount_should_be_changed() 
            {
                SUT.Outgos.First().Amount.ShouldEqual(Money.From(1000));
            }
            [Test]
            public void Then_amount_left_changed_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.AmountAvailableChanged).ShouldBeTrue();
            }
        }
        public class When_change_outgo_type :SpecsFor<Budget>
        {
            private readonly OutgoId outgoId = OutgoId.From(Guid.NewGuid());
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()),BudgetName.From("name"));
            }
            protected override void Given()
            {
                Given(new OutgoExist(outgoId, EntryTime.From(new DateTime(2022,4,4,12,0,0))));
                base.Given();
            }
            protected override void When()
            {
                SUT.ChangeOutgoType(outgoId, OutgoType.Unexpected, It.IsAny<EntryTime>());
            }
            [Test]
            public void Then_outgo_type_changed_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.OutgoTypeChanged).ShouldBeTrue();
            }
            [Test]
            public void Then_outgo_type_should_be_changed() 
            {
                SUT.Outgos.First().Type.ShouldEqual(OutgoType.Unexpected);
            }
        }
        public class When_add_outgo_given_is_monthly_type_and_budget_period_span_encompas_two_outgos : SpecsFor<Budget>
        {
            private readonly BudgetId budgetId = BudgetId.From(Guid.NewGuid());
            private readonly EntryTime entryTime = EntryTime.From(new DateTime(2022, 4, 4, 12, 0, 0));
            private readonly PeriodId periodId = PeriodId.From(Guid.NewGuid());
            protected override void InitializeClassUnderTest()
            {
                var events = new object[]
                {
                    new Events.BudgetCreated(budgetId,"budget", entryTime),
                    new Events.PeriodCreated(budgetId, periodId,entryTime,entryTime.AddMonths(12),PeriodState.Cyclic.Name,PeriodType.Month.Name,entryTime ),
                    new Events.PeriodStopChanged(budgetId, periodId, entryTime.AddMonths(1).AddDays(2), entryTime),
                    new Events.IncomeAddedToBudget(budgetId, Guid.NewGuid(),1000m, IncomeType.Gift.Name,entryTime)
                };
                SUT = new Budget(events);
            }
            protected override void When()
            {
                SUT.AddOutgo(OutgoId.From(Guid.NewGuid()), Money.From(100m), OutgoType.Monthly, entryTime.AddHours(2));
            }
            [Test]
            public void Then_amount_left_changed_should_calculate_with_two_outgos() 
            {
                GetAmountLeftChangedEvent(SUT)?.AmountLeft.ShouldEqual(800);
            }
            [Test]
            public void Then_amount_available_changed_should_calculate_with_two_outgos() 
            {
                var expected = (1000 - 200) / SUT.Period.TotalDays;
                GetAmountAvailableChangedEvent(SUT)?.AmountAvailable.ShouldEqual(expected);
            }
            [Test]
            public void Then_amount_limit_should_calculate_with_two_outgos() 
            {
                var amountLimit = Money.From(Math.Round(((decimal)(1000 - 200)) / SUT.Period.TotalDays));
                var expected = Money.From((SUT.Period.ElapsedDaysFromStart(ThresholdTime.From(entryTime.AddHours(2))) * amountLimit) - 0);
                GetAmountLimitChangedEvent(SUT)?.AmountLimit.ShouldEqual(expected);
            }
        }

        public class When_add_outgo_given_is_weekly_type_and_budget_period_span_encompas_three_outgos : SpecsFor<Budget>
        {
            private readonly BudgetId budgetId = BudgetId.From(Guid.NewGuid());
            private readonly EntryTime entryTime = EntryTime.From(new DateTime(2022, 4, 4, 12, 0, 0));
            private readonly PeriodId periodId = PeriodId.From(Guid.NewGuid());
            protected override void InitializeClassUnderTest()
            {
                var events = new object[]
                {
                    new Events.BudgetCreated(budgetId,"budget", entryTime),
                    new Events.PeriodCreated(budgetId, periodId,entryTime,entryTime.AddDays(7*3),PeriodState.Cyclic.Name,PeriodType.Month.Name,entryTime ),
                    new Events.IncomeAddedToBudget(budgetId, Guid.NewGuid(),1000m, IncomeType.Gift.Name,entryTime)
                };
                SUT = new Budget(events);
            }
            protected override void When()
            {
                SUT.AddOutgo(OutgoId.From(Guid.NewGuid()), Money.From(50m), OutgoType.Weekly, entryTime.AddHours(2));
            }
            [Test]
            public void Then_three_outgos_should_be_calculated_to_total() 
            {
                GetAmountLeftChangedEvent(SUT)?.AmountLeft.ShouldEqual(850m);
            }
        }

        public class When_add_outgo_given_is_half_yearly_type_and_budget_period_span_encompas_two_outgos : SpecsFor<Budget> 
        {
            private readonly BudgetId budgetId = BudgetId.From(Guid.NewGuid());
            private readonly EntryTime entryTime = EntryTime.From(new DateTime(2022, 4, 4, 12, 0, 0));
            private readonly PeriodId periodId = PeriodId.From(Guid.NewGuid());
            protected override void InitializeClassUnderTest()
            {
                var events = new object[]
               {
                    new Events.BudgetCreated(budgetId,"budget", entryTime),
                    new Events.PeriodCreated(budgetId, periodId,entryTime,entryTime.AddMonths(12),PeriodState.Cyclic.Name,PeriodType.Year.Name,entryTime ),
                    new Events.IncomeAddedToBudget(budgetId, Guid.NewGuid(),10000m, IncomeType.Gift.Name,entryTime)
               };
                SUT = new Budget(events);
            }
            protected override void When()
            {
                SUT.AddOutgo(OutgoId.From(Guid.NewGuid()), Money.From(500m), OutgoType.HalfYearly, entryTime.AddHours(2));
            }
            [Test]
            public void Then_two_outgos_should_be_calculated_to_total() 
            {
                GetAmountLeftChangedEvent(SUT)?.AmountLeft.ShouldEqual(9000m);
            }
        }
        public class When_add_outgo_given_is_yearly_type_and_budget_period_span_encompas_two_outgos : SpecsFor<Budget>
        {
            private readonly BudgetId budgetId = BudgetId.From(Guid.NewGuid());
            private readonly EntryTime entryTime = EntryTime.From(new DateTime(2022, 4, 4, 12, 0, 0));
            private readonly PeriodId periodId = PeriodId.From(Guid.NewGuid());
            protected override void InitializeClassUnderTest()
            {
                var events = new object[]
              {
                    new Events.BudgetCreated(budgetId,"budget", entryTime),
                    new Events.PeriodCreated(budgetId, periodId,entryTime,entryTime.AddMonths(24),PeriodState.Cyclic.Name,PeriodType.Year.Name,entryTime ),
                    new Events.IncomeAddedToBudget(budgetId, Guid.NewGuid(),10000m, IncomeType.Gift.Name,entryTime)
              };
                SUT = new Budget(events);
            }
            protected override void When()
            {
                SUT.AddOutgo(OutgoId.From(Guid.NewGuid()), Money.From(1200m), OutgoType.Yearly, entryTime.AddHours(2));
            }
            [Test]
            public void Then_two_outgos_should_be_calculated_to_total()
            {
                GetAmountLeftChangedEvent(SUT)?.AmountLeft.ShouldEqual(7600m);
            }
        }

        public class When_add_outgo_given_is_yearly_type_and_budget_period_start_is_afted_outgo : SpecsFor<Budget>
        {
            private readonly BudgetId budgetId = BudgetId.From(Guid.NewGuid());
            private readonly EntryTime entryTime = EntryTime.From(new DateTime(2022, 4, 4, 12, 0, 0));
            private readonly PeriodId periodId = PeriodId.From(Guid.NewGuid());
            protected override void InitializeClassUnderTest()
            {
                var events = new object[]
              {
                    new Events.BudgetCreated(budgetId,"budget", entryTime),
                    new Events.PeriodCreated(budgetId, periodId,entryTime,entryTime.AddMonths(24),PeriodState.Cyclic.Name,PeriodType.Year.Name,entryTime ),
                    new Events.IncomeAddedToBudget(budgetId, Guid.NewGuid(),10000m, IncomeType.Gift.Name,entryTime)
              };
                SUT = new Budget(events);
            }
            protected override void When()
            {
                SUT.AddOutgo(OutgoId.From(Guid.NewGuid()), Money.From(1200m), OutgoType.Yearly, entryTime.AddHours(-24));
            }
            [Test]
            public void Then_two_outgos_should_be_calculated_to_total()
            {
                GetAmountLeftChangedEvent(SUT)?.AmountLeft.ShouldEqual(10000m);
            }
        }


        public class When_change_outgo_amount_given_outgo_does_not_exist : SpecsFor<Budget>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()),BudgetName.From("name"));
            }
            [Test]
            public void Then_invalid_operation_exception_should_be_thrown_when_change_amount() 
            {
                Assert.Throws<InvalidOperationException>(() => SUT.ChangeOutgoAmount(It.IsAny<OutgoId>(), It.IsAny<Money>(), It.IsAny<EntryTime>()));
            }
            [Test]
            public void Then_invalid_operation_exception_should_be_thrown_when_change_type() 
            {
                Assert.Throws<InvalidOperationException>(() => SUT.ChangeOutgoType(It.IsAny<OutgoId>(), It.IsAny<OutgoType>(), It.IsAny<EntryTime>()));
            }
        }

        private static Events.AmountLeftChanged? GetAmountLeftChangedEvent(Budget b)
            => b.GetChanges().FirstOrDefault(x => x is Events.AmountLeftChanged) as Events.AmountLeftChanged;
        private static Events.AmountAvailableChanged? GetAmountAvailableChangedEvent(Budget b)
            => b.GetChanges().FirstOrDefault(x => x is Events.AmountAvailableChanged) as Events.AmountAvailableChanged;
        private static Events.AmountLimitChanged? GetAmountLimitChangedEvent(Budget b)
            => b.GetChanges().FirstOrDefault(x => x is Events.AmountLimitChanged) as Events.AmountLimitChanged;
        private class OutgoExist : IContext<Budget>
        {
            private readonly OutgoId outgoId;
            private readonly EntryTime entryTime;

            public OutgoExist(OutgoId outgoId,EntryTime entryTime)
            {
                this.outgoId = outgoId;
                this.entryTime = entryTime;
            }
            public void Initialize(ISpecs<Budget> state)
            {
                state.SUT.AddOutgo(outgoId, Money.From(100), Domain.Enumerations.OutgoType.Monthly, entryTime);
            }
        }
    }
}
