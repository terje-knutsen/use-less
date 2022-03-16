using Moq;
using Nito.AsyncEx;
using NUnit.Framework;
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
using UseLess.Services;
using UseLess.Services.Api;
using static UseLess.Messages.BudgetCommands;

namespace Useless.Services.Tests
{
    internal class BudgetServiceSpecs
    {
        private static BudgetId budgetId = BudgetId.From(Guid.NewGuid());
        public class When_create_budget: SpecsFor<BudgetService>
        {
            protected override void When()
            {
                AsyncContext.Run(async () => await SUT.Handle(new V1.Create { BudgetId = Guid.NewGuid(), Name = "test" }));
            }
            [Test]
            public void Then_budget_should_be_saved() 
            {
                GetMockFor<IAggregateStore>().Verify(x => x.Save<Budget,BudgetId>(It.IsAny<Budget>()));
            }
        }
        public class When_create_budget_given_already_exist : SpecsFor<BudgetService>
        {
            protected override void Given()
            {
                Given<BudgetExist>();
                base.Given();
            }
            [Test]
            public void Then_invalid_operation_exception_should_be_thrown()
            {
                Assert.Throws<InvalidOperationException>(()=> AsyncContext.Run(async () => await SUT.Handle(new V1.Create { BudgetId = Guid.NewGuid(), Name = "name" })));
            }
        }
        public class When_add_income : SpecsFor<BudgetService> 
        {
            protected override void Given()
            {
                Given<BudgetExist>();
                base.Given();
            }
            protected override void When()
            {
                AsyncContext.Run(async () => await SUT.Handle(new V1.AddIncome { BudgetId = budgetId, IncomeId = Guid.NewGuid(), Amount = 100m, Type = IncomeType.Bonus.Name }));
            }
            [Test]
            public void Then_budget_should_be_updated() 
            {
                GetMockFor<IAggregateStore>().Verify(x => x.Save<Budget, BudgetId>(It.IsAny<Budget>()));
            }
        }
        public class When_handle_update_commands : SpecsFor<BudgetService>
        {
            protected override void Given()
            {
                Given<BudgetExist>();
                base.Given();
            }
            
            
            [TestCaseSource(nameof(CommandCases))]
            public void Then_command_should_be_handled(object cmd) 
            {
                AsyncContext.Run(async () => await SUT.Handle(cmd));
                GetMockFor<IAggregateStore>()
                    .Verify(x => x.Save<Budget, BudgetId>(It.IsAny<Budget>()));
            }
            static object[] CommandCases =
                {
                new object[]{new V1.AddIncome { BudgetId = budgetId, Amount = 100m, IncomeId = Guid.NewGuid(),Type = IncomeType.Perks.Name} },
                new object[]{new V1.AddOutgo {BudgetId = budgetId, Amount = 55m, OutgoId = Guid.NewGuid(), Type = OutgoType.Once.Name } },
                new object[]{new V1.AddExpense { BudgetId = budgetId, Amount = 33m, ExpenseId = Guid.NewGuid() }, },
                new object[]{new V1.AddPeriod { BudgetId = budgetId, PeriodId = Guid.NewGuid(),StartTime = new DateTime(2022,3,10,12,0,0) } }
                };
        }
        private class BudgetExist : IContext<BudgetService>
        {
            private readonly Budget budget;
            public BudgetExist():this(Budget.Create(budgetId,BudgetName.From("name")))
            {}
            public BudgetExist(Budget budget) 
            {
                this.budget = budget;
            }
            public void Initialize(ISpecs<BudgetService> state)
            {
                state.GetMockFor<IAggregateStore>().Setup(x => x.Exists<Budget,BudgetId>(It.IsAny<BudgetId>()))
                    .Returns(Task.FromResult(true));
                state.GetMockFor<IAggregateStore>().Setup(x => x.Load<Budget, BudgetId>(It.IsAny<BudgetId>()))
                    .Returns(Task.FromResult(budget));
            }
        }
    }
}
