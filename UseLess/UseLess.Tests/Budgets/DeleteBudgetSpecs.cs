using Moq;
using NUnit.Framework;
using Should;
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
    internal class DeleteBudgetSpecs
    {
        public class When_delete_budget:SpecsFor<Budget>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()), BudgetName.From("name"));
            }
            protected override void When()
            {
                SUT.Delete(It.IsAny<EntryTime>());
            }
            [Test]
            public void Then_budget_deleted_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.BudgetDeleted).ShouldBeTrue();
            }
            [Test]
            public void Then_budget_state_should_be_deleted() 
            {
                SUT.State.ShouldEqual(BudgetState.Deleted);
            }
        }
    }
}
