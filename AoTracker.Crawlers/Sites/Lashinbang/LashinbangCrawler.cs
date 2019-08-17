using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Sites.Yahoo;

namespace AoTracker.Crawlers.Sites.Lashinbang
{
    public class LashinbangCrawler : CrawlerBase<LashinbangItem>
    {
        public LashinbangCrawler(IHttpClientProvider httpClientProvider)
        {
            Domain = CrawlerDomain.Yahoo;
            Parser = new LashinbangParser();
            Source = new LashinbangSource(httpClientProvider);
            Cache = new CrawlerCache<LashinbangItem>();
        }
    }
}
