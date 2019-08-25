using AoTracker.Crawlers.Enums;

namespace AoTracker.Crawlers.Interfaces
{
    public interface ICrawlerResultItem
    {
        CrawlerDomain Domain { get; }

        string Id { get; set; }
        string InternalId { get; set; }
        string Name { get; set; }
        string ImageUrl { get; set; }
        float Price { get; set; }
    }
}
