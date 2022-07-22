using System.Threading.Tasks;
using Useless.Framework;
using Xamarin.Forms;

namespace Useless.ViewModels.Bases
{
    public abstract class InOutRegistrationViewModel<T,K> : BaseValidationViewModel<K>
    {
        public InOutRegistrationViewModel(
            INavigationService navService) : base(navService)
        { }
        private string description;

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                Validate(() => !string.IsNullOrWhiteSpace(description), "Description must be provided");
                OnPropertyChanged();
                SaveCommand.ChangeCanExecute();
            }
        }
        private decimal amount;

        public decimal Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                Validate(() => amount >= 0, "Amount cannot be negative");
                OnPropertyChanged();
                SaveCommand.ChangeCanExecute();
            }
        }

        private Command saveCommand;

        public Command SaveCommand =>
            saveCommand ??= new Command(async () => await Save(),CanSave);
        protected abstract Task Save();
        bool CanSave() => !HasErrors;
        public abstract string Title { get; }
        public K Id { get; private set; }

        public override void Init(K item)
        {
            Id = item;
        }
    }
}
