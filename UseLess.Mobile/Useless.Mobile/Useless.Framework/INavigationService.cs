using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Useless.Framework
{
    public interface INavigationService
    {
        bool CanGoBack { get; }
        Task GoBack();
        Task NavigateTo<TVM>() where TVM : BaseViewModel;
        Task NavigateTo<TVM,K>(K id) where TVM : GenericBaseViewModel<K>;
        void RemoveLastView();
        void ClearBackStack();
        Task NavigateToUri(Uri uri);
        event PropertyChangedEventHandler CanGoBackChanged;
    }
}
