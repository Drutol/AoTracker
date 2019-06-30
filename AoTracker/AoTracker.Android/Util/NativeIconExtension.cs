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
using AoTracker.Domain.Enums;

namespace AoTracker.Droid.Util
{
    public static class NativeIconExtension
    {
        public static int ToResource(this NativeIcon icon)
        {
            switch (icon)
            {
                case NativeIcon.Add:
                    return Resource.Drawable.icon_add;
            }

            return 0;
        }
    }
}