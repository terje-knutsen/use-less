using System;
using Xamarin.Forms;

namespace Useless.Pages.Controls
{
    public sealed class BudgetsCarouselView : CarouselView
    {
        public static readonly BindableProperty AppItemProperty =
            BindableProperty.Create(
                nameof(AppItem),
                typeof(Guid),
                typeof(BudgetsCarouselView),
                defaultBindingMode:BindingMode.TwoWay);
            public Guid AppItem 
        {
            get => (Guid)GetValue(AppItemProperty);
            set => SetValue(AppItemProperty, value);
        }
        public static readonly BindableProperty ShouldScrollProperty =
            BindableProperty.Create(
                nameof(ShouldScroll),
                typeof(bool),
                typeof(BudgetsCarouselView),
                defaultBindingMode:BindingMode.TwoWay);
        public bool ShouldScroll 
        {
            get => (bool)GetValue(ShouldScrollProperty);
            set => SetValue(ShouldScrollProperty, value);
        }
    }
}
