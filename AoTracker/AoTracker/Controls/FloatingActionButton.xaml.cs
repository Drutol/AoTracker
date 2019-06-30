using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Domain.Enums;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AoTracker.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FloatingActionButton
    {
        public static readonly BindableProperty ButtonColorProperty = BindableProperty.Create(
            nameof(ButtonColor),
            typeof(Color),
            typeof(FloatingActionButton), 
            Color.Accent);

        public static readonly BindableProperty NativeIconProperty = BindableProperty.Create(
            nameof(NativeIcon),
            typeof(NativeIcon),
            typeof(FloatingActionButton), 
            NativeIcon.None);

        public FloatingActionButton()
        {
            InitializeComponent();
        }

        public Color ButtonColor
        {
            get => (Color) GetValue(ButtonColorProperty);
            set => SetValue(ButtonColorProperty, value);
        }

        public NativeIcon NativeIcon
        {
            get => (NativeIcon) GetValue(NativeIconProperty);
            set => SetValue(NativeIconProperty, value);
        }
    }
}