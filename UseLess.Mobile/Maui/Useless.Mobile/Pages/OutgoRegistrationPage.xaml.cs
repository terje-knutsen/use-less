using Useless.Mobile.ViewModels;

namespace Useless.Mobile.Pages;

public partial class OutgoRegistrationPage : ContentPage
{
	public OutgoRegistrationPage(AddOutgoViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}