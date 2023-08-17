using Useless.Mobile.ViewModels;

namespace Useless.Mobile.Pages;

public partial class IncomeRegistrationPage : ContentPage
{
	public IncomeRegistrationPage(AddIncomeViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}