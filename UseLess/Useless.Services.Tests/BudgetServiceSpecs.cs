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
using UseLess.Services.Budgets;
using static UseLess.Messages.BudgetCommands;

namespace Useless.Services.Tests
{
    internal class BudgetServiceSpecs
    {
        private static BudgetId budgetId = BudgetId.From(Guid.NewGuid());
        private static IncomeId incomeId = IncomeId.From(Guid.NewGuid());
        private static OutgoId outgoId = OutgoId.From(Guid.NewGuid());
        private static ExpenseId expenseId = ExpenseId.From(Guid.NewGuid());
        private static PeriodId periodId = PeriodId.From(Guid.NewGuid());
        public class When_create_budget: SpecsFor<BudgetService>
        {
            protected override void When()
            {
                AsyncContext.Run(async () => await SUT.Handle(budgetId, new V1.Create { BudgetId = budgetId, Name = "test" }));
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
                Assert.Throws<InvalidOperationException>(()=> AsyncContext.Run(async () => await SUT.Handle(budgetId, new V1.Create {BudgetId = budgetId, Name = "name" })));
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
                AsyncContext.Run(async () => await SUT.Handle(budgetId, new V1.AddIncome { IncomeId = Guid.NewGuid(), Amount = 100m, Type = IncomeType.Bonus.Name }));
            }
            [Test]
            public void Then_budget_should_be_updated() 
            {
                GetMockFor<IAggregateStore>().Verify(x => x.Save<Budget, BudgetId>(It.IsAny<Budget>()));
            }
        }
        public class When_handle_update_commands_given_is_add_commands : SpecsFor<BudgetService>
        {
            protected override void Given()
            {
                Given<BudgetExist>();
                base.Given();
            }
            
            
            [TestCaseSource(nameof(CommandCases))]
            public void Then_command_should_be_handled(object cmd) 
            {
                AsyncContext.Run(async () => await SUT.Handle(budgetId,cmd));
                GetMockFor<IAggregateStore>()
                    .Verify(x => x.Save<Budget, BudgetId>(It.IsAny<Budget>()));
            }
            static object[] CommandCases =
                {
                new object[]{new V1.AddIncome { Amount = 100m, IncomeId = incomeId,Type = IncomeType.Perks.Name} },
                new object[]{new V1.AddOutgo {Amount = 55m, OutgoId = outgoId, Type = OutgoType.Once.Name } },
                new object[]{new V1.AddExpense { Amount = 33m, ExpenseId = expenseId }, },
                new object[]{new V1.AddPeriod { PeriodId = Guid.NewGuid(),StartTime = new DateTime(2022,3,10,12,0,0) } }
                };
        }
        public class When_handle_update_commands_given_is_update_commands : SpecsFor<BudgetService>
        {
            protected override void Given()
            {
                Given<BudgetExist>();
                Given<BudgetHasIncome>();
                Given<BudgetHasOutgo>();
                Given<BudgetHasExpense>();
                Given<BudgetHasPeriod>();
                base.Given();
            }
            [TestCaseSource(nameof(CommandCases))]
            public void Then_command_should_be_handled(object cmd) 
            {
                AsyncContext.Run(async () => await SUT.Handle(budgetId,cmd));
                GetMockFor<IAggregateStore>()
                    .Verify(x => x.Save<Budget,BudgetId>(It.IsAny<Budget>()));
            }
            static object[] CommandCases =
            {
                new object[]{ new V1.ChangeIncomeAmount {  Amount = 200m, IncomeId = incomeId} },
                new object[]{new V1.ChangeIncomeType { Type = IncomeType.Gambling.Name, IncomeId = incomeId} },
                new object[]{new V1.ChangeOutgoAmount {  Amount = 600m, OutgoId = outgoId} },
                new object[]{new V1.ChangeOutgoType { OutgoId = outgoId,  Type = OutgoType.Unexpected.Name} },
                new object[]{new V1.ChangeExpenseAmount {ExpenseId = expenseId, Amount = 233m} },
                new object[]{new V1.SetPeriodState {PeriodId = periodId, PeriodState = PeriodState.NonCyclic.Name} },
                new object[]{new V1.SetPeriodStopTime {PeriodId = periodId, StopTime = DateTime.Now.AddDays(32)} },
                new object[]{new V1.SetPeriodType { PeriodId = periodId, PeriodType = PeriodType.Month.Name} }
            };
        }
        private class BudgetHasIncome : IContext<BudgetService>
        {
            public void Initialize(ISpecs<BudgetService> state)
            {
                AsyncContext.Run(async ()=> await state.SUT.Handle(budgetId,new V1.AddIncome { IncomeId = incomeId, Amount = 1000m, Type = IncomeType.Bonus.Name }));
            }
        }
        private class BudgetHasOutgo : IContext<BudgetService>
        {
            public void Initialize(ISpecs<BudgetService> state)
            {
                AsyncContext.Run(async () => await state.SUT.Handle(budgetId,new V1.AddOutgo {  OutgoId = outgoId, Amount = 400m, Type = OutgoType.Monthly.Name }));
            }
        }
        private class BudgetHasExpense : IContext<BudgetService>
        {
            public void Initialize(ISpecs<BudgetService> state)
            {
                AsyncContext.Run(async () => await state.SUT.Handle(budgetId, new V1.AddExpense { ExpenseId = expenseId, Amount = 23m }));
            }
        }
        private class BudgetHasPeriod : IContext<BudgetService>
        {
            public void Initialize(ISpecs<BudgetService> state)
            {
                AsyncContext.Run(async () => await state.SUT.Handle(budgetId, new V1.AddPeriod { PeriodId = periodId, StartTime = DateTime.Now }));
            }
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
