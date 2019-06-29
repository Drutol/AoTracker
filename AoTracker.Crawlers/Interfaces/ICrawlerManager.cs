using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;

namespace AoTracker.Crawlers.Interfaces
{
    public interface ICrawlerManager
    {
        void InitializeCrawlers(IHttpClientProvider httpClientProvider);

        ICrawler<T> GetCrawler<T>() where T : ICrawlerResultItem;
        ICrawler GetCrawler(CrawlerDomain crawlerDomain);
    }
}
