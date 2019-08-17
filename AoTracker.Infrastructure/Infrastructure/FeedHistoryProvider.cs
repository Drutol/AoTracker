using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Interfaces;
using AoTracker.Domain.Models;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.Infrastructure
{
    public class FeedHistoryProvider : IFeedHistoryProvider
    {
        private const string HistoryDirectory = "history";

        private readonly IDataCache _dataCache;

        private readonly Dictionary<Guid, List<HistoryFeedEntry>> _historyDictionary =
            new Dictionary<Guid, List<HistoryFeedEntry>>();

        public FeedHistoryProvider(IDataCache dataCache)
        {
            _dataCache = dataCache;
        }

        public async Task<List<HistoryFeedEntry>> GetHistory(CrawlerSet set)
        {
            if (_historyDictionary.TryGetValue(set.Guid, out var history))
                return history;

            history = await _dataCache.RetrieveData<List<HistoryFeedEntry>>($"{HistoryDirectory}/{set.Guid}");

            if (history != null)
                _historyDictionary[set.Guid] = history;

            return history;
        }

        public async Task UpdateHistory(CrawlerSet set, List<HistoryFeedEntry> history)
        {
            _historyDictionary[set.Guid] = history;
            await _dataCache.SaveDataAsync($"{HistoryDirectory}/{set.Guid}", history);
        }
    }
}
