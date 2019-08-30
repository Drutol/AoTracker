using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Surugaya;
using AoTracker.Infrastructure.Crawling.CloudParser;
using Microsoft.Extensions.Logging;

namespace AoTracker.Infrastructure.Crawling
{
    public class CloudCrawlingManagerProvider : CrawlerManager
    {
        public override void InitializeCrawlers(IHttpClientProvider httpClientProvider, ILoggerFactory loggerFactory)
        {
            _crawlers = new Dictionary<CrawlerDomain, ICrawler>
            {
                {
                    CrawlerDomain.Surugaya,
                    new SurugayaCrawler(httpClientProvider, loggerFactory)
                    {
                        Parser = new SurugayaCloudParser(httpClientProvider)
                    }
                }
            };
        }
    }
}
