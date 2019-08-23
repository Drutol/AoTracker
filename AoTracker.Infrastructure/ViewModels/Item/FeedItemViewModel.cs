using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoLibs.Adapters.Core.Interfaces;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.ViewModels.Feed;
using AoTracker.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels.Item
{
    public class FeedItemViewModel : ItemViewModelBase<ICrawlerResultItem>, IFeedItem
    {
        private readonly IDomainLinkHandlerManager _domainLinkHandlerManager;
        private readonly IUriLauncherAdapter _launcherAdapter;
        private readonly IIgnoredItemsManager _ignoredItemsManager;
        private readonly FeedTabViewModel _parent;

        public FeedItemViewModel(
            IDomainLinkHandlerManager domainLinkHandlerManager,
            IUriLauncherAdapter launcherAdapter,
            IIgnoredItemsManager ignoredItemsManager,
            ICrawlerResultItem item,
            FeedTabViewModel parent) : base(item)
        {
            _domainLinkHandlerManager = domainLinkHandlerManager;
            _launcherAdapter = launcherAdapter;
            _ignoredItemsManager = ignoredItemsManager;
            _parent = parent;
        }

        public bool IsNew { get; private set; }
        public float PriceDifference { get; set; }
        public PriceChange PriceChange { get; private set; }
        public DateTime LastChanged { get; private set; }
        public CrawlerSet SetOfOrigin { get; private set; }

        public RelayCommand NavigateItemWebsiteCommand => new RelayCommand(() =>
        {
            _launcherAdapter.LaunchUri(new Uri(_domainLinkHandlerManager.GenerateWebsiteLink(BackingModel)));
        });

        public RelayCommand IgnoreItemCommand => new RelayCommand(() =>
        {
            _ignoredItemsManager.AddIgnoredItem(BackingModel);
            _parent.RemoveItem(this);
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
            }
            else
            {
                if (BackingModel.Price > historyEntry.PreviousPrice)
                {
                    PriceChange = PriceChange.Increase;
                }
                else if (BackingModel.Price < historyEntry.PreviousPrice)
                {
                    PriceChange = PriceChange.Decrease;
                }
                else
                {
                    PriceChange = PriceChange.Stale;
                }

                if (PriceChange != PriceChange.Stale)
                {
                    LastChanged = DateTime.UtcNow;
                    PriceDifference = BackingModel.Price - historyEntry.PreviousPrice;
                }
                else
                    LastChanged = historyEntry.LastChanged;
            }
        }

        public HistoryFeedEntry BuildHistoryEntry()
        {
            return new HistoryFeedEntry
            {
                InternalId = BackingModel.InternalId,
                LastChanged = LastChanged,
                PreviousPrice = BackingModel.Price
            };
        }
    }

    public class FeedItemViewModel<T> : FeedItemViewModel where T : ICrawlerResultItem
    {
        public T Item { get; }


        public FeedItemViewModel(IDomainLinkHandlerManager domainLinkHandlerManager, IUriLauncherAdapter launcherAdapter, IIgnoredItemsManager ignoredItemsManager, ICrawlerResultItem item, FeedTabViewModel parent) : base(domainLinkHandlerManager, launcherAdapter, ignoredItemsManager, item, parent)
        {
            Item = (T)item;
        }
    }
}
