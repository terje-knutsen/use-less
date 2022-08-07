using Ninject.Modules;
using Useless.Forms.Navigation;
using Useless.Framework;
using Useless.Pages;
using Useless.ViewModels;

namespace Useless.Mobile.Modules
{
    internal sealed class NavigationModule : NinjectModule
    {
        public override void Load()
        {
            var service = new NavigationService();
            service.RegisterViewMapping(typeof(StartupViewModel), typeof(StartupPage));
            service.RegisterViewMapping(typeof(IncomesViewModel), typeof(IncomeCollectionPage));
            service.RegisterViewMapping(typeof(EditIncomeViewModel), typeof(EditIncomePage));
            service.RegisterViewMapping(typeof(AddIncomeViewModel), typeof(InOutRegistrationPage));
            service.RegisterViewMapping(typeof(OutgosViewModel), typeof(OutgoCollectionPage));
            service.RegisterViewMapping(typeof(AddOutgoViewModel), typeof(InOutRegistrationPage));
            service.RegisterViewMapping(typeof(EditOutgoViewModel), typeof(EditPage));
            service.RegisterViewMapping(typeof(ExpensesViewModel), typeof(CollectionPage));
            service.RegisterViewMapping(typeof(AddExpenseViewModel), typeof(InOutRegistrationPage));
            service.RegisterViewMapping(typeof(EditExpenseViewModel), typeof(EditPage));
            _ = Bind<INavigationService>().ToMethod(x => service).InSingletonScope();
        }
    }
}
