using System.ComponentModel;
using Useless.Mobile.ViewModels.Base;

namespace Useless.Mobile.Api
{
    public interface INavigationService
    {
        Task GoBack();
        Task NavigateTo<TVM, K>(K id) where TVM : GenericBaseViewModel<K>;
    }
}
