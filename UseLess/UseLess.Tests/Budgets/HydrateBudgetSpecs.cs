using NUnit.Framework;
using Should;
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
    internal class HydrateBudgetSpecs
    {
        private static BudgetId budgetId = BudgetId.From(Guid.NewGuid());
        private static DateTime dateTime = new DateTime(2022, 3, 1, 12, 0, 0);
        private static IncomeId incomeId = IncomeId.From(Guid.NewGuid());
        private static OutgoId outgoId = OutgoId.From(Guid.NewGuid());
        private static ExpenseId expenseId = ExpenseId.From(Guid.NewGuid());
        private static IEnumerable<object> events = new object[]
        {
            new Events.BudgetCreated(budgetId,"budgetName",BudgetState.Active.Name, dateTime.Date),
            new Events.IncomeAddedToBudget(budgetId,incomeId, 5000m, "GIFT", dateTime.AddHours(1)),
            new Events.OutgoAddedToBudget(budgetId,outgoId, 1500m, "ONCE",dateTime.AddHours(2)),
            new Events.ExpenseAddedToBudget(budgetId,expenseId, 250m,dateTime.AddHours(3))
        };
        public class When_hydrate_budget_from_events : SpecsFor<Budget> 
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = new Budget(events);
            }
            [Test]
            public void Then_budget_name_should_be_set() 
            {
                SUT.Name.ShouldEqual(BudgetName.From("budgetName"));
            }
            [Test]
            public void Then_income_should_be_set() 
            {
                SUT.Incomes.ShouldNotBeEmpty();
            }
            [Test]
            public void Then_outgos_should_not_be_empty() 
            {
                SUT.Outgos.ShouldNotBeEmpty();
            }
            [Test]
            public void Then_expenses_should_not_be_empty() 
            {
                SUT.Expenses.ShouldNotBeEmpty();
            }
        }
        public class When_hydrate_budget_from_events_given_changes_was_made : SpecsFor<Budget> 
        {
            protected override void InitializeClassUnderTest()
            {
                var changedEvents = new object[]
                {
                    new Events.IncomeAmountChanged(budgetId,incomeId, 10000m, 5000m, dateTime.AddDays(1)),
                    new Events.IncomeTypeChanged(budgetId,incomeId, "PERKS", dateTime.AddDays(1)),
                    new Events.OutgoAmountChanged(budgetId,outgoId, 3400m,3000m, dateTime.AddDays(2)),
                    new Events.OutgoTypeChanged(budgetId,outgoId, "WEEKLY", dateTime.AddDays(2)),
                    new Events.ExpenseAmountChanged(budgetId,expenseId,244m, 200m, dateTime.AddDays(3))
                };
                var e = events.ToList();
                e.AddRange(changedEvents.ToList());
                SUT = new Budget(e);
            }
            [Test]
            public void Then_income_type_should_be_changed() 
            {
                SUT.Incomes.First().Type.ShouldEqual(IncomeType.Perks);
            }
            [Test]
            public void Then_income_amount_should_be_changed() 
            {
                SUT.Incomes.First().Amount.ShouldEqual(Money.From(10000m));
            }
            [Test]
            public void Then_outgo_amount_should_be_changed() 
            {
                SUT.Outgos.First().Amount.ShouldEqual(Money.From(3400m));
            }
            [Test]
            public void Then_outgo_type_should_be_changed() 
            {
                SUT.Outgos.First().Type.ShouldEqual(OutgoType.Weekly);
            }
            [Test]
            public void Then_expense_amount_should_be_changed() 
            {
                SUT.Expenses.First().Amount.ShouldEqual(Money.From(244m));
            }
        }

        public class When_add_change_to_hydrated_budget : SpecsFor<Budget>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = new Budget(events);
            }
            protected override void When()
            {
                SUT.ChangeIncomeAmount(incomeId, Money.From(10500m), EntryTime.From(dateTime.AddDays(1)));
            }
            [Test]
            public void Then_changes_should_be_applied() 
            {
                SUT.GetChanges().Count().ShouldEqual(2);
            }
        }
    }
}
