using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;

namespace AoTracker.Infrastructure.Models
{
    public class CrawlerEntry
    {
        public CrawlerDomain CrawlerDomain { get; set; }
        public string Title { get; set; }
    }
}
