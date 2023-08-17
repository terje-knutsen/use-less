using Useless.Mobile.ViewModels;

namespace Useless.Mobile.Pages;

public partial class EditPage : ContentPage
{
	public EditPage(EditOutgoViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
    public EditPage(EditExpenseViewModel viewModel)
    {
		InitializeComponent();
		BindingContext = viewModel;
    }
}