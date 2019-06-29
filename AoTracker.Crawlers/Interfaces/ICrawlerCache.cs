using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Infrastructure;

namespace AoTracker.Crawlers.Interfaces
{
    public interface ICrawlerCache<T> where T : ICrawlerResultItem
    {
        CacheResult<T> Get(ICrawlerSourceParameters parameters);
        CacheResult<T> GetAll();

        void Set(IEnumerable<T> items, ICrawlerSourceParameters parameters);
        bool IsCached(ICrawlerSourceParameters parameters);

        void Clear();
    }
}
