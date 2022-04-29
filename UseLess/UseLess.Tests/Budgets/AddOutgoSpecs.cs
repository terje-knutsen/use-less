using NUnit.Framework;
using Should;
using SpecsFor.Core;
using SpecsFor.StructureMap;
using UseLess.Domain;
using UseLess.Domain.Enumerations;
using UseLess.Domain.Tests.Budgets;
using UseLess.Domain.Values;
using UseLess.Messages;
using static UseLess.Messages.Exceptions;

namespace UseLess.Tests.Budgets
{
    internal class AddOutgoSpecs
    {
        public class When_add_outgo : SpecsFor<Budget> 
        {
            private readonly EntryTime entryTime = EntryTime.From(new DateTime(2022, 2, 2));
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()),BudgetName.From("a budget"));
            }
            protected override void When()
            {
                SUT.AddOutgo(OutgoId.From(Guid.NewGuid()), Money.From(22), OutgoType.Unexpected, entryTime);
            }
            [Test]
            public void Then_outgo_added_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.OutgoAddedToBudget).ShouldBeTrue();
            }
            [Test]
            public void Then_outgo_amount_should_be_set() 
            {
                SUT.Outgos.First().Amount.ShouldEqual(Money.From(22));
            }
            [Test]
            public void Then_outgo_type_should_be_set() 
            {
                SUT.Outgos.First().Type.ShouldEqual(OutgoType.Unexpected);
            }
            [Test]
            public void Then_outgo_id_should_be_set() 
            {
                SUT.Outgos.First().Id.ShouldNotEqual(default);
            }
            [Test]
            public void Then_outgo_entry_time_should_be_set() 
            {
                SUT.Outgos.First().EntryTime.ShouldEqual(entryTime);
            }
        }

        public class When_add_outgo_given_outgo_already_added : SpecsFor<Budget>
        {
            private readonly OutgoId outgoId = OutgoId.From(Guid.NewGuid());
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()), BudgetName.From("a budget"));
            }
            protected override void Given()
            {
                Given(new OutgoExist(outgoId));
                base.Given();
            }
            [Test]
            public void Then_add_outgo_a_second_time_should_throw_exception()
            {
                Assert.Throws<OutgoAlreadyExistException>(() => SUT.AddOutgo(outgoId, Money.From(100), OutgoType.Unexpected, EntryTime.Now));
            }
        }
        public class When_add_outgo_given_period_is_set : SpecsFor<Budget>
        {
            private readonly StartTime startTime = StartTime.From(new DateTime(2022, 4, 10, 12, 0, 0));
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()), BudgetName.From("budget"));
            }
            protected override void Given()
            {
                Given(new CommonContext.PeriodIsSet(startTime));
                base.Given();
            }
            protected override void When()
            {
                SUT.AddOutgo(OutgoId.From(Guid.NewGuid()), Money.From(100), OutgoType.Unexpected, EntryTime.From(((DateTime)startTime).AddDays(2)));
            }
            [Test]
            public void Then_amount_available_changed_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.AmountAvailableChanged).ShouldBeTrue();
            }
        }

        private class OutgoExist : IContext<Budget>
        {
            private readonly OutgoId outgoId;

            public OutgoExist(OutgoId outgoId)
            {
                this.outgoId = outgoId;
            }
         
            public void Initialize(ISpecs<Budget> state)
            {
                state.SUT.AddOutgo(outgoId, Money.From(100), OutgoType.Unexpected, EntryTime.Now);
            }
        }
    }
}
