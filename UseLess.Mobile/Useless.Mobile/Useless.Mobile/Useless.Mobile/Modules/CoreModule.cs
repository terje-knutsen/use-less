
using Ninject.Modules;
using Useless.ViewModels;

namespace Useless.Mobile.Modules
{
    internal sealed class CoreModule : NinjectModule
    {
        public override void Load()
        {
            _ = Bind<StartupViewModel>().ToSelf();
        }
    }
}
