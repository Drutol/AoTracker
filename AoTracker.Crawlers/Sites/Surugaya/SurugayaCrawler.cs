using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using Microsoft.Extensions.Logging;

namespace AoTracker.Crawlers.Surugaya
{
    public class SurugayaCrawler : CrawlerBase<SurugayaItem>
    {
        public SurugayaCrawler(IHttpClientProvider httpClientProvider, ILoggerFactory loggerFactory)
        {
            Domain = CrawlerDomain.Surugaya;
            Parser = new SurugayaParser(loggerFactory.CreateLogger<SurugayaParser>());
            Source = new SurugayaSource(httpClientProvider);
            Cache = new CrawlerCache<SurugayaItem>();
        }
    }
}
