using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Models;

namespace AoTracker.Interfaces
{
    public interface IFeedHistoryProvider
    {
        Task<List<HistoryFeedEntry>> GetHistory(CrawlerSet set);
        Task UpdateHistory(CrawlerSet set, List<HistoryFeedEntry> history);
        Task<bool> HasAnyChanged(CrawlerSet batchSetOfOrigin, IEnumerable<ICrawlerResultItem> crawlerResultResults);
    }
}
