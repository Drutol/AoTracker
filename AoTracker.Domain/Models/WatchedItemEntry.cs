using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using AoTracker.Crawlers.Enums;

namespace AoTracker.Domain.Models
{
    public class WatchedItemEntry
    {
        public CrawlerDomain Domain { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string ImageUrl { get; set; }
    }
}
