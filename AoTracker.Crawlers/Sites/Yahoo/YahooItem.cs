using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Sites.Yahoo
{
    public class YahooItem : ICrawlerResultItem
    {
        public enum ItemCondition
        {
            Unknown,
            Used,
            New
        }

        public CrawlerDomain Domain { get; } = CrawlerDomain.Yahoo;
        public string Id { get; set; }
        public string InternalId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public float Price { get; set; }

        public ItemCondition Condition { get; set; }
        public int BuyoutPrice { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public bool IsShippingFree { get; set; }
        public int Tax { get; set; }
        public int BidsCount { get; set; }
    }
}
