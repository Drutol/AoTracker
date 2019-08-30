using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Surugaya;
using Microsoft.Extensions.Logging;

namespace AoTracker.Crawlers.Sites.Mercari
{
    public class MercariCrawler : CrawlerBase<MercariItem>
    {
        public MercariCrawler(IHttpClientProvider httpClientProvider, ILoggerFactory loggerFactory)
        {
            Domain = CrawlerDomain.Mercari;
            Parser = new MercariParser(loggerFactory.CreateLogger<MercariParser>());
            Source = new MercariSource(httpClientProvider);
            Cache = new CrawlerCache<MercariItem>();
        }
    }
}
