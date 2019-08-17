using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Infrastructure
{
    public abstract class CrawlerBase<T> : ICrawler<T> where T : ICrawlerResultItem
    {
        public CrawlerDomain Domain { get; protected set; }
        public ICrawlerSource Source { get; protected set; }


        public ICrawlerParser<T> Parser { get; set; }
        public ICrawlerCache<T> Cache { get; set; }

        public virtual async Task<ICrawlerResult<T>> Crawl(CrawlerParameters parameters)
        {
            if (parameters.VolatileParameters.UseCache && Cache.IsCached(parameters))
                return CrawlerResultBase<T>.FromCache(Cache.Get(parameters));

            try
            {
                var source = await Source.ObtainSource(parameters);
                var result = await Parser.Parse(source, parameters);

                Cache.Set(result.Results, parameters);

                return result;
            }
            catch (Exception e)
            {
                return new CrawlerResultBase<T>
                {
                    Success = false
                };
            }
        }

         async Task<ICrawlerResult<ICrawlerResultItem>> ICrawler.Crawl(CrawlerParameters parameters)
         {
             return (ICrawlerResult<ICrawlerResultItem>) await Crawl(parameters);
         }
    }
}
