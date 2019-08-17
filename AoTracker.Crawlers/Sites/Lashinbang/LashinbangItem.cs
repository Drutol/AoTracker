using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Sites.Lashinbang
{
    public class LashinbangItem : ICrawlerResultItem
    {
        public CrawlerDomain Domain { get; } = CrawlerDomain.Lashinbang;
        public string Id { get; set; }
        public string InternalId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public float Price { get; set; }
    }
}
