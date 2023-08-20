using Useless.Mobile.Pages.Api;

namespace Useless.Mobile.Pages.Views;

public partial class EditName : ContentView
{
	public EditName()
	{
		InitializeComponent();
	}

    private void Editor_TextChanged(object sender, TextChangedEventArgs e)
    {
		var vm = BindingContext as IEditName;
		vm?.NameChanged(e.OldTextValue, e.NewTextValue);
    }
}