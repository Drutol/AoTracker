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

        public override async Task<ICrawlerResult<SurugayaItem>> Crawl(CrawlerParameters parameters)
        {
            if (Cache.IsCached(parameters))
                return CrawlerResultBase<SurugayaItem>.FromCache(Cache.Get(parameters));

            try
            {
                var source = await Source.ObtainSource(parameters);
                var result = await Parser.Parse(source, parameters);

                Cache.Set(result.Results, parameters);

                return result;
            }
            catch (Exception e)
            {
                return new CrawlerResultBase<SurugayaItem>
                {
                    Success = false
                };
            }

        }
    }
}
