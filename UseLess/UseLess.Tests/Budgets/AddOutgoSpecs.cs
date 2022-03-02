using NUnit.Framework;
using Should;
using SpecsFor.StructureMap;
using UseLess.Domain;
using UseLess.Domain.Enumerations;
using UseLess.Domain.Values;
using UseLess.Messages;

namespace UseLess.Tests.Budgets
{
    internal class AddOutgoSpecs
    {
        public class When_add_outgo : SpecsFor<Budget> 
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetName.From("a budget"));
            }
            protected override void When()
            {
                SUT.AddOutgo(OutgoId.From(Guid.NewGuid()), Money.From(22), OutgoType.Unexpected);
            }
            [Test]
            public void Then_outgo_added_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.OutgoAdded).ShouldBeTrue();
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
        }
    }
}
