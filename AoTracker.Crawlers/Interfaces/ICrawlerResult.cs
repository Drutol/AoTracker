using System;
using System.Collections.Generic;

namespace AoTracker.Crawlers.Interfaces
{
    public interface ICrawlerResult
    {
        bool Success { get; }

        bool IsCached { get; }
        bool HasMore { get; }
        DateTime? CacheTime { get; set; }
    }

    public interface ICrawlerResultList<out T> : ICrawlerResult where T : ICrawlerResultItem
    {
        IEnumerable<T> Results { get; }
    }

    public interface ICrawlerResultSingle<out T> : ICrawlerResult where T : ICrawlerResultItem
    {
        T Result { get; }
    }
}
