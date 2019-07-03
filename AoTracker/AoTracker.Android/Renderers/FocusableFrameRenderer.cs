using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AoTracker.Controls;
using AoTracker.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using FrameRenderer = Xamarin.Forms.Platform.Android.FastRenderers.FrameRenderer;

[assembly: ExportRenderer(typeof(AoTracker.Controls.FocusableFrame), typeof(FocusableFrameRenderer))]
namespace AoTracker.Droid.Renderers
{
    public class FocusableFrameRenderer : FrameRenderer
    {
        public FocusableFrameRenderer(Context context) : base(context)
        {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            Control.Clickable = true;
            Control.Focusable = true;

            var outValue = new TypedValue();
            Context.Theme.ResolveAttribute(
                global::Android.Resource.Attribute.SelectableItemBackground, outValue, true);
            Control.Foreground = Context.GetDrawable(outValue.ResourceId);
            Control.Click += ControlOnClick;
        }

        private void ControlOnClick(object sender, EventArgs e)
        {
            (Element as FocusableFrame).OnClicked();
        }
    }
}