using System;
using AoTracker.Crawlers.Enums;

namespace AoTracker.Domain.Models
{
    public class IgnoredItemEntry
    {
        public CrawlerDomain Domain { get; set; }
        public string InternalId { get; set; }
        public string Name { get; set; }
        public DateTime IgnoredAt { get; set; }
        public string ImageUrl { get; set; }
    }
}
