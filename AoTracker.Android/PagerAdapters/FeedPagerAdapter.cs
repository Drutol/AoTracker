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
                Fragments.Add(new FeedPageTabFragment(entry));
            }
        }

        private void TabEntriesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var newEntry = e.NewItems[0] as FeedTabEntry;
                    Fragments.Add(new FeedPageTabFragment(newEntry));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    var removedEntry = e.NewItems[0] as FeedTabEntry;
                    var index = _tabEntries.IndexOf(removedEntry);
                    Fragments.Remove(Fragments[index]);
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
            return new FeedPagerAdapter(childFragmentManager, _tabEntries, _parent)
            {
                Fragments = Fragments
            };
        }
    }
}