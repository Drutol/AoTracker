using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Sites.Mercari
{
    public class MercariItem : ICrawlerResultItem
    {
        public CrawlerDomain Domain { get; } = CrawlerDomain.Mercari;
        public string Id { get; set; }
        public string InternalId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public float Price { get; set; }
    }
}
