using Useless.Mobile.ViewModels;

namespace Useless.Mobile.Pages;

public partial class EditIncomePage : ContentPage
{
	public EditIncomePage(EditIncomeViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}