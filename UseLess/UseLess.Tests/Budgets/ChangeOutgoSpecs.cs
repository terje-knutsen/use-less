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
using UseLess.Domain.Values;
using UseLess.Messages;

namespace UseLess.Tests.Budgets
{
    internal class ChangeOutgoSpecs
    {
        public class When_change_outgo_amount : SpecsFor<Budget>
        {
            private readonly OutgoId outgoId = OutgoId.From(Guid.NewGuid());
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()),BudgetName.From("name"));
            }
            protected override void Given()
            {
                Given(new OutgoExist(outgoId));
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
                Given(new OutgoExist(outgoId));
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
        private class OutgoExist : IContext<Budget>
        {
            private readonly OutgoId outgoId;

            public OutgoExist(OutgoId outgoId)
            {
                this.outgoId = outgoId;
            }
            public void Initialize(ISpecs<Budget> state)
            {
                state.SUT.AddOutgo(outgoId, Money.From(100), Domain.Enumerations.OutgoType.Monthly, It.IsAny<EntryTime>());
            }
        }
    }
}
