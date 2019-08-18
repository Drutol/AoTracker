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
    }
}