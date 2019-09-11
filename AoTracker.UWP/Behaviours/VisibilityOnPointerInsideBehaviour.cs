using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Microsoft.Xaml.Interactivity;

namespace AoTracker.UWP.Behaviours
{
    public class VisibilityOnPointerInsideBehaviour : Behavior<UIElement>
    {
        public static readonly DependencyProperty VisibilityProperty = DependencyProperty.Register(
            "Visibility", typeof(Visibility), typeof(VisibilityOnPointerInsideBehaviour), new PropertyMetadata(default(Visibility)));

        public Visibility Visibility
        {
            get { return (Visibility) GetValue(VisibilityProperty); }
            set { SetValue(VisibilityProperty, value); }
        }

        protected override void OnAttached()
        {
            AssociatedObject.PointerEntered += AssociatedObjectOnPointerEntered;
            AssociatedObject.PointerExited += AssociatedObjectOnPointerExited;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PointerEntered -= AssociatedObjectOnPointerEntered;
            AssociatedObject.PointerExited -= AssociatedObjectOnPointerExited;
        }

        private void AssociatedObjectOnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }

        private void AssociatedObjectOnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Visibility = Visibility.Visible;
        }
    }
}
