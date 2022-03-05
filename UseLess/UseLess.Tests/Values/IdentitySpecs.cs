using NUnit.Framework;
using Should;
using SpecsFor.StructureMap;
using UseLess.Domain.Values;

namespace UseLess.Tests.Values
{
    internal class IdentitySpecs
    {
        public class When_compare_identity : SpecsFor<object>
        {
            private readonly Guid guid = Guid.NewGuid();
            protected override void InitializeClassUnderTest()
            {
                SUT = BudgetId.From(guid);
            }
            [Test]
            public void Then_equality_should_be_correct()
            {
                SUT.Equals(BudgetId.From(guid)).ShouldBeTrue();
                ((BudgetId)SUT == guid).ShouldBeTrue();
                ((BudgetId)SUT == BudgetId.From(guid)).ShouldBeTrue();
            } 
            [Test]
            public void Then_in_equality_should_be_correct() 
            {
                SUT.Equals(BudgetId.From(Guid.NewGuid())).ShouldBeFalse();
                ((BudgetId)SUT != guid).ShouldBeFalse();
                ((BudgetId)SUT != BudgetId.From(Guid.NewGuid())).ShouldBeTrue();
            }
        }
    }
}
