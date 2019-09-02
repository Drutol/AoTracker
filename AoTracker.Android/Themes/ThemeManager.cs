using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
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
        public static Color TextColour { get; private set; }
        public static Color TextInvertedColour { get; private set; }
        public static Color ToolbarTextColour { get; private set; }

        public static Color LimeColour { get; private set; }
        public static Color RedColour { get; private set; }

        public static Color AccentColour { get; set; }
        public static Color DarkBackgroundColour { get; set; }

        public static AppTheme CurrentTheme { get; set; }

        public static void ApplyTheme(this AppCompatActivity activity)
        {
            var settings = ResourceLocator.ObtainScope().Resolve<ISettings>();
            var theme = settings.AppTheme;
            CurrentTheme = theme;
            // Base
            if ((theme & AppTheme.Dark) == AppTheme.Dark)
            {
                activity.SetTheme(Resource.Style.AoTracker_Dark);
                activity.Window.SetStatusBarColor(DarkBackgroundColour);
            }
            else if ((theme & AppTheme.Black) == AppTheme.Black)
            {
                activity.SetTheme(Resource.Style.AoTracker_Black);
                activity.Window.SetStatusBarColor(Color.Black);
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
            else if ((theme & AppTheme.Cyan) == AppTheme.Cyan)
            {
                activity.Theme.ApplyStyle(Resource.Style.ColourSchemeCyan, true);
            }
            else if ((theme & AppTheme.Purple) == AppTheme.Purple)
            {
                activity.Theme.ApplyStyle(Resource.Style.ColourSchemePurple, true);
            }
            else if ((theme & AppTheme.SkyBlue) == AppTheme.SkyBlue)
            {
                activity.Theme.ApplyStyle(Resource.Style.ColourSchemeSkyBlue, true);
            }
            else if ((theme & AppTheme.Red) == AppTheme.Red)
            {
                activity.Theme.ApplyStyle(Resource.Style.ColourSchemeRed, true);
            }
            else if ((theme & AppTheme.Pink) == AppTheme.Pink)
            {
                activity.Theme.ApplyStyle(Resource.Style.ColourSchemePink, true);
            }

            // Toolbar overrides
            if ((theme & AppTheme.Dark) == AppTheme.Dark)
            {
                activity.Theme.ApplyStyle(Resource.Style.ColourSchemeToolbarDark, true);
            }
            else if ((theme & AppTheme.Black) == AppTheme.Black)
            {
                activity.Theme.ApplyStyle(Resource.Style.ColourSchemeToolbarBlack, true);
            }

            RedColour = activity.Resources.GetColor(Resource.Color.RedColour, activity.Theme);
            LimeColour = activity.Resources.GetColor(Resource.Color.LimeColour, activity.Theme);

            // Colour settings

            AccentColour = ResolveColour(activity, Resource.Attribute.AccentColour);
            TextColour = ResolveColour(activity, Resource.Attribute.TextColour);
            TextInvertedColour = ResolveColour(activity, Resource.Attribute.TextInvertedColour);
            ToolbarTextColour = ResolveColour(activity, Resource.Attribute.ToolbarTextColour);
        }

        private static Color ResolveColour(Activity activity, int attr)
        {
            var typedValue = new TypedValue();
            activity.Theme.ResolveAttribute(attr, typedValue, true);
            return new Color(typedValue.Data);
        }

    }
}