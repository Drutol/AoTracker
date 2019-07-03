using AoTracker.Crawlers.Enums;

namespace AoTracker.Crawlers.Interfaces
{
    public interface ICrawlerResultItem
    {
        CrawlerDomain Domain { get; }

        string Id { get; }
        string Name { get; }
        string ImageUrl { get; }
        float Price { get; }
    }
}
