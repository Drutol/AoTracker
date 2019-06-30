using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Infrastructure;

namespace AoTracker.Crawlers.Interfaces
{
    public interface ICrawlerCache<T> where T : ICrawlerResultItem
    {
        CacheResult<T> Get(CrawlerParameters parameters);
        CacheResult<T> GetAll();

        void Set(IEnumerable<T> items, CrawlerParameters parameters);
        bool IsCached(CrawlerParameters parameters);

        void Clear();
    }
}
