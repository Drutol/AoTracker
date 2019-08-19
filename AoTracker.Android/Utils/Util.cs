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
    public static class Util
    {
        public static int IndexToIconResource(int index)
        {
            if (index == 1)
                return Resource.Drawable.icon_one;
            if (index == 2)
                return Resource.Drawable.icon_two;
            if (index == 3)
                return Resource.Drawable.icon_three;
            if (index == 4)
                return Resource.Drawable.icon_four;
            if (index == 5)
                return Resource.Drawable.icon_five;
            if (index == 6)
                return Resource.Drawable.icon_six;
            if (index == 7)
                return Resource.Drawable.icon_seven;
            if (index == 8)
                return Resource.Drawable.icon_eight;
            if (index == 9)
                return Resource.Drawable.icon_nine;

            return Resource.Drawable.icon_box_blank;
        }
    }
}