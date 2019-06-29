using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Infrastructure
{
    public class CacheResult<T> where T : ICrawlerResultItem
    {
        public DateTime CacheTime { get; set; }
        public List<T> Cache { get; set; }
    }
}
