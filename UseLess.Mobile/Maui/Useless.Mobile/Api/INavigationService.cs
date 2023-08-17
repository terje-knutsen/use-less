using System.ComponentModel;
using Useless.Mobile.ViewModels.Base;

namespace Useless.Mobile.Api
{
    public interface INavigationService
    {
        bool CanGoBack { get; }
        Task GoBack();
        //Task NavigateTo<TVM>() where TVM : BaseViewModel;
        Task NavigateTo<TVM, K>(K id) where TVM : GenericBaseViewModel<K>;
        void RemoveLastView();
        void ClearBackStack();
        Task NavigateToUri(Uri uri);
        event PropertyChangedEventHandler CanGoBackChanged;
    }
}
