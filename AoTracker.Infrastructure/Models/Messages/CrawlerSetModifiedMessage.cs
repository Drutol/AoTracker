using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Models;

namespace AoTracker.Infrastructure.Models.Messages
{
    public class CrawlerSetModifiedMessage
    {
        public CrawlerSetModifiedMessage(CrawlerSet set)
        {
            ModifiedCrawlerSet = set;
        }

        public CrawlerSet ModifiedCrawlerSet { get; set; }
    }
}
