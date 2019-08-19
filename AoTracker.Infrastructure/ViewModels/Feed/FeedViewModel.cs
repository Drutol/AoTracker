using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Models;
using AoTracker.Infrastructure.Models.Messages;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.ViewModels.Feed
{
    public class FeedViewModel : ViewModelBase
    {
        private readonly IUserDataProvider _userDataProvider;
        private ObservableCollection<FeedTabEntry> _feedTabEntries;

        public ObservableCollection<FeedTabEntry> FeedTabEntries
        {
            get => _feedTabEntries;
            set => Set(ref _feedTabEntries, value);
        }

        public FeedViewModel(IUserDataProvider userDataProvider)
        {
            _userDataProvider = userDataProvider;
            _userDataProvider.CrawlingSets.CollectionChanged += CrawlingSetsOnCollectionChanged;
            MessengerInstance.Register<CrawlerSetModifiedMessage>(this, OnCrawlerModified);
        }

        private void OnCrawlerModified(CrawlerSetModifiedMessage set)
        {
            var feedItem = GetRelevantFeedEntry(set.ModifiedCrawlerSet);
            feedItem.CrawlerSets = new List<CrawlerSet> {set.ModifiedCrawlerSet};
        }

        private void CrawlingSetsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var addedSet = e.NewItems.Cast<CrawlerSet>().ToList();
                    _feedTabEntries.Add(new FeedTabEntry(addedSet)
                    {
                        Name = addedSet.First().Name
                    });
                    break;
                case NotifyCollectionChangedAction.Move:
                    var movedSet = e.NewItems.Cast<CrawlerSet>().First();
                    var item = GetRelevantFeedEntry(movedSet);
                    var itemIndex = _feedTabEntries.IndexOf(item);
                    _feedTabEntries.Move(itemIndex, e.NewStartingIndex + 1);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    var removedSet = e.NewItems.Cast<CrawlerSet>().First();
                    var removedTab = GetRelevantFeedEntry(removedSet);
                    _feedTabEntries.Remove(removedTab);
                    break;
            }
        }

        public void NavigatedTo()
        {
            if (FeedTabEntries?.Any() ?? false)
                return;

            var entries = new List<FeedTabEntry>(0);
            if (_userDataProvider.CrawlingSets.Count > 1)
            {
                entries.Add(
                    new FeedTabEntry(_userDataProvider.CrawlingSets.Where(set => set.Descriptors.Any()).ToList())
                    {
                        Name = "All"
                    });
            }

            foreach (var crawlerSet in _userDataProvider.CrawlingSets.Where(set => set.Descriptors.Any()).Take(5))
            {
                entries.Add(new FeedTabEntry(new List<CrawlerSet> {crawlerSet})
                {
                    Name = crawlerSet.Name
                });
            }

            FeedTabEntries = new ObservableCollection<FeedTabEntry>(entries);
        }

        private FeedTabEntry GetRelevantFeedEntry(CrawlerSet set)
        {
            return _feedTabEntries.First(entry => entry.CrawlerSets.Count == 1 && entry.CrawlerSets[0] == set);
        }
    }
}
