using NUnit.Framework;
using Should;
using SpecsFor.StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Domain;
using UseLess.Domain.Tests.Budgets;
using UseLess.Domain.Values;
using UseLess.Messages;

namespace UseLess.Tests.Budgets
{
    internal class AddExpenseSpecs
    {
        public class when_add_expense : SpecsFor<Budget>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()),BudgetName.From("name"));
            }
            protected override void When()
            {
                SUT.AddExpense(ExpenseId.From(Guid.NewGuid()), Money.From(4), EntryTime.From(new DateTime(2022,2,12)));
            }
            [Test]
            public void Then_expense_event_added_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.ExpenseAddedToBudget).ShouldBeTrue();
            }
            [Test]
            public void Then_expense_should_be_set() 
            {
                SUT.Expenses.Any().ShouldBeTrue();
            }
            [Test]
            public void Then_properties_should_be_set() 
            {
                SUT.Expenses.First().Id.ShouldNotBeNull();
                (SUT.Expenses.First().Amount == Money.From(4)).ShouldBeTrue();
                SUT.Expenses.First().EntryTime.ShouldEqual(EntryTime.From(new DateTime(2022, 2, 12)));
            }
        }
        public class When_add_expense_given_period_is_set : SpecsFor<Budget>
        {
            private readonly StartTime startTime = StartTime.From(new DateTime(2022, 4, 10, 12, 0, 0));
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()), BudgetName.From("name"));
            }
            protected override void Given()
            {
                Given(new CommonContext.PeriodIsSet(startTime));
                base.Given();
            }
            protected override void When()
            {
                SUT.AddExpense(ExpenseId.From(Guid.NewGuid()), Money.From(20m), EntryTime.From(((DateTime)startTime).AddDays(3)));
            }
            [Test]
            public void Then_amounts_should_be_recalculated() 
            {
                SUT.GetChanges().Any(x => x is Events.AmountLeftChanged).ShouldBeTrue();
            }
        }
    }
}
