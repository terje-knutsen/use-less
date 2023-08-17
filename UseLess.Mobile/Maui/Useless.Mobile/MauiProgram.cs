using Microsoft.Extensions.Logging;
using Useless.Mobile.Extensions;
using Useless.Mobile.ViewModels.Base;

namespace Useless.Mobile;

public static class MauiProgram
{
    private static MauiAppBuilder builder;

    public static MauiApp CreateMauiApp()
	{
		builder = MauiApp.CreateBuilder();
		
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.RegisterAppServices()
			.RegisterViewModels()
			.RegisterRoutes();
		

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	public static IServiceCollection AddPage<TPage,TViewModel>(IServiceCollection services) 
		where TPage : Page 
		where TViewModel : ViewModelBase
	{
		services.AddTransient(typeof(TPage))
			.AddTransient(typeof(TViewModel));
		Routing.RegisterRoute(typeof(TViewModel).FullName,typeof(TPage));
		return services;
	}
}
