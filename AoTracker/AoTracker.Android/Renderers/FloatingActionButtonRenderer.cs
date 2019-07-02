using System;
using System.ComponentModel;
using Android.Content;
using Android.Content.Res;
using Android.Views;
using Android.Widget;
using AoTracker.Droid.Renderers;
using AoTracker.Droid.Util;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using FloatingActionButton = Android.Support.Design.Widget.FloatingActionButton;

[assembly: ExportRenderer(typeof(AoTracker.Controls.FloatingActionButton), typeof(FloatingActionButtonRenderer))]
namespace AoTracker.Droid.Renderers
{
    public class FloatingActionButtonRenderer : ViewRenderer<Controls.FloatingActionButton, FrameLayout>
    {
        private FloatingActionButton Fab => Control.GetChildAt(0) as FloatingActionButton;

        public FloatingActionButtonRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Controls.FloatingActionButton> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
                return;

            var container = new FrameLayout(Context);
            var fab = new FloatingActionButton(Context)
            {
                LayoutParameters = new FrameLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent)
                {
                    Gravity = GravityFlags.Center
                },
                UseCompatPadding = true
            };
            container.AddView(fab);

            UpdateBackground(fab);
            UpdateIcon(fab);

            fab.Click += OnClick;

            SetNativeControl(container);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Element.ButtonColor))
            {
                UpdateBackground(Fab);
            }
            else if (e.PropertyName == nameof(Element.NativeIcon))
            {
                UpdateIcon(Fab);
            }

            base.OnElementPropertyChanged(sender, e);

        }

        private void UpdateBackground(FloatingActionButton fab)
        {
            fab.SupportBackgroundTintList = ColorStateList.ValueOf(Element.ButtonColor.ToAndroid());
        }

        private void UpdateIcon(FloatingActionButton fab)
        {
            fab.SetImageResource(Element.NativeIcon.ToResource());
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);
            Control.BringToFront();
        }

        private void OnClick(object sender, EventArgs e)
        {
            ((IButtonController) Element).SendClicked();
        }
    }
}