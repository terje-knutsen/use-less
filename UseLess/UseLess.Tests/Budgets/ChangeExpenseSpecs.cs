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
using UseLess.Domain.Values;
using UseLess.Messages;

namespace UseLess.Tests.Budgets
{
    internal class ChangeExpenseSpecs
    {
        public class When_change_expense_amount : SpecsFor<Budget>
        {
            private readonly ExpenseId expenseId = ExpenseId.From(Guid.NewGuid());
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()),BudgetName.From("name"));
            }
            protected override void Given()
            {
                Given(new ExpenseExist(expenseId));
                base.Given();
            }
            protected override void When()
            {
                SUT.ChangeExpenseAmount(expenseId, Money.From(122), It.IsAny<EntryTime>());
            }
            [Test]
            public void Then_expense_amount_changed_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.ExpenseAmountChanged).ShouldBeTrue();
            }
            [Test]
            public void Then_expense_amount_should_be_changed() 
            {
                SUT.Expenses.First().Amount.ShouldEqual(Money.From(122));
            }
        }
        public class When_change_expense_amount_given_expense_does_not_exist : SpecsFor<Budget> 
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()),BudgetName.From("name"));
            }
            [Test]
            public void Then_invalid_operation_exception_should_be_thrown() 
            {
                Assert.Throws<InvalidOperationException>(() => SUT.ChangeExpenseAmount(It.IsAny<ExpenseId>(), It.IsAny<Money>(), It.IsAny<EntryTime>())); ;
            }
        }
        private class ExpenseExist : IContext<Budget>
        {
            private readonly ExpenseId expenseId;

            public ExpenseExist(ExpenseId expenseId)
            {
                this.expenseId = expenseId;
            }
            public void Initialize(ISpecs<Budget> state)
            {
                state.SUT.AddExpense(expenseId, Money.From(255), It.IsAny<EntryTime>());
            }
        }
    }
}
