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

        public class When_delete_expense_given_period_is_set : SpecsFor<Budget> 
        {
            private DateTime now = new DateTime(2022, 4, 20, 12, 0, 0);
            private readonly Guid budgetIdGuid = Guid.NewGuid();
            private readonly Guid expenseIdGuid = Guid.NewGuid();
            protected override void InitializeClassUnderTest()
            {
                var events = new object[] 
                {
                    new Events.BudgetCreated(budgetIdGuid,"name",BudgetState.Active.Name, now),
                    new Events.PeriodCreated(budgetIdGuid, Guid.NewGuid(),now.AddMinutes(1),now.AddMonths(1),PeriodState.Cyclic.Name,PeriodType.Month.Name,now),
                    new Events.ExpenseAddedToBudget(budgetIdGuid,expenseIdGuid,44m,now.AddHours(1)),
                    new Events.AmountLeftChanged(budgetIdGuid, 44m, now)
                };
                SUT = new Budget(events);
            }
            protected override void When()
            {
                SUT.DeleteExpense(ExpenseId.From(expenseIdGuid), EntryTime.From(now.AddDays(2)));
            }
            [Test]
            public void Then_budget_details_should_be_recalculated() 
            {
                SUT.GetChanges().Any(x => x is Events.AmountLeftChanged).ShouldBeTrue();
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
