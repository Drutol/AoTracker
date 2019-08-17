using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Sites.Yahoo
{
    class YahooCrawler : CrawlerBase<YahooItem>
    {
        public YahooCrawler(IHttpClientProvider httpClientProvider)
        {
            Domain = CrawlerDomain.Yahoo;
            Parser = new YahooParser();
            Source = new YahooSource(httpClientProvider);
            Cache = new CrawlerCache<YahooItem>();
        }
    }
}
