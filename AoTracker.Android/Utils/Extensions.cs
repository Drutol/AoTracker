using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AoLibs.Adapters.Android.Recycler;
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
            if (page == PageIndex.WatchedItems)
                return Resource.Drawable.icon_eye;
            return 0;
        }   
        
        public static int ToIconResource(this WelcomeStage stage)
        {
            if (stage == WelcomeStage.Hello)
                return Resource.Drawable.icon_stop_big;
            return Resource.Drawable.icon_stop_big;
        }

        public static void SetUpWithEmptyState<T>(this ObservableCollection<T> collection, View emptyState)
        {
            collection.CollectionChanged += (sender, args) =>
            {
                emptyState.Visibility = BindingConverters.BoolToVisibility(collection.Count == 0);
            };
            emptyState.Visibility = BindingConverters.BoolToVisibility(collection.Count == 0);
        }

        public static void HideKeyboard(this Activity activity)
        {
            var imm = (InputMethodManager)activity.GetSystemService(Context.InputMethodService);
            //Find the currently focused view, so we can grab the correct window token from it.
            var view = activity.CurrentFocus ?? new View(activity);
            //If no view currently has focus, create a new one, just so we can grab a window token from it
            imm.HideSoftInputFromWindow(view.WindowToken, 0);
        }
    }
}