using Akavache;
using System;
using System.Threading.Tasks;
using Useless.Api;
using Useless.Framework;
using Xamarin.Forms;

namespace Useless.ViewModels.Bases
{
    public abstract class EditViewModel<T> : BaseValidationViewModel<T>
    {
        private string type;
        private decimal amount;

        public EditViewModel(
        INavigationService navService,
        IProjection<T> queryService,
        IBlobCache cache) : base(navService)
        {
            this.queryService = queryService;
            this.cache = cache;
        }

        private void OnActionApplied(object sender, EventArgs e)
        {
            if (NavService.CanGoBack)
                NavService.GoBack();
        }

        public abstract string Title { get; }
        public string Type
        {
            get => type;
            set
            {
                type = value;
                Validate(() => !string.IsNullOrWhiteSpace(type), "Description must be provided");
                OnPropertyChanged();
                SaveCommand.ChangeCanExecute();
            }
        }
        private string tag;
        public string Tag
        {
            get => tag;
            set
            {
                tag = value.ToUpper();
                Validate(() => !string.IsNullOrWhiteSpace(tag), "Tag cannot be empty");
                OnPropertyChanged();
                //AddTagCommand.ChangeCanExecute();
                //DeleteTagCommand.ChangeCanExecute();
            }
        }

        public decimal Amount
        {
            get => amount;
            set
            {
                amount = value;
                Validate(() => amount > 0, "Amount must be greater than zero");
                OnPropertyChanged();
                SaveCommand.ChangeCanExecute();
            }
        }
        //private ObservableCollection<ReadModels.Tag> tags = new ObservableCollection<ReadModels.Tag>();
        //public ObservableCollection<ReadModels.Tag> Tags
        //{
        //    get => tags;
        //    set
        //    {
        //        UpdateSelection(value);
        //        tags = value;
        //        OnPropertyChanged();
        //    }
        //}

        //protected abstract void UpdateSelection(ObservableCollection<ReadModels.Tag> value);

        public override void Init(T item)
        {
            InitializeWith(item);
        }


        internal abstract void InitializeWith(T parameter);

        private Command saveCommand;
        private Command deleteCommand;
        private Command addTagCommand;
        private Command deleteTagCommand;
        private readonly IProjection<T> queryService;
        private readonly IBlobCache cache;

        public Command SaveCommand =>
            saveCommand ??= new Command(async () => await Save(), CanSave);
        public Command DeleteCommand =>
            deleteCommand ??= new Command(async () => await Delete(), CanDelete);
        //public Command AddTagCommand =>
        //    addTagCommand ??= new Command(async (x) =>
        //    await AddTag(x), x => CanAddTag());
        //public Command DeleteTagCommand =>
        //    deleteTagCommand ??= new Command(async (x) =>
        //    await DeleteTag(x), x => CanDeleteTag());
        private bool CanSave() => HasChanges && !HasErrors;
        private bool CanDelete() => true;
        //private bool CanAddTag() => !string.IsNullOrEmpty(Tag);
        //private bool CanDeleteTag() => Tags.Any(t => t.Text.ToUpper() == Tag.ToUpper());
        private bool HasChanges => TypeChanged || AmountChanged;
        internal bool TypeChanged
        {
            get
            {
                if (OriginalItem == null) return false;
                return OriginalType != Type;
            }
        }
        internal bool AmountChanged
        {
            get
            {
                if (OriginalItem == null) return false;
                return OriginalAmount != Amount;
            }
        }
        private T originalItem;
        public T OriginalItem
        {
            get { return originalItem; }
            set
            {
                originalItem = value;
                OnPropertyChanged();
            }
        }
        protected abstract string OriginalType { get; }
        public abstract decimal OriginalAmount { get; }
        public Guid Id { get; private set; }

        public async Task Save()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                await DoSave();
                await NavService.GoBack();
            }
            finally
            {
                IsBusy = false;
            }
        }
        public async Task Delete()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                await DoDelete();
                await NavService.GoBack();
            }
            finally
            {
                IsBusy = false;
            }
        }

        internal abstract Task DoSave();
        internal abstract Task DoDelete();
        //internal abstract Task ConnectTag(int tagId);
        //internal abstract Task UnConnectTag(int tagId);
        //protected virtual async Task AddTag(object x)
        //{
        //    Tag = string.Empty;
        //    SyncTags();
        //    await Task.CompletedTask;
        //}
        //protected virtual async Task DeleteTag(object x)
        //{
        //    Tag = string.Empty;
        //    SyncTags();
        //    await Task.CompletedTask;
        //}
        //protected int TagIdToRemove() => Tags.Where(t => t.Text.ToUpper() == Tag.ToUpper()).Select(t => t.TagId).FirstOrDefault();


        //protected override async Task OnItemTapped<K>(K tapped)
        //{
        //    if (tapped is ReadModels.Tag item)
        //    {
        //        item.Selected = !item.Selected;
        //        if (item.Selected)
        //        {
        //            await ConnectTag(item.TagId);
        //        }
        //        else
        //        {
        //            await UnConnectTag(item.TagId);
        //        }
        //    }
        //    SyncTags();
        //    await Task.CompletedTask;
        //}
        //private void SyncTags()
        //{
        //    cache.GetAndFetchLatest("tags", async ()
        //     => await queryService.GetTagsAsync())
        //     .Subscribe(observer => Tags = new ObservableCollection<ReadModels.Tag>(observer));
        //}
    }
}
