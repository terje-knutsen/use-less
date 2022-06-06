using System.Collections.ObjectModel;
using Useless.Framework;

namespace Useless.ViewModels
{
    public sealed class StartupViewModel : BaseViewModel
    {
        public StartupViewModel(INavigationService navService) : base(navService)
        {
            SelectedCategory = Categories[0];
        }
        public ObservableCollection<string> Categories => new()
        {
            "Mat",
            "Klær",
            "Barn",
            "Bil"
        };
        public ObservableCollection<string> Budgets => new()
        {
            "Mat-budsjett",
            "Bil-budsjett"
        };
        public string SelectedCategory { get; set; }
        public string SelectedBudget { get; set; }
        public decimal Spending { get; set; }
        public decimal Limit => 155m;
    }
}
