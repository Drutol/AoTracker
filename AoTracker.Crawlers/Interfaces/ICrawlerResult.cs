using System.Collections.Generic;

namespace AoTracker.Crawlers.Interfaces
{
    public interface ICrawlerResult<out T> where T : ICrawlerResultItem
    {
        bool Success { get; }

        IEnumerable<T> Results { get; }
    }
}
