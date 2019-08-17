using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Surugaya;

namespace AoTracker.Crawlers.Sites.Mercari
{
    public class MercariCrawler : CrawlerBase<MercariItem>
    {
        public MercariCrawler(IHttpClientProvider httpClientProvider)
        {
            Domain = CrawlerDomain.Mercari;
            Parser = new MercariParser();
            Source = new MercariSource(httpClientProvider);
            Cache = new CrawlerCache<MercariItem>();
        }
    }
}
