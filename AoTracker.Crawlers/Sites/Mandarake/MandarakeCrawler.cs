using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using Microsoft.Extensions.Logging;

namespace AoTracker.Crawlers.Mandarake
{
    public class MandarakeCrawler : CrawlerBase<MandarakeItem>
    {
        public MandarakeCrawler(IHttpClientProvider httpClientProvider, ILoggerFactory loggerFactory)
        {
            Domain = CrawlerDomain.Mandarake;
            Parser = new MandarakeParser(loggerFactory.CreateLogger<MandarakeParser>());
            Source = new MandarakeSource(httpClientProvider);
            Cache = new CrawlerCache<MandarakeItem>();
        }
    }
}
