using NUnit.Framework;
using Should;
using SpecsFor.StructureMap;
using UseLess.Domain.Values;

namespace UseLess.Tests.Values
{
    internal class MoneySpecs
    {
        public class When_create : SpecsFor<object>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Money.From(100);
            }
            [Test]
            public void Then_equality_should_be_set() 
            {
                SUT.Equals(Money.From(100)).ShouldBeTrue();
                ((Money)SUT == Money.From(100.0m)).ShouldBeTrue();
                ((Money)SUT != Money.From(100)).ShouldBeFalse();
            }
            [Test]
            public void Then_compare_less_should_be_correct() 
            {
                ((Money)SUT < Money.From(100.01m)).ShouldBeTrue();
            }
            [Test]
            public void Then_compare_greater_should_be_correct() 
            {
                ((Money)SUT > Money.From(99.99m)).ShouldBeTrue();
            }
            [Test]
            public void Then_less_than_or_equal_should_be_correct() 
            {
                ((Money)SUT <= Money.From(100.1m)).ShouldBeTrue();
                ((Money)SUT <= Money.From(100.0m)).ShouldBeTrue();
                ((Money)SUT <= Money.From(99.99m)).ShouldBeFalse();
            }
            [Test]
            public void Then_greater_than_or_equal_should_be_correct() 
            {
                ((Money)SUT >= Money.From(99.99m)).ShouldBeTrue();
                ((Money)SUT >= Money.From(100.0m)).ShouldBeTrue();
            }

        }
    }
}
