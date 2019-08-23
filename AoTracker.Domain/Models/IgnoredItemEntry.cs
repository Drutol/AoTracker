using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;

namespace AoTracker.Infrastructure.Models
{
    public class IgnoredItemEntry
    {
        public CrawlerDomain Domain { get; set; }
        public string InternalId { get; set; }
        public string Name { get; set; }
    }
}
