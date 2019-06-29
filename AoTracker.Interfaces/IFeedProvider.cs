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

        void StartAggregating(CancellationToken feedCtsToken);
    }
}
