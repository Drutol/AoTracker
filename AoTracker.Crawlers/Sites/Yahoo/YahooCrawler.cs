using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using Microsoft.Extensions.Logging;

namespace AoTracker.Crawlers.Sites.Yahoo
{
    class YahooCrawler : CrawlerBase<YahooItem>
    {
        public YahooCrawler(IHttpClientProvider httpClientProvider, ILoggerFactory loggerFactory)
        {
            Domain = CrawlerDomain.Yahoo;
            Parser = new YahooParser(loggerFactory.CreateLogger<YahooParser>());
            Source = new YahooSource(httpClientProvider);
            Cache = new CrawlerCache<YahooItem>();
        }

        public override async Task<ICrawlerResultList<YahooItem>> Crawl(CrawlerParameters parameters)
        {
            var result = await base.Crawl(parameters);

            if (result.Success)
            {
                var yahooResult = (CrawlerResultBase<YahooItem>) result;
                if (result.Results.Count() == YahooSource.ItemsPerRequest)
                    yahooResult.HasMore = true;
            }

            return result;
        }
    }
}
