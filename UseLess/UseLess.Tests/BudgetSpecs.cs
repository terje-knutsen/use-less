﻿using Moq;
using NUnit.Framework;
using Should;
using SpecsFor.StructureMap;
using UseLess.Domain;
using UseLess.Domain.Enumerations;
using UseLess.Domain.Values;
using UseLess.Messages;
using static UseLess.Messages.Exceptions;

namespace UseLess.Tests
{
    internal class BudgetSpecs
    {
        public class When_budget_created : SpecsFor<Budget>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetName.From("budget name"));
            }
            [Test]
            public void Then_budget_created_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.BudgetCreated).ShouldBeTrue();
            }
            [Test]
            public void Then_budget_id_should_be_set() 
            {
                SUT.Id.ShouldNotBeNull();
            }
            [Test]
            public void Then_budget_name_should_be_set() 
            {
                SUT.Name.ShouldNotBeNull();
            }
        }
        public class When_add_income_given_budget_is_not_valid : SpecsFor<Budget>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = new Budget();
                SUT.Load(new[] { new Events.IncomeAdded(It.IsAny<Guid>(), It.IsAny<decimal>(), It.IsAny<string>()) });
            }
            [Test]
            public void Then_exception_should_be_thrown() 
            {
                Assert.Throws(typeof(InvalidStateException),()=> SUT.AddIncome(Money.From(23m),IncomeType.Bonus));
            }
        }
        public class When_add_income_given_budget_is_valid : SpecsFor<Budget>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetName.From("budget name"));
            }
            protected override void When()
            {
                SUT.AddIncome(Money.From(32), IncomeType.Bonus);
            }
            [Test]
            public void Then_income_added_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.IncomeAdded).ShouldBeTrue();
            }
            [Test]
            public void Then_income_should_be_set() 
            {
                SUT.Incomes.First().ShouldEqual(Money.From(32));
            }
        }
    }
}