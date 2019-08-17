using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Surugaya
{
    public class SurugayaCrawler : CrawlerBase<SurugayaItem>
    {
        public SurugayaCrawler(IHttpClientProvider httpClientProvider)
        {
            Domain = CrawlerDomain.Surugaya;
            Parser = new SurugayaParser();
            Source = new SurugayaSource(httpClientProvider);
            Cache = new CrawlerCache<SurugayaItem>();
        }
    }
}
