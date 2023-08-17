using Useless.Mobile.ViewModels;

namespace Useless.Mobile.Pages;

public partial class ExpenseRegistrationPage : ContentPage
{
	public ExpenseRegistrationPage(AddExpenseViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}