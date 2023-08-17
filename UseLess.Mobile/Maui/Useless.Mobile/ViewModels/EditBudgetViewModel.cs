using Useless.Mobile.Api;
using Useless.Mobile.Extensions;
using Useless.Mobile.Models;
using Useless.Mobile.ViewModels.Base;

namespace Useless.Mobile.ViewModels
{
    public sealed class EditBudgetViewModel : EditViewModel<Budget>
    {
        private string name;
        private DateTime start = DateTime.MinValue.AddDays(1);
        private DateTime end = DateTime.MaxValue.AddDays(-1);

        public EditBudgetViewModel(INavigationService navService) : base(navService)
        {

        }
        public override string Title => "EditBudget".Translate();
        public static DateTime MinimumStartDate => DateTime.MinValue;
        public DateTime MinimumEndDate => Start.AddDays(1);
        public DateTime MaximumStartDate => End.AddDays(-1);
        public static DateTime MaximumEndDate => DateTime.MaxValue;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public DateTime Start
        {
            get => start;
            set
            {
                start = value;
                OnPropertyChanged();
            }
        }

        public DateTime End
        {
            get => end;
            set
            {
                end = value;
                OnPropertyChanged();
            }
        }

        protected override bool HasChanges
            => Name != OriginalItem.Name ||
                Start != OriginalItem.Start ||
                End != OriginalItem.End;



        internal override Task DoDelete()
        {
            return Task.CompletedTask;
        }

        internal override Task DoSave()
        {
            return Task.CompletedTask;
        }
        internal override void InitializeWith(Budget parameter)
        {
            OriginalItem = parameter;
            Name = parameter.Name;
            Start = parameter.Start;
            End = parameter.End;
        }
    }
}
