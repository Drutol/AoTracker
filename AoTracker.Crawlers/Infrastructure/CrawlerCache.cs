using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Infrastructure
{
    public class CrawlerCache<T> : ICrawlerCache<T> where T : ICrawlerResultItem
    {
        private Dictionary<string,List<T>> _cache = new Dictionary<string, List<T>>();
        private Dictionary<string, DateTime> _cacheTimes = new Dictionary<string, DateTime>();

        public CacheResult<T> Get(CrawlerParameters parameters)
        {
            return new CacheResult<T>()
            {
                Cache = _cache[ToKey(parameters)],
                CacheTime = _cacheTimes[ToKey(parameters)]
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
            _cache[ToKey(parameters)] = new List<T>(items);
            _cacheTimes[ToKey(parameters)] = DateTime.UtcNow;
        }

        public bool IsCached(CrawlerParameters parameters)
        {
            return _cache.ContainsKey(ToKey(parameters));
        }

        public void Clear()
        {
            _cache.Clear();
        }

        private string ToKey(CrawlerParameters crawlerParameters)
        {
            return $"{crawlerParameters.Parameters.SearchQuery}_{crawlerParameters.VolatileParameters.Page}";
        }
    }
}
