using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AoTracker.Domain.Enums;
using Xamarin.Forms;

namespace AoTracker.XamlResources
{
    public static class ResourceExtension
    {
        private const string Colour = "Colour";

        private const string AccentColour = "AccentColour";
        private const string AccentDarkColour = "AccentDarkColour";
        private const string BackgroundColour = "BackgroundColour";
        private const string ToolbarColour = "ToolbarColour";
        private const string TextColour = "TextColour";
        private const string ToolbarTextColour = "ToolbarTextColour";
        private const string ForegroundColour = "ForegroundColour";
        private const string ForegroundDeepColour = "ForegroundDeepColour";
        private const string RippleColour = "RippleColour";

        private const string Dark = "Dark";
        private const string Light = "Light";

        private const string Orange = "Orange";
        private const string Lime = "Lime";
        private const string SkyBlue = "SkyBlue";
        private const string Purple = "Purple";

        public static void SetTheme(this ResourceDictionary dictionary, AppTheme appTheme)
        {
            object accentColour = null;
            object accentDarkColour = null;

            if ((appTheme & AppTheme.Orange) == AppTheme.Orange)
            {
                accentColour = dictionary.GetResourceValue(Orange + Colour);
                accentDarkColour = dictionary.GetResourceValue(Orange + Dark + Colour);
            }
            else if ((appTheme & AppTheme.Lime) == AppTheme.Lime)
            {
                accentColour = dictionary.GetResourceValue(Lime + Colour);
                accentDarkColour = dictionary.GetResourceValue(Lime + Dark + Colour);
            }
            else if ((appTheme & AppTheme.SkyBlue) == AppTheme.SkyBlue)
            {
                accentColour = dictionary.GetResourceValue(SkyBlue + Colour);
                accentDarkColour = dictionary.GetResourceValue(SkyBlue + Dark + Colour);
            }
            else if ((appTheme & AppTheme.Purple) == AppTheme.Purple)
            {
                accentColour = dictionary.GetResourceValue(Purple + Colour);
                accentDarkColour = dictionary.GetResourceValue(Purple + Dark + Colour);
            }

            if ((appTheme & AppTheme.Light) == AppTheme.Light)
            {
                dictionary[BackgroundColour] = dictionary.GetResourceValue(Light + BackgroundColour);
                dictionary[ToolbarColour] = accentColour;
                dictionary[TextColour] = dictionary.GetResourceValue(Light + TextColour);
                dictionary[ToolbarTextColour] = dictionary.GetResourceValue(Dark + TextColour);
                dictionary[ForegroundColour] = dictionary.GetResourceValue(Light + ForegroundColour);
                dictionary[ForegroundDeepColour] = dictionary.GetResourceValue(Light + ForegroundDeepColour);
                dictionary[RippleColour] = dictionary.GetResourceValue(Light + RippleColour);
            }
            else
            {
                dictionary[BackgroundColour] = dictionary.GetResourceValue(Dark + BackgroundColour);
                dictionary[ToolbarColour] = dictionary.GetResourceValue(Dark + ToolbarColour);
                dictionary[TextColour] = dictionary.GetResourceValue(Dark + TextColour);
                dictionary[ToolbarTextColour] = dictionary.GetResourceValue(Dark + TextColour);
                dictionary[ForegroundColour] = dictionary.GetResourceValue(Dark + ForegroundColour);
                dictionary[ForegroundDeepColour] = dictionary.GetResourceValue(Dark + ForegroundDeepColour);
                dictionary[RippleColour] = dictionary.GetResourceValue(Dark + RippleColour);
            }

            dictionary[AccentColour] = accentColour;
            dictionary[AccentDarkColour] = accentDarkColour;
        }

        private static object GetResourceValue(this ResourceDictionary dictionary, string keyName)
        {
            if(!dictionary.TryGetValue(keyName, out var retVal))
            {
                Debugger.Break();
            }
            
            return retVal;
        }
    }
}
