using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Models;

namespace AoTracker.Infrastructure.Models.NavArgs
{
    public class CrawlerSetDetailsPageNavArgs
    {
        public CrawlerSetDetailsPageNavArgs(CrawlerSet set)
        {
            CrawlerSet = set;
        }

        public CrawlerSet CrawlerSet { get; set; }
    }
}
