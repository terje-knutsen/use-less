
using Akavache;
using Ninject.Modules;
using System.Collections.Generic;
using Useless.Api;
using Useless.ApplicationService.InMemory;
using Useless.ViewModels;
using UseLess.Messages;

namespace Useless.Mobile.Modules
{
    internal sealed class CoreModule : NinjectModule
    {
        public override void Load()
        {
            _ = Bind<StartupViewModel>().ToSelf();
            _ = Bind<IncomesViewModel>().ToSelf();
            _ = Bind<AddIncomeViewModel>().ToSelf();
            _ = Bind<OutgosViewModel>().ToSelf();
            _ = Bind<AddOutgoViewModel>().ToSelf();
            _ = Bind<EditIncomeViewModel>().ToSelf();
            _ = Bind<EditOutgoViewModel>().ToSelf();
            _ = Bind<EditExpenseViewModel>().ToSelf();
            _ = Bind<ExpensesViewModel>().ToSelf();
            _ = Bind<AddExpenseViewModel>().ToSelf();
            _ = Bind<IBlobCache>().ToConstant(BlobCache.LocalMachine);
            _ = Bind<IApplyBudgetCommand>().To<LocalCommandApplier>();
            _ = Bind<IProjection<ReadModels.Budget, QueryModels.GetBudget>>().To<LocalProjection>();
            _ = Bind<IProjection<ReadModels.Income,QueryModels.GetIncome>>().To<LocalProjection>();
            _ = Bind<IProjection<ReadModels.Outgo,QueryModels.GetOutgo>>().To<LocalProjection>();
            _ = Bind<IProjection<ReadModels.Expense,QueryModels.GetExpense>>().To<LocalProjection>();
            _ = Bind<ICollectionProjection<ReadModels.Income, QueryModels.GetIncomes>>().To<LocalProjection>();
            _ = Bind<ICollectionProjection<ReadModels.Budget, QueryModels.GetBudgets>>().To<LocalProjection>();
            _ = Bind<ICollectionProjection<ReadModels.Outgo, QueryModels.GetOutgos>>().To<LocalProjection>();
            _ = Bind<ICollectionProjection<ReadModels.Expense, QueryModels.GetExpenses>>().To<LocalProjection>();
            _ = Bind<ICollectionProjection<ReadModels.IncomeType, QueryModels.GetIncomeTypes>>().To<LocalProjection>();
            _ = Bind<ICollectionProjection<ReadModels.OutgoType, QueryModels.GetOutgoTypes>>().To<LocalProjection>();
            _ = Bind<IDictionary<string, object>>().ToConstant(Xamarin.Forms.Application.Current.Properties);
        }
    }
}
