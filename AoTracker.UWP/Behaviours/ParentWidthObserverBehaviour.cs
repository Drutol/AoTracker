using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace AoTracker.UWP.Behaviours
{
    public class ParentWidthObserverBehaviour : Behavior<Control>
    {
        public static readonly DependencyProperty ObservedWidthProperty = DependencyProperty.Register(
            "ObservedWidth", typeof(double), typeof(ParentWidthObserverBehaviour), new PropertyMetadata(default(double)));

        public double ObservedWidth
        {
            get => (double) GetValue(ObservedWidthProperty);
            set => SetValue(ObservedWidthProperty, value);
        }

        protected override void OnAttached()
        {
            AssociatedObject.SizeChanged += AssociatedObjectOnSizeChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SizeChanged -= AssociatedObjectOnSizeChanged;
        }

        private void AssociatedObjectOnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ObservedWidth = e.NewSize.Width;
        }
    }
}
