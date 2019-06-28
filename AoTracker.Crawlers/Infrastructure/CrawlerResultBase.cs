using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Infrastructure
{
    public class CrawlerResultBase<T> : ICrawlerResult<T> where T : ICrawlerResultItem
    {
        public bool Success { get; set; }
        public IEnumerable<T> Results { get; set; }
    }
}
