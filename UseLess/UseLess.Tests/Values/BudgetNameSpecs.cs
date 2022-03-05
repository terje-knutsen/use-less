using NUnit.Framework;
using Should;
using SpecsFor.StructureMap;
using UseLess.Domain.Values;

namespace UseLess.Tests.Values
{
    internal class BudgetNameSpecs
    {
        public class When_compare_names: SpecsFor<object>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = BudgetName.From("BudgetName");
            }
            [Test]
            public void Then_equal_should_be_correct() 
            {
                SUT.Equals(BudgetName.From("BudgetName")).ShouldBeTrue();
                ((BudgetName)SUT == "BudgetName").ShouldBeTrue();
                ((BudgetName)SUT == BudgetName.From("BudgetName")).ShouldBeTrue();
            }
        }
    }
}
