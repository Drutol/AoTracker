using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoTracker.Domain.Models;

namespace AoTracker.Infrastructure.Models
{
    public class FeedTabEntry
    {
        public string Name { get; set; }
        public List<CrawlerSet> CrawlerSets { get; set; }

        public FeedTabEntry(List<CrawlerSet> crawlerSets)
        {
            CrawlerSets = crawlerSets;
        }
    }
}
