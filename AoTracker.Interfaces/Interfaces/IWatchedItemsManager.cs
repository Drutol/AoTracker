using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Models;

namespace AoTracker.Interfaces
{
    public interface IWatchedItemsManager : IInitializable
    {
        event EventHandler<WatchedItemDataEntry> ItemDetailsFetched;

        List<WatchedItemDataEntry> Entries { get; }

        void AddWatchedEntry(ICrawlerResultItem entry);
        void RemoveWatchedEntry(WatchedItemDataEntry entry);
        void RemoveWatchedEntry(ICrawlerResultItem backingModel);

        void StartAggregatingWatchedItemsData();
        void RequestSingleItemUpdate(WatchedItemDataEntry entry);
        bool IsWatched(ICrawlerResultItem item);
    }
}
