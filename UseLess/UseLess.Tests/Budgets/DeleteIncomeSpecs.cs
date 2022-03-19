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
    internal class DeleteIncomeSpecs
    {
        private static IncomeId incomeId = IncomeId.From(Guid.NewGuid());
        public class When_delete_income : SpecsFor<Budget>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = Budget.Create(BudgetId.From(Guid.NewGuid()), BudgetName.From("name"));
            }
            protected override void Given()
            {
                Given<IncomeExist>();
                base.Given();
            }
            protected override void When() => SUT.DeleteIncome(incomeId, It.IsAny<EntryTime>());
            [Test]
            public void Then_income_removed_event_should_be_applied() 
            {
                SUT.GetChanges().Any(x => x is Events.IncomeDeleted).ShouldBeTrue();
            }
            [Test]
            public void Then_income_should_be_removed() 
            {
                SUT.Incomes.Any(x => x.Id == incomeId).ShouldBeFalse();
            }

        }

        private class IncomeExist : IContext<Budget>
        {
            public void Initialize(ISpecs<Budget> state)
            {
                state.SUT.AddIncome(incomeId, Money.From(200m),Enumerations.IncomeType.Gambling, EntryTime.From(DateTime.Now));
            }
        }
    }
}
