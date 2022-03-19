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
using UseLess.Domain.Values;
using UseLess.Messages;

namespace UseLess.Domain.Tests.Budgets
{
    internal class DeleteExpenseSpecs
    {
        private static readonly ExpenseId expenseId = ExpenseId.From(Guid.NewGuid());
        public class When_delete_expense : SpecsFor<Budget>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()), BudgetName.From("name"));
            }
            protected override void Given()
            {
                Given<ExpenseExist>();
                base.Given();
            }
            protected override void When()
            {
                SUT.DeleteExpense(expenseId, It.IsAny<EntryTime>());
            }
            [Test]
            public void Then_expense_deleted_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.ExpenseDeleted).ShouldBeTrue();
            }
            [Test]
            public void Then_expense_should_be_removed() 
            {
                SUT.Expenses.Any(x => x.Id == expenseId).ShouldBeFalse();
            }
        }

        private class ExpenseExist : IContext<Budget>
        {
            public void Initialize(ISpecs<Budget> state)
            {
                state.SUT.AddExpense(expenseId, Money.From(100m), EntryTime.From(DateTime.Now));
            }
        }
    }
}
