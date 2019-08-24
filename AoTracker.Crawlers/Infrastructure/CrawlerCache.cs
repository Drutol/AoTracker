using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Infrastructure
{
    public class CrawlerCache<T> : ICrawlerCache<T> where T : ICrawlerResultItem
    {
        private Dictionary<string,T> _detailCacheDictionary = new Dictionary<string, T>();
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

        public CacheResult<T> Get(string id)
        {
            if (!_detailCacheDictionary.TryGetValue(id, out var item))
            {
                item = _cache.SelectMany(pair => pair.Value).FirstOrDefault(i => i.Id.Equals(id));
                _detailCacheDictionary[id] = item;
            }

            return new CacheResult<T>()
            {
                Cache = new List<T>
                {
                    item
                }
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

        public void Set(T item, string id)
        {
            _detailCacheDictionary[id] = item;
        }

        public bool IsCached(CrawlerParameters parameters)
        {
            return _cache.ContainsKey(ToKey(parameters));
        }

        public bool IsCached(string id)
        {
            return _detailCacheDictionary.ContainsKey(id) ||
                   _cache.Any(pair => pair.Value.FirstOrDefault(item => item.Id.Equals(id)) != null);
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
