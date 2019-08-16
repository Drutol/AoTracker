using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Interfaces
{
    public interface IFeedProvider
    {
        event EventHandler<IEnumerable<ICrawlerResultItem>> NewCrawlerBatch;
        event EventHandler Finished;

        List<ICrawlerResultItem> CachedFeed { get; }

        void StartAggregating(CancellationToken feedCtsToken, bool force);
    }
}
