using Useless.Mobile.ViewModels;

namespace Useless.Mobile.Pages;

public partial class StartupPage : ContentPage
{
	public StartupPage(StartupViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
		var vm = BindingContext as StartupViewModel;
		vm?.UpdateCollection();
    }

    private void CarouselView_PositionChanged(object sender, PositionChangedEventArgs e)
    {
       var vm = BindingContext as StartupViewModel;
        vm?.SetCurrentPosition(e.CurrentPosition);
    }
}