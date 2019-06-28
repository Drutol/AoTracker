namespace AoTracker.Crawlers.Interfaces
{
    public interface ICrawlerResultItem
    {
        string Id { get; }
        string Name { get; }
        string ImageUrl { get; }
        float Price { get; }
        
    }
}
