using System.Collections.ObjectModel;
using System.Linq;
using Useless.Mobile.iOS.Renderers;
using Useless.Pages.Controls;
using UseLess.Messages;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(BudgetsCarouselView), typeof(AppCarouselViewRenderer))]
namespace Useless.Mobile.iOS.Renderers
{
    public class AppCarouselViewRenderer : CarouselViewRenderer
    {
        protected override CarouselViewController CreateController(CarouselView newElement, ItemsViewLayout layout)
        => new AppCarouselViewController(newElement, layout);
    }
    public class AppCarouselViewController : CarouselViewController
    {
        private BudgetsCarouselView appCarouselView;
        public AppCarouselViewController(CarouselView itemsView, ItemsViewLayout layout) : base(itemsView, layout)
        {
            appCarouselView = itemsView as BudgetsCarouselView;
        }
        
        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();

             SetItem();
        }
        void SetItem()
        {
            if (appCarouselView != null && appCarouselView.ShouldScroll && appCarouselView.AppItem != null)
            {
                if (appCarouselView.ItemsSource is ObservableCollection<ReadModels.Budget> collection)
                {
                    var budget = collection.FirstOrDefault(x => x.BudgetId.ToString() == appCarouselView.AppItem.ToString());
                    int position = collection.IndexOf(budget);
                    if (position >= 0)
                        Carousel.ScrollTo(position,animate:false);
                    appCarouselView.ShouldScroll = false;
                }
            }
        }
    }
}