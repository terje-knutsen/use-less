﻿using Moq;
using NUnit.Framework;
using Should;
using SpecsFor.StructureMap;
using UseLess.Domain;
using UseLess.Domain.Enumerations;
using UseLess.Domain.Values;
using UseLess.Messages;

namespace UseLess.Tests.Budgets
{
    internal class CreateSpecs
    {
        public class When_create : SpecsFor<Budget>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()),BudgetName.From("budget name"));
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
            [Test]
            public void Then_budget_state_should_be_active() 
            {
                SUT.State.ShouldEqual(BudgetState.Active);
            }
        }
        public class When_create_given_budget_name_is_empty : SpecsFor<object>
        {
            [Test]
            public void Then_domain_exception_should_be_thrown()
            {
                Assert.Throws<Exceptions.BudgetNameException>(() => Budget.Create(BudgetId.From(Guid.NewGuid()),BudgetName.From("")));
            }
        }
        public class When_create_given_budget_name_length_exceeds_allowed_length : SpecsFor<object>
        {
            [Test]
            public void Then_domain_exception_should_be_thrown() 
            {
                Assert.Throws<Exceptions.BudgetNameException>(() => Budget.Create(BudgetId.From(Guid.NewGuid()),BudgetName.From(new string('a', 46))));
            }
        }
        public class When_create_given_budget_name_includes_leading_and_trailing_white_spaces : SpecsFor<Budget>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()),BudgetName.From("       new name         "));
            }
            [Test]
            public void Then_all_leading_and_trailing_white_spaces_should_be_removed() 
            {
                SUT.Name.ShouldEqual(BudgetName.From("new name"));
            }
        }
    }
}
