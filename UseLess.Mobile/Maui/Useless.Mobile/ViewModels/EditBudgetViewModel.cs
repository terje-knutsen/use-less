using System.Windows.Input;
using Useless.Mobile.Api;
using Useless.Mobile.Extensions;
using Useless.Mobile.Models;
using Useless.Mobile.Pages.Api;
using Useless.Mobile.ViewModels.Base;
using UseLess.Messages;

namespace Useless.Mobile.ViewModels
{
    public sealed class EditBudgetViewModel : EditViewModel<Budget>, IEditName, IEditPeriod
    {
        private string name;
        private Period period;
        private DateTime start = DateTime.MinValue.AddDays(1);
        private DateTime end = DateTime.MaxValue.AddDays(-1);
        private PeriodType[] periodTypes = new[]
        {
            new PeriodType{Type = "UNDEFINED" },
            new PeriodType{Type = "WEEK" },
            new PeriodType { Type = "MONTH" },
            new PeriodType { Type = "YEAR" }    
        };

        public EditBudgetViewModel(INavigationService navService) : base(navService)
        {
        }

        public override string Title => "EditBudget".Translate();
        public DateTime MinimumEndDate => Period?.Start.AddDays(1) ?? DateTime.Now;
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
        public Period Period
        {
            get => period;
            set
            {
                period = value;
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
        public PeriodType PeriodType
        {
            get => periodType;
            set
            {
                periodType = value;
                OnPropertyChanged();
            }
        }
        public string PeriodState
        {
            get => periodState;
            set
            {
                periodState = value;
                OnPropertyChanged();
            }
        }
        public PeriodType[] PeriodTypes
        => periodTypes;
 
        protected override bool HasChanges
            => Name != OriginalItem.Name ||
                Period != OriginalItem.Period;



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
            period = parameter.Period;
            PeriodType = PeriodTypes.FirstOrDefault(x => x.Type == parameter.Period.Type);
            periodState = parameter.Period.State;
        }

        public void StartTimeChanged(DateTime startTime)
        {
        }

        public void EndTimeChanged(DateTime endTime)
        {

        }

        public void NameChanged(string oldName, string newName)
        {
            if (!string.IsNullOrEmpty(oldName))
            {

            }
        }
        public void PeriodTypeChanged(string oldPeriodType, string newName)
        {

        }


        private IDictionary<string, BudgetCommands.Command> budgetCommands = new Dictionary<string, BudgetCommands.Command>
        {
            {nameof(Name),new BudgetCommands.V1.ChangeBudgetName() },
            {nameof(End), new BudgetCommands.V1.SetPeriodStopTime() },
            {nameof(PeriodType), new BudgetCommands.V1.SetPeriodType() },
            {"t", new BudgetCommands.V1.SetPeriodState() }
        };
        private PeriodType periodType;
        private string periodState;
        private string periodTypeName;
    }
}
