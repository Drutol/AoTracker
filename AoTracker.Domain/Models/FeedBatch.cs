using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Domain.Models
{
    public class FeedBatch
    {
        public ICrawlerResultList<ICrawlerResultItem> CrawlerResult { get; set; }
        public CrawlerSet SetOfOrigin { get; set; }
    }
}
