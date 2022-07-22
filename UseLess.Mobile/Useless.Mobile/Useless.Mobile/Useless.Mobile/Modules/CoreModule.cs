
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
            _ = Bind<IProjection<ReadModels.Budget>>().To<LocalProjection>();
            _ = Bind<IProjection<ReadModels.Income>>().To<LocalProjection>();
            _ = Bind<IProjection<ReadModels.Outgo>>().To<LocalProjection>();
            _ = Bind<IProjection<ReadModels.Expense>>().To<LocalProjection>();
            _ = Bind<IDictionary<string, object>>().ToConstant(Xamarin.Forms.Application.Current.Properties);
        }
    }
}
