using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AoTracker.Controls;
using AoTracker.Droid.Renderers;
using FFImageLoading.Forms.Platform;
using Xamarin.Forms;


[assembly: ExportRenderer(typeof(FeedImage), typeof(FeedImageRenderer))]
namespace AoTracker.Droid.Renderers
{
    public sealed class FeedImageRenderer : CachedImageFastRenderer
    {
        public FeedImageRenderer(Context context) : base(context)
        {
            SetAdjustViewBounds(true);
        }
    }
}