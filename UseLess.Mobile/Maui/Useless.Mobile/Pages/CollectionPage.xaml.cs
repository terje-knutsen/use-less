using Useless.Mobile.ViewModels;

namespace Useless.Mobile.Pages;

public partial class CollectionPage : ContentPage
{
	public CollectionPage(ExpensesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
		var vm = BindingContext as ExpensesViewModel;
		vm?.Refresh();
    }
}