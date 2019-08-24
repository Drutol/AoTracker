using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Infrastructure;

namespace AoTracker.Crawlers.Interfaces
{
    public interface ICrawler
    {
        CrawlerDomain Domain { get; }
        ICrawlerSource Source { get; }

        Task<ICrawlerResultList<ICrawlerResultItem>> Crawl(CrawlerParameters parameters);
        Task<ICrawlerResultSingle<ICrawlerResultItem>> CrawlById(string id);

        bool IsCached(CrawlerParameters parameters);
    }

    public interface ICrawler<T> : ICrawler where T : ICrawlerResultItem
    {
        ICrawlerParser<T> Parser { get; set; }
        ICrawlerCache<T> Cache { get; set; }

        new Task<ICrawlerResultList<T>> Crawl(CrawlerParameters parameters);
        new Task<ICrawlerResultSingle<T>> CrawlById(string id);
    }
}
