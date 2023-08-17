using Useless.Mobile.ViewModels;

namespace Useless.Mobile.Pages;

public partial class IncomeCollectionPage : ContentPage
{
	public IncomeCollectionPage(IncomesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
		var vm = BindingContext as IncomesViewModel;
		vm?.Refresh();
    }
}