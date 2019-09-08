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
    public class TextBlockHideWhenEmptyBehaviour : Behavior<TextBlock>
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(TextBlockHideWhenEmptyBehaviour),
            new PropertyMetadata(default(string), PropertyChangedCallback));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = ((TextBlockHideWhenEmptyBehaviour) d).AssociatedObject;
            var text = (string)e.NewValue;


            if (string.IsNullOrEmpty(text))
                textBlock.Visibility = Visibility.Collapsed;
            else
            {
                textBlock.Text = text;
                textBlock.Visibility = Visibility.Visible;
            }
        }

        protected override void OnAttached()
        {
            if (string.IsNullOrEmpty(AssociatedObject.Text))
                AssociatedObject.Visibility = Visibility.Collapsed;
        }
    }
}
