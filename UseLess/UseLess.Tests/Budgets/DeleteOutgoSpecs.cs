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
using UseLess.Domain.Enumerations;
using UseLess.Domain.Values;
using UseLess.Messages;

namespace UseLess.Domain.Tests.Budgets
{
    internal class DeleteOutgoSpecs
    {
        private static readonly OutgoId outgoId = OutgoId.From(Guid.NewGuid());
        public class When_delete_outgo : SpecsFor<Budget>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()), BudgetName.From("name"));
            }
            protected override void Given()
            {
                Given<OutgoExist>();
                base.Given();
            }
            protected override void When()
            {
                SUT.DeleteOutgo(outgoId, It.IsAny<EntryTime>());
            }
            [Test]
            public void Then_outgo_deleted_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.OutgoDeleted).ShouldBeTrue();
            }
            [Test]
            public void Then_outgo_should_be_removed() 
            {
                SUT.Outgos.Any(x => x.Id == outgoId).ShouldBeFalse();
            }
        }
        private class OutgoExist : IContext<Budget>
        {
            public void Initialize(ISpecs<Budget> state)
            {
                state.SUT.AddOutgo(outgoId, Money.From(199m), OutgoType.Monthly, EntryTime.From(DateTime.Now));
            }
        }
    }
}
