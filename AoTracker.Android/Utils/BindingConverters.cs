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

namespace AoTracker.Android.Utils
{
    public static class BindingConverters
    {
        public static ViewStates BoolToVisibility(bool arg)
        {
            return arg ? ViewStates.Visible : ViewStates.Gone;
        }
    }
}