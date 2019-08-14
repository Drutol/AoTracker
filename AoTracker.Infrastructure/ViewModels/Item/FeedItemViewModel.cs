﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Models;
using AoTracker.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels.Item
{
    public class FeedItemViewModel : ItemViewModelBase<ICrawlerResultItem>, IFeedItem
    {
        private readonly FeedViewModel _parent;

        public FeedItemViewModel(ICrawlerResultItem item, FeedViewModel parent) : base(item)
        {
            _parent = parent;
        }

        public bool IsNew { get; set; }
        public PriceChange PriceChange { get; set; }

        public void WithHistory(List<HistoryFeedEntry> feedHistory)
        {
            if(feedHistory == null)
                return;

            var historyEntry = feedHistory.FirstOrDefault(entry => entry.InternalId == BackingModel.InternalId);

            if (historyEntry == null)
            {
                IsNew = true;
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
            }
        }
    }
}
