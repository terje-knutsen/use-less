using Useless.Mobile.ViewModels;

namespace Useless.Mobile.Pages;

public partial class EditBudgetPage : ContentPage
{
	public EditBudgetPage(EditBudgetViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}