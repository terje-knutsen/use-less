using Ninject;
using Ninject.Modules;
using Useless.Forms.Navigation;
using Useless.Framework;
using Useless.Mobile.Modules;
using Useless.Pages;
using Useless.ViewModels;
using Xamarin.Forms;

namespace Useless.Mobile
{
    public partial class App : Application
    {
        public App(params INinjectModule[] platformModules)
        {
            InitializeComponent();
            Device.SetFlags(new string[] { "Shapes_Experimental" });
            Kernel = new StandardKernel(new CoreModule(), new NavigationModule());
            Kernel.Load(platformModules);
            Akavache.Registrations.Start("Useless.Mobile");
            MainPage = CreateStartupPage();
        }

        private Page CreateStartupPage()
        {
            var page = new NavigationPage(new StartupPage 
            {
                BindingContext = Kernel.Get<StartupViewModel>()
            });
            var service = Kernel.Get<INavigationService>() as NavigationService;
            service.XamarinFormsNav = page.Navigation;
            return page;
        }

        protected override void OnStart()
        {
            //#ffffff • #d0e1f9 • #4d648d • #283655 • #1e1f26
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        public IKernel Kernel { get; private set; }
    }
}
