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
using AoTracker.Crawlers.Enums;
using AoTracker.Domain.Enums;

namespace AoTracker.Android.Utils
{
    public static class Extensions
    {
        public static int ToImageResource(this CrawlerDomain domain)
        {
            if (domain == CrawlerDomain.Mandarake)
                return Resource.Drawable.mandarake;
            if (domain == CrawlerDomain.Surugaya)
                return Resource.Drawable.surugaya;
            if (domain == CrawlerDomain.Mercari)
                return Resource.Drawable.mercari;
            if (domain == CrawlerDomain.Yahoo)
                return Resource.Drawable.yahoo;
            if (domain == CrawlerDomain.Lashinbang)
                return Resource.Drawable.lashinbang;
            return 0;
        }

        public static int ToIconResource(this PageIndex page)
        {
            if (page == PageIndex.Feed)
                return Resource.Drawable.icon_feed;
            if (page == PageIndex.CrawlerSets)
                return Resource.Drawable.icon_sets;
            if (page == PageIndex.SettingsIndex)
                return Resource.Drawable.icon_setting;
            if (page == PageIndex.SettingsGeneral)
                return Resource.Drawable.icon_setting;
            if (page == PageIndex.SettingsAbout)
                return Resource.Drawable.icon_info;
            if (page == PageIndex.IgnoredItems)
                return Resource.Drawable.icon_stop;
            return 0;
        }
    }
}