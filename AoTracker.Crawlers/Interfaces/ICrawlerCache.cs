using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Infrastructure;

namespace AoTracker.Crawlers.Interfaces
{
    public interface ICrawlerCache<T> where T : ICrawlerResultItem
    {
        CacheResult<T> Get(CrawlerParameters parameters);
        CacheResult<T> Get(string id);
        CacheResult<T> GetAll();

        void Set(IEnumerable<T> items, CrawlerParameters parameters);
        void Set(T item, string id);

        bool IsCached(CrawlerParameters parameters);
        bool IsCached(string id);

        void Clear();
    }
}
