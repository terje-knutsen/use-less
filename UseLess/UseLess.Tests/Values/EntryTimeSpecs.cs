using NUnit.Framework;
using Should;
using SpecsFor.StructureMap;
using UseLess.Domain.Values;

namespace UseLess.Tests.Values
{
    internal class EntryTimeSpecs
    {
        public class When_compare_entry_time : SpecsFor<object>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = EntryTime.From(new DateTime(2022, 2, 12, 12, 0, 0));
            }
            [Test]
            public void Then_equality_should_be_correct() 
            {
                SUT.Equals(EntryTime.From(new DateTime(2022, 2, 12, 12, 0, 0))).ShouldBeTrue();
                ((EntryTime)SUT == EntryTime.From(new DateTime(2022,2,12,12,0,0))).ShouldBeTrue();
                ((EntryTime)SUT == new DateTime(2022, 2, 12, 12, 0,0)).ShouldBeTrue();
                ((EntryTime)SUT != new DateTime(2022, 2, 12, 12, 0, 1)).ShouldBeTrue();
            }
        }
    }
}
