using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
using Object = Java.Lang.Object;

namespace AoTracker.Android.PagerAdapters
{
    public class FeedPagerAdapter : FragmentPagerAdapter
    {
        private readonly ObservableCollection<FeedTabEntry> _tabEntries;
        private readonly FeedPageFragment _parent;

        public List<FeedPageTabFragment> Fragments { get; private set; } = new List<FeedPageTabFragment>();

        public FeedPagerAdapter(FragmentManager fm, ObservableCollection<FeedTabEntry> tabEntries, FeedPageFragment parent) : base(fm)
        {
            _tabEntries = tabEntries;
            _parent = parent;
            _tabEntries.CollectionChanged += TabEntriesOnCollectionChanged;

            foreach (var entry in tabEntries)
            {
                entry.ResetEventSubscriptions();
                Fragments.Add(new FeedPageTabFragment(entry));
            }

            Fragments.FirstOrDefault()?.NavigatedTo();
        }

        public FeedPagerAdapter(FragmentManager fm, ObservableCollection<FeedTabEntry> tabEntries, List<FeedPageTabFragment> fragments, FeedPageFragment parent) : base(fm)
        {
            _tabEntries = tabEntries;
            _parent = parent;
            _tabEntries.CollectionChanged += TabEntriesOnCollectionChanged;

            Fragments = fragments;
            Fragments.FirstOrDefault()?.NavigatedTo();
        }

        private void TabEntriesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var newEntry = e.NewItems[0] as FeedTabEntry;
                    Fragments.Insert(e.NewStartingIndex, new FeedPageTabFragment(newEntry));
                    if(e.NewStartingIndex == _parent.ViewPager.CurrentItem)
                        Fragments.First().NavigatedTo();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    var removedEntry = e.OldItems[0] as FeedTabEntry;
                    var matchingFragment = Fragments.FirstOrDefault(fragment => fragment.TabEntry == removedEntry);
                    if (matchingFragment != null)
                    {
                        var index = Fragments.IndexOf(matchingFragment);
                        Fragments.Remove(Fragments[index]);
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    var movedEntry = e.NewItems[0] as FeedTabEntry;
                    var targetIndex = e.NewStartingIndex;
                    var fragmentIndex = Fragments.IndexOf(Fragments.First(fragment => fragment.TabEntry == movedEntry));
                    var movedFragment = Fragments[fragmentIndex];
                    Fragments.RemoveAt(fragmentIndex);
                    Fragments.Insert(targetIndex, movedFragment);
                    break;
            }

            NotifyDataSetChanged();
            _parent.UpdateTabIcons();
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

        public FeedPagerAdapter Duplicate(FragmentManager childFragmentManager)
        {
            _tabEntries.CollectionChanged -= TabEntriesOnCollectionChanged;
            return new FeedPagerAdapter(childFragmentManager, _tabEntries, Fragments, _parent);
        }
    }
}