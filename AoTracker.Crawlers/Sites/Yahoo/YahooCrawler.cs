using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public override async Task<ICrawlerResultList<YahooItem>> Crawl(CrawlerParameters parameters)
        {
            var result = await base.Crawl(parameters);

            if (result.Success)
            {
                var yahooResult = (CrawlerResultBase<YahooItem>) result;
                if (result.Results.Count() == 120)
                    yahooResult.HasMore = true;
            }

            return result;
        }
    }
}
