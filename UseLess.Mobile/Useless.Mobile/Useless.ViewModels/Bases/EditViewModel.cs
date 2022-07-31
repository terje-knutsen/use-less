using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Useless.Framework;
using Xamarin.Forms;

namespace Useless.ViewModels.Bases
{
    public abstract class EditViewModel<T> : BaseValidationViewModel<T>
    {
        private T originalItem;

        public EditViewModel(
        INavigationService navService) : base(navService)
        {
        }

        private void OnActionApplied(object sender, EventArgs e)
        {
            if (NavService.CanGoBack)
                NavService.GoBack();
        }

        public abstract string Title { get; }
        public override void Init(T item)
        {
            InitializeWith(item);
        }


        internal abstract void InitializeWith(T parameter);

        private Command saveCommand;
        private Command deleteCommand;

        public Command SaveCommand =>
            saveCommand ??= new Command(async () => await Save(), CanSave);
        public Command DeleteCommand =>
            deleteCommand ??= new Command(async () => await Delete(), CanDelete);

        private bool CanSave() => HasChanges && !HasErrors;
        private bool CanDelete() => true;

        protected abstract bool HasChanges { get; }


        public T OriginalItem
        {
            get { return originalItem; }
            set
            {
                originalItem = value;
                OnPropertyChanged();
            }
        }
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
    }
}
