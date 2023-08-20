using Useless.Mobile.ViewModels;

namespace Useless.Mobile;

public partial class AppShell : Shell
{
	public AppShell(StartupViewModel viewModel)
	{
        InitializeComponent();
		BindingContext = viewModel;
	}
}
