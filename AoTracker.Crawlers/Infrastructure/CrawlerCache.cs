using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Infrastructure
{
    public class CrawlerCache<T> : ICrawlerCache<T> where T : ICrawlerResultItem
    {
        private Dictionary<int,List<T>> _cache = new Dictionary<int, List<T>>();
        private Dictionary<int, DateTime> _cacheTimes = new Dictionary<int, DateTime>();

        public CacheResult<T> Get(CrawlerParameters parameters)
        {
            return new CacheResult<T>()
            {
                Cache = _cache[parameters.VolatileParameters.Page],
                CacheTime = _cacheTimes[parameters.VolatileParameters.Page]
            };
        }

        public CacheResult<T> GetAll()
        {
            return new CacheResult<T>()
            {
                Cache = _cache.SelectMany(pair => pair.Value).ToList(),
                CacheTime = _cacheTimes.Min(pair => pair.Value)
            };
        }

        public void Set(IEnumerable<T> items, CrawlerParameters parameters)
        {
            _cache[parameters.VolatileParameters.Page] = new List<T>(items);
            _cacheTimes[parameters.VolatileParameters.Page] = DateTime.UtcNow;
        }

        public bool IsCached(CrawlerParameters parameters)
        {
            return _cache.ContainsKey(parameters.VolatileParameters.Page);
        }

        public void Clear()
        {
            _cache.Clear();
        }
    }
}
