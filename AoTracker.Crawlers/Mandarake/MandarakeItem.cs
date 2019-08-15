using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Mandarake
{
    public class MandarakeItem : ICrawlerResultItem
    {
        public CrawlerDomain Domain { get; set; }
        public string Id { get; set; }
        public string InternalId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public float Price { get; set; }

        public string Shop { get; set; }
    }
}
