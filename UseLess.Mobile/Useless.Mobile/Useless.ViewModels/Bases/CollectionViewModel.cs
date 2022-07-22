using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Useless.Framework;
using Xamarin.Forms;

namespace Useless.ViewModels.Bases
{
    public abstract class CollectionViewModel<T> : BaseViewModel
    {
        ObservableCollection<T> entries = new();
        protected CollectionViewModel(INavigationService navService) : base(navService)
        {
        }
        public ObservableCollection<T> Items
        {
            get => entries;
            set
            {
                entries = value;
                OnPropertyChanged();
            }
        } 
        public override void Init()
        {
            LoadEntries();
        }
        private Command refreshCommand;

        public Command RefreshCommand => refreshCommand ??= new Command(() => LoadEntries());

        protected void LoadEntries()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                InitCollection();
                SetCurrent();
            }
            finally
            {
                IsBusy = false;
            }
        }
        public Command NewItemCommand => new Command(async () => await OnNewItemCommand());

        protected override async Task OnItemTapped<I>(I tapped)
        {
            if (tapped is T item)
            {
                await OnItemSelected(item);
            }
        }
        protected abstract void InitCollection();
        protected virtual void SetCurrent() { }
        protected abstract Task OnNewItemCommand();
        protected abstract Task OnItemSelected(T item);
    }

    public abstract class GenericCollectionViewModel<T,K> : GenericBaseViewModel<K>
    {
        ObservableCollection<T> entries = new();

        public GenericCollectionViewModel(
            INavigationService navService) : base(navService)
        { }

        public ObservableCollection<T> Items
        {
            get => entries;
            set
            {
                entries = value;
                OnPropertyChanged();
            }
        }

        public override void Init(K item = default)
        {
            SetItem(item);
            LoadEntries();
        }
        internal virtual void SetItem(K item) 
        { }
        private Command refreshCommand;

        public Command RefreshCommand => refreshCommand ??= new Command(() => LoadEntries());

        protected void LoadEntries()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                SetCollection();
            }
            finally
            {
                IsBusy = false;
            }
        }
        public Command NewItemCommand => new Command(async () => await OnNewItemCommand());

        protected override async Task OnItemTapped<I>(I tapped)
        {
            if (tapped is T item)
            {
                await OnItemSelected(item);
            }
        }
        protected abstract void SetCollection();
        protected abstract Task OnNewItemCommand();
        protected abstract Task OnItemSelected(T item);
    }
}
