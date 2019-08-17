using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Mandarake
{
    public class MandarakeCrawler : CrawlerBase<MandarakeItem>
    {
        public MandarakeCrawler(IHttpClientProvider httpClientProvider)
        {
            Domain = CrawlerDomain.Mandarake;
            Parser = new MandarakeParser();
            Source = new MandarakeSource(httpClientProvider);
            Cache = new CrawlerCache<MandarakeItem>();
        }
    }
}
