using Useless.Mobile.ViewModels;

namespace Useless.Mobile.Pages;

public partial class OutgoCollectionPage : ContentPage
{
	public OutgoCollectionPage(OutgosViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
		var vm = BindingContext as OutgosViewModel;
		vm?.Refresh();
    }
}