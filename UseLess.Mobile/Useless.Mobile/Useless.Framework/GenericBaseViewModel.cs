using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Useless.Framework
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected ViewModelBase(INavigationService navService) => NavService = navService;
        protected INavigationService NavService { get; private set; }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        Command<object> tapGestureCommand;
        public Command<object> TapGestureCommand
            => tapGestureCommand ??= new Command<object>(async tapped => await HandleTapGesture(tapped));

        protected virtual async Task HandleTapGesture(object tapped)
        {
            await OnItemTapped(tapped);
        }
        protected virtual Task OnItemTapped<K>(K tapped) => Task.CompletedTask;
    }
    public abstract class BaseViewModel : ViewModelBase
    {
        protected BaseViewModel(INavigationService navService) : base(navService)
        { }
        public abstract void Init();
    }
    public abstract class GenericBaseViewModel<T> : ViewModelBase
    {
        protected GenericBaseViewModel(INavigationService navService) : base(navService)
        { }
        public abstract void Init(T item);
    }
}
