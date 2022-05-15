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

            _ = Bind<INavigationService>().ToMethod(x => service).InSingletonScope();
        }
    }
}
