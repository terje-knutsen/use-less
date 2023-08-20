using System.ComponentModel;
using Useless.Mobile.Api;
using Useless.Mobile.ViewModels.Base;

namespace Useless.Forms.Navigation
{
    internal sealed class NavigationService : INavigationService
    {
        public event PropertyChangedEventHandler CanGoBackChanged;

        public async Task GoBack()
        {
            await Shell.Current.Navigation.PopAsync(false);
        }
 
        public async Task NavigateTo<TVM,K>(K id) where TVM : GenericBaseViewModel<K>
        {
            await NavigateToView(typeof(TVM));
            if (Shell.Current.CurrentPage.BindingContext is GenericBaseViewModel<K> vm)
                vm.Init(id);
        }
 
        private static async Task NavigateToView(Type type)
        => await Shell.Current.GoToAsync(type.FullName,false);
    }
}
