using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Surugaya;
using AoTracker.Infrastructure.Crawling.CloudParser;

namespace AoTracker.Infrastructure.Crawling
{
    public class CloudCrawlingManagerProvider : CrawlerManager
    {
        public override void InitializeCrawlers(IHttpClientProvider httpClientProvider)
        {
            _crawlers = new Dictionary<CrawlerDomain, ICrawler>
            {
                {
                    CrawlerDomain.Surugaya,
                    new SurugayaCrawler(httpClientProvider)
                    {
                        Parser = new SurugayaCloudParser(httpClientProvider)
                    }
                }
            };
        }
    }
}
