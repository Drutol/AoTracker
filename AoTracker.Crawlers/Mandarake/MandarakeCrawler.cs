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

        public override async Task<ICrawlerResult<MandarakeItem>> Crawl(CrawlerParameters parameters)
        {
            if (parameters.VolatileParameters.UseCache && Cache.IsCached(parameters))
                return CrawlerResultBase<MandarakeItem>.FromCache(Cache.Get(parameters));

            try
            {
                var source = await Source.ObtainSource(parameters);
                var result = await Parser.Parse(source, parameters);

                Cache.Set(result.Results, parameters);

                return result;
            }
            catch (Exception e)
            {
                return new CrawlerResultBase<MandarakeItem>
                {
                    Success = false
                };
            }

        }
    }
}
