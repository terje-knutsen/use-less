using Microsoft.Maui.Platform;

namespace Useless.Mobile.Pages.Views;

public partial class AddBudget : Microsoft.Maui.Controls.ContentView
{
	public AddBudget()
	{
		InitializeComponent();
	}

    private void nameEntry_Completed(object sender, EventArgs e)
    {
#if ANDROID
		if (Platform.CurrentActivity.CurrentFocus != null)
			Platform.CurrentActivity.HideKeyboard(Platform.CurrentActivity.CurrentFocus);
#endif
	}
}