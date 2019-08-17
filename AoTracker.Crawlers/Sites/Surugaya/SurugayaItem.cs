using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Surugaya
{
    public class SurugayaItem : ICrawlerResultItem
    {
        public CrawlerDomain Domain { get; } = CrawlerDomain.Surugaya;
        public string Id { get; set; }
        public string InternalId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public float Price { get; set; }

        public string Category { get; set; }
        public string Brand { get; set; }
    }
}
