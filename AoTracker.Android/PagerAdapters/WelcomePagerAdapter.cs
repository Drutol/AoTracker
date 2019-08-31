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
using AoTracker.Android.Fragments.Welcome;
using AoTracker.Domain.Models;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace AoTracker.Android.PagerAdapters
{
    public class WelcomePagerAdapter : FragmentPagerAdapter
    {
        private readonly List<WelcomeTabEntry> _entries;

        private List<WelcomePageTabFragment> _fragments;

        public WelcomePagerAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public WelcomePagerAdapter(FragmentManager fm, List<WelcomeTabEntry> entries) : base(fm)
        {
            _entries = entries;

            _fragments = entries.Select(entry => new WelcomePageTabFragment(entry)).ToList();
        }

        public override int Count => _entries.Count;

        public override Fragment GetItem(int position)
        {
            return _fragments[position];
        }
    }
}