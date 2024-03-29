﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Models;

namespace AoTracker.Interfaces
{
    public interface IFeedProvider
    {
        event EventHandler<FeedBatch> NewCrawlerBatch;
        event EventHandler Finished;

        int StartAggregating(List<CrawlerSet> set, CancellationToken feedCtsToken, bool force);
        bool CheckCache(List<CrawlerSet> tabEntryCrawlerSets);
    }
}
