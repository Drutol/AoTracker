using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoLibs.Adapters.Core.Interfaces;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Models;
using AoTracker.Infrastructure.ViewModels.Feed;
using AoTracker.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels.Item
{
    public class FeedItemViewModel : ItemViewModelBase<ICrawlerResultItem>, IMerchItem
    {
        private readonly IDomainLinkHandlerManager _domainLinkHandlerManager;
        private readonly IUriLauncherAdapter _launcherAdapter;
        private readonly IIgnoredItemsManager _ignoredItemsManager;
        private readonly IWatchedItemsManager _watchedItemsManager;
        private readonly FeedTabViewModel _parent;

        public ICrawlerResultItem Item => BackingModel;

        public FeedItemViewModel(
            IDomainLinkHandlerManager domainLinkHandlerManager,
            IUriLauncherAdapter launcherAdapter,
            IIgnoredItemsManager ignoredItemsManager,
            IWatchedItemsManager watchedItemsManager,
            ICrawlerResultItem item,
            FeedTabViewModel parent) : base(item)
        {
            _domainLinkHandlerManager = domainLinkHandlerManager;
            _launcherAdapter = launcherAdapter;
            _ignoredItemsManager = ignoredItemsManager;
            _watchedItemsManager = watchedItemsManager;
            _parent = parent;
        }

        public bool IsNew { get; private set; }
        public float PriceDifference { get; set; }
        public PriceChange PriceChange { get; private set; }
        public DateTime LastChanged { get; private set; }
        public CrawlerSet SetOfOrigin { get; private set; }

        public float PreviousPrice { get; set; }

        public RelayCommand NavigateItemWebsiteCommand => new RelayCommand(() =>
        {
            _launcherAdapter.LaunchUri(new Uri(_domainLinkHandlerManager.GenerateWebsiteLink(BackingModel)));
        });

        public RelayCommand IgnoreItemCommand => new RelayCommand(() =>
        {
            _ignoredItemsManager.AddIgnoredItem(BackingModel);
            _parent.RemoveItem(this);
        });

        public bool IsWatched => _watchedItemsManager.IsWatched(BackingModel);

        public RelayCommand WatchItemCommand => new RelayCommand(() =>
        {
            _watchedItemsManager.AddWatchedEntry(BackingModel);
        });

        public RelayCommand UnwatchItemCommand => new RelayCommand(() =>
        {
            _watchedItemsManager.RemoveWatchedEntry(BackingModel);
        });

        public void WithHistory(List<HistoryFeedEntry> feedHistory, CrawlerSet setOfOrigin)
        {
            SetOfOrigin = setOfOrigin;

            if (feedHistory == null)
            {
                LastChanged = DateTime.UtcNow;
                return;
            }

            var historyEntry = feedHistory.FirstOrDefault(entry => entry.InternalId == BackingModel.InternalId);
            if (historyEntry == null)
            {
                IsNew = true;
                LastChanged = DateTime.UtcNow;
                PreviousPrice = BackingModel.Price;
            }
            else
            {
                float priceToCompare;
                if (Math.Abs(BackingModel.Price - historyEntry.LatestPrice) < 0.001)
                {
                    LastChanged = historyEntry.LastChanged;
                    PreviousPrice = historyEntry.PreviousPrice;
                    priceToCompare = historyEntry.PreviousPrice;
                }
                else
                {
                    LastChanged = DateTime.UtcNow;
                    PreviousPrice = historyEntry.LatestPrice;
                    priceToCompare = historyEntry.LatestPrice;
                }

                if (BackingModel.Price > priceToCompare)
                {
                    PriceChange = PriceChange.Increase;
                }
                else if (BackingModel.Price < priceToCompare)
                {
                    PriceChange = PriceChange.Decrease;
                }
                else
                {
                    PriceChange = PriceChange.Stale;
                }

                if (PriceChange != PriceChange.Stale)
                {
                    PriceDifference = BackingModel.Price - priceToCompare;
                }
            }
        }

        public HistoryFeedEntry BuildHistoryEntry()
        {
            return new HistoryFeedEntry
            {
                InternalId = BackingModel.InternalId,
                LastChanged = LastChanged,
                LatestPrice = BackingModel.Price,
                PreviousPrice = PreviousPrice
            };
        }


    }

    public class FeedItemViewModel<T> : FeedItemViewModel where T : ICrawlerResultItem
    {
        public T Item { get; }


        public FeedItemViewModel(IDomainLinkHandlerManager domainLinkHandlerManager,
            IUriLauncherAdapter launcherAdapter, IIgnoredItemsManager ignoredItemsManager,
            IWatchedItemsManager watchedItemsManager, ICrawlerResultItem item, FeedTabViewModel parent) : base(
            domainLinkHandlerManager, launcherAdapter, ignoredItemsManager, watchedItemsManager, item, parent)
        {
            Item = (T) item;
        }
    }
}
