using System;
using System.Collections.Generic;
using System.Text;

namespace AoTracker.Crawlers.Interfaces
{
    public interface ICrawlerManager
    {
        void InitializeCrawlers(IHttpClientProvider httpClientProvider);

        ICrawler<T> GetCrawler<T>() where T : ICrawlerResultItem;
    }
}
