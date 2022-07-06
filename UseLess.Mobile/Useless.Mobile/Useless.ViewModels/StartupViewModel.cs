using System;
using System.Collections.ObjectModel;
using Useless.Framework;
using UseLess.Messages;

namespace Useless.ViewModels
{
    public sealed class StartupViewModel : BaseViewModel
    {
        private ObservableCollection<ReadModels.Budget> budgets;
        private ReadModels.Budget selectedBudget;

        public StartupViewModel(INavigationService navService) : base(navService)
        {
            budgets = new ObservableCollection<ReadModels.Budget>()
        {
            new ReadModels.Budget{
                BudgetId = Guid.NewGuid(),
                Available = 559.22m,
                End = DateTime.Now.AddDays(13).Date,
                Start = DateTime.Now.AddDays(-16).Date,
                EntryTime = DateTime.Now.AddDays(-17),
                Expense = 2455.00m,
                Income = 10780.00m,
                Left = 5305.00m,
                Limit = 559.00m,
                Name = "Juni-juli",
                Outgo = 0.00m
            },
            new ReadModels.Budget{
                BudgetId = Guid.NewGuid(),
                Available = 533.22m,
                End = DateTime.Now.AddDays(13).Date,
                Start = DateTime.Now.AddDays(-16).Date,
                EntryTime = DateTime.Now.AddDays(-17),
                Expense = 344.00m,
                Income = 1500.00m,
                Left = 1200.00m,
                Limit = 55.00m,
                Name = "Bil",
                Outgo = 0.00m
            }
        };
        }
        public ObservableCollection<string> Categories => new()
        {
            "Mat",
            "Klær",
            "Barn",
            "Bil"
        };
        public ObservableCollection<ReadModels.Budget> Budgets 
        {
            get {return budgets;} 
            set
            { 
                budgets = value; 
                OnPropertyChanged(); 
            } 
        }
        public string SelectedCategory { get; set; }
        public ReadModels.Budget SelectedBudget 
        {
            get { return selectedBudget; }
            set 
            { 
                selectedBudget = value;
                OnPropertyChanged();
            } 
        }
        public decimal Spending { get; set; }
        public decimal Limit => 155m;
    }
}
