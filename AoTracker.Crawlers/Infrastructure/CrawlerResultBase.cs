using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Infrastructure
{
    public class CrawlerResultBase<T> :
        ICrawlerResultList<T>,
        ICrawlerResultSingle<T> where T : ICrawlerResultItem
    {
        public bool Success { get; set; }
        public IEnumerable<T> Results { get; set; }

        public bool IsCached { get; set; }
        public bool HasMore { get; set; }
        public DateTime? CacheTime { get; set; }
        public T Result { get; set; }

        public static CrawlerResultBase<T> FromCache(CacheResult<T> cache)
        {
            return new CrawlerResultBase<T>
            {
                Results = cache.Cache,
                Result = cache.Cache.Count == 1 ? cache.Cache.First() : default,
                IsCached = true,
                CacheTime = cache.CacheTime,
                Success = true
            };
        }
    }
}
