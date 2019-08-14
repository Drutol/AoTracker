using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.Statics;
using AoTracker.Interfaces;
using Autofac;

namespace AoTracker.Android.Themes
{
    public static class ThemeManager
    {
        public static int TextColour { get; private set; }
        public static int TextInvertedColour { get; private set; }
        public static int ToolbarTextColour { get; private set; }

        public static void ApplyTheme(this AppCompatActivity activity)
        {
            var settings = ResourceLocator.ObtainScope().Resolve<ISettings>();
            var theme = settings.AppTheme;

            // Base
            if ((theme & AppTheme.Dark) == AppTheme.Dark)
            {
                activity.SetTheme(Resource.Style.AoTracker_Dark);
            }
            else
            {
                activity.SetTheme(Resource.Style.AoTracker_Light);
            }

            // Colour scheme
            if ((theme & AppTheme.Orange) == AppTheme.Orange)
            {
                activity.Theme.ApplyStyle(Resource.Style.ColourSchemeOrange, true);
            }
            else if ((theme & AppTheme.Lime) == AppTheme.Lime)
            {
                activity.Theme.ApplyStyle(Resource.Style.ColourSchemeLime, true);
            }

            // Toolbar overrides
            if ((theme & AppTheme.Dark) == AppTheme.Dark)
            {
                activity.SetTheme(Resource.Style.ColourSchemeToolbarDark);
            }

            var typedValue = new TypedValue();

            activity.Theme.ResolveAttribute(Resource.Attribute.TextColour, typedValue, true);
            TextColour = typedValue.Data;

            activity.Theme.ResolveAttribute(Resource.Attribute.TextInvertedColour, typedValue, true);
            TextInvertedColour = typedValue.Data;

            activity.Theme.ResolveAttribute(Resource.Attribute.ToolbarTextColour, typedValue, true);
            ToolbarTextColour = typedValue.Data;
        }


    }
}