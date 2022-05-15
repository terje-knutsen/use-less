using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Useless.Framework
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected BaseViewModel(INavigationService navService) => NavService = navService;
        protected INavigationService NavService { get; private set; }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public virtual void Init() { }
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
        protected virtual Task OnItemTapped<T>(T tapped) => Task.CompletedTask;
    }
    public abstract class BaseViewModel<TParameter> : BaseViewModel
    {
        protected BaseViewModel(INavigationService navService) : base(navService) { }
        public override void Init()
        {
            Init(default);
        }
        public virtual void Init(TParameter parameter) { }
    }
}
