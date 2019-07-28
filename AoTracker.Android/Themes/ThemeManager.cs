using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
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
        public static void ApplyTheme(this AppCompatActivity activity)
        {
            var settings = ResourceLocator.ObtainScope().Resolve<ISettings>();
            var theme = settings.AppTheme;

            if ((theme & AppTheme.Dark) == AppTheme.Dark)
            {
                activity.SetTheme(Resource.Style.AoTracker_Light);
            }
            else
            {
                activity.SetTheme(Resource.Style.AoTracker_Light);
            }

            if ((theme & AppTheme.Orange) == AppTheme.Orange)
            {
                activity.Theme.ApplyStyle(Resource.Style.ColourSchemeOrange, true);
            }
            else if ((theme & AppTheme.Lime) == AppTheme.Lime)
            {
                activity.Theme.ApplyStyle(Resource.Style.ColourSchemeLime, true);
            }
        }
    }
}