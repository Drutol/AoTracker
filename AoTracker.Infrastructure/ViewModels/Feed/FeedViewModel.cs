using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoLibs.Utilities.Shared;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Messaging;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Models;
using AoTracker.Infrastructure.Models.Messages;
using AoTracker.Interfaces;
using AoTracker.Resources;

namespace AoTracker.Infrastructure.ViewModels.Feed
{
    public class FeedViewModel : ViewModelBase
    {
        public enum Message
        {
            ShowJumpToActionButton,
            HideJumpToActionButton
        }

        private readonly IUserDataProvider _userDataProvider;
        private readonly ISettings _settings;
        private ObservableCollection<FeedTabEntry> _feedTabEntries;
        private bool _jumpToButtonVisibility = true;
        private bool? _lastAggregateSetting;

        public override PageIndex PageIdentifier { get; } = PageIndex.Feed;

        public bool ContainsAggregate { get; set; }

        public FeedViewModel(
            IUserDataProvider userDataProvider, 
            ISettings settings)
        {
            _userDataProvider = userDataProvider;
            _settings = settings;
            _userDataProvider.CrawlingSets.CollectionChanged += CrawlingSetsOnCollectionChanged;
            MessengerInstance.Register<CrawlerSetModifiedMessage>(this, OnCrawlerModified);

            PageTitle = AppResources.PageTitle_Feed;

            MessengerInstance.Register<Message>(this, OnMessage);
        }

        private void OnCrawlerModified(CrawlerSetModifiedMessage message)
        {
            if (!message.FavouriteChanged)
            {
                var feedItem = GetRelevantFeedEntry(message.ModifiedCrawlerSet);
                if (feedItem != null)
                {
                    feedItem.CrawlerSets = new List<CrawlerSet> { message.ModifiedCrawlerSet };

                    if (!feedItem.CrawlerSets.Any(set => set.Descriptors.Any()))
                        FeedTabEntries.Remove(feedItem);
                }
            }
            else if (message.FavouriteChanged)
            {
                if(!_settings.GenerateFeedAggregate)
                    return;

                var favourites = _userDataProvider.CrawlingSets.Where(set => set.IsFavourite).ToList();

                if (ContainsAggregate)
                {
                    if (!favourites.Any())
                    {
                        ContainsAggregate = false;
                        FeedTabEntries.RemoveAt(0);
                    }
                    else
                    {
                        var diff = favourites.Diff(_feedTabEntries[0].CrawlerSets,
                            (set, crawlerSet) => set.Guid == crawlerSet.Guid);
                        if (diff.Added.Any() || diff.Removed.Any())
                        {
                            _feedTabEntries[0].CrawlerSets = favourites;
                        }
                    }

                }
                else if (favourites.Count > 1)
                {
                    var tab = BuildAggregateTab();
                    if (tab.CrawlerSets?.Any() ?? false)
                    {
                        ContainsAggregate = true;
                        FeedTabEntries.Insert(0, tab);
                    }
                }
            }
        }

        public ObservableCollection<FeedTabEntry> FeedTabEntries
        {
            get => _feedTabEntries;
            set => Set(ref _feedTabEntries, value);
        }

        public bool JumpToButtonVisibility
        {
            get => _jumpToButtonVisibility;
            set => Set(ref _jumpToButtonVisibility, value);
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
                    var removedSet = e.OldItems.Cast<CrawlerSet>().First();
                    var removedTab = GetRelevantFeedEntry(removedSet);
                    _feedTabEntries.Remove(removedTab);
                    break;
            }
        }

        public void NavigatedTo()
        {
            MessengerInstance.Send(ToolbarRequestMessage.ShowSearchInterface);

            if (FeedTabEntries?.Any() ?? false)
            {
                // we have pages generated so now let's just check for changes

                // first whether we still generate aggregate
                if (_lastAggregateSetting != _settings.GenerateFeedAggregate)
                {
                    if (_settings.GenerateFeedAggregate && !ContainsAggregate)
                    {
                        if (_userDataProvider.CrawlingSets.Count(set => set.IsFavourite) > 1)
                        {
                            var aggregateTab = BuildAggregateTab();
                            if (aggregateTab.CrawlerSets?.Any() ?? false)
                            {
                                ContainsAggregate = true;
                                FeedTabEntries.Insert(0, aggregateTab);
                            }
                        }
                    }
                    else if (!_settings.GenerateFeedAggregate && ContainsAggregate)
                    {
                        ContainsAggregate = false;
                        FeedTabEntries.RemoveAt(0);
                    }
                }
                _lastAggregateSetting = _settings.GenerateFeedAggregate;

                return;
            }

            var entries = new List<FeedTabEntry>(0);
            if (_userDataProvider.CrawlingSets.Count(set => set.IsFavourite) > 1 && _settings.GenerateFeedAggregate)
            {
                var aggregateTab = BuildAggregateTab();

                if (aggregateTab.CrawlerSets?.Any() ?? false)
                {
                    entries.Add(aggregateTab);
                    ContainsAggregate = true;
                }
            }

            foreach (var crawlerSet in _userDataProvider.CrawlingSets.Where(set => set.Descriptors.Any()).Take(5))
            {
                entries.Add(new FeedTabEntry(new List<CrawlerSet> {crawlerSet})
                {
                    Name = crawlerSet.Name
                });
            }

            FeedTabEntries = new ObservableCollection<FeedTabEntry>(entries);

            if (FeedTabEntries.Count <= 2)
                JumpToButtonVisibility = false;

        }

        public void NavigatedBack()
        {
            HideSearchInterface();
        }

        public void NavigatedFrom()
        {
            HideSearchInterface();
        }

        private void HideSearchInterface()
        {
            MessengerInstance.Send(ToolbarRequestMessage.ResetToolbar);
        }

        private FeedTabEntry BuildAggregateTab()
        {
            return new FeedTabEntry(_userDataProvider.CrawlingSets
                .Where(set => set.Descriptors.Any() && set.IsFavourite).ToList())
            {
                Name = "All"
            };
        }

        private void OnMessage(Message message)
        {
            if(FeedTabEntries.Count <= 2)
                return;

            if (message == Message.ShowJumpToActionButton)
                JumpToButtonVisibility = true;
            else if (message == Message.HideJumpToActionButton)
                JumpToButtonVisibility = false;
        }


        private FeedTabEntry GetRelevantFeedEntry(CrawlerSet set)
        {
            return _feedTabEntries.FirstOrDefault(entry => entry.CrawlerSets.Count == 1 && entry.CrawlerSets[0] == set);
        }
    }
}
