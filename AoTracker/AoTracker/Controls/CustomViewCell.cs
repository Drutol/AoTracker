using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AoTracker.Controls
{
    public class CustomViewCell : ViewCell
    {
        public static readonly BindableProperty SelectedBackgroundColorProperty =
            BindableProperty.Create(nameof(SelectedBackgroundColor),
                typeof(Color),
                typeof(CustomViewCell),
                Color.Default);

        public static readonly BindableProperty ShowSelectedBackgroundProperty =
            BindableProperty.Create(nameof(ShowSelectedBackground),
                typeof(bool),
                typeof(CustomViewCell),
                false);

        public bool ShowSelectedBackground
        {
            get => (bool)GetValue(ShowSelectedBackgroundProperty);
            set => SetValue(ShowSelectedBackgroundProperty, value);
        }

        public Color SelectedBackgroundColor
        {
            get => (Color)GetValue(SelectedBackgroundColorProperty);
            set => SetValue(SelectedBackgroundColorProperty, value);
        }
    }
}
