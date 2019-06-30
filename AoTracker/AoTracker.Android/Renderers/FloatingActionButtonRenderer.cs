using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AoTracker.Controls;
using AoTracker.Domain.Enums;
using AoTracker.Droid.Renderers;
using FFImageLoading;
using FFImageLoading.Svg.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;
using FloatingActionButton = Android.Support.Design.Widget.FloatingActionButton;

[assembly: ExportRenderer(typeof(AoTracker.Controls.FloatingActionButton), typeof(FloatingActionButtonRenderer))]
namespace AoTracker.Droid.Renderers
{

    public class FloatingActionButtonRenderer : ViewRenderer<Controls.FloatingActionButton, FrameLayout>
    {

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
                }
            };
            container.AddView(fab);


            // set the bg
            fab.UseCompatPadding = true;
            UpdateBackground(fab);
            UpdateIcon(fab);
            fab.Click += Fab_Click;
            SetNativeControl(container);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Element.ButtonColor))
            {
                UpdateBackground(Fab);
            }

            if (e.PropertyName == nameof(Element.NativeIcon))
            {
                UpdateIcon(Fab);
            }

            base.OnElementPropertyChanged(sender, e);

        }

        private void UpdateBackground(FloatingActionButton fab)
        {
            fab.SetBackgroundColor(Element.ButtonColor.ToAndroid());
        }

        private void UpdateIcon(FloatingActionButton fab)
        {
            switch (Element.NativeIcon)
            {
                case NativeIcon.Add:
                    fab.SetImageResource(Resource.Drawable.icon_add);
                    break;
            }
        }


        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);
            Control.BringToFront();
        }

        private FloatingActionButton Fab => Control.GetChildAt(0) as FloatingActionButton;

        private void Fab_Click(object sender, EventArgs e)
        {
            // proxy the click to the element
            ((IButtonController) Element).SendClicked();
        }
    }
}