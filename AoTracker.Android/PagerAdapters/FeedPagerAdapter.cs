using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using AoTracker.Android.Fragments.Feed;
using AoTracker.Infrastructure.Models;
using AoTracker.Infrastructure.Statics;
using Java.Lang;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace AoTracker.Android.PagerAdapters
{
    public class FeedPagerAdapter : FragmentStatePagerAdapter
    {
        private readonly List<FeedTabEntry> _tabEntries;

        public List<FeedPageTabFragment> Fragments { get; } = new List<FeedPageTabFragment>();

        public FeedPagerAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public FeedPagerAdapter(FragmentManager fm, List<FeedTabEntry> tabEntries) : base(fm)
        {
            _tabEntries = tabEntries;

            foreach (var entry in tabEntries)
            {
                Fragments.Add(new FeedPageTabFragment(entry));
            }
        }

        public override int Count => _tabEntries.Count;

        public override Fragment GetItem(int position)
        {
            return Fragments[position];
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(_tabEntries[position].Name);
        }
    }
}