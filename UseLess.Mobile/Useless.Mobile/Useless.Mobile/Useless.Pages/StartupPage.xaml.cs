using System;
using Useless.Framework;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Useless.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartupPage : ContentPage
    {
        public StartupPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is BaseViewModel viewModel)
                viewModel.Init();
        }
    }
}