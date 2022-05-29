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
        public ObservableCollection<string> Categories => new ObservableCollection<string> 
        {
            "Mat",
            "Klær",
            "Barn",
            "Bil"
        };
        public string SelectedCategory { get; set; }
        public decimal Limit => 155m;
    }
}
