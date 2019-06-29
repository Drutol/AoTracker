using System;
using System.Collections.Generic;

namespace AoTracker.Crawlers.Interfaces
{
    public interface ICrawlerResult<out T> where T : ICrawlerResultItem
    {
        bool Success { get; }

        bool IsCached { get; }
        DateTime? CacheTime { get; set; }

        IEnumerable<T> Results { get; }
    }
}
