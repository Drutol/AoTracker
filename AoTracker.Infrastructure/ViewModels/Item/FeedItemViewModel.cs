using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly FeedTabViewModel _parent;

        public FeedItemViewModel(ICrawlerResultItem item, FeedTabViewModel parent) : base(item)
        {
            _parent = parent;
        }

        public bool IsNew { get; private set; }
        public float PriceDifference { get; set; }
        public PriceChange PriceChange { get; private set; }
        public DateTime LastChanged { get; private set; }
        public CrawlerSet SetOfOrigin { get; private set; }

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

        public FeedItemViewModel(ICrawlerResultItem item, FeedTabViewModel parent) : base(item, parent)
        {
            Item = (T) item;
        }
    }
}
