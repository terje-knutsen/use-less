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
