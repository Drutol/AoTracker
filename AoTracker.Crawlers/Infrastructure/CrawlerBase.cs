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

        public abstract Task<ICrawlerResult<T>> Crawl(ICrawlerSourceParameters parameters);
    }
}
