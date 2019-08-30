using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Mandarake;
using AoTracker.Crawlers.Sites.Lashinbang;
using AoTracker.Crawlers.Sites.Mercari;
using AoTracker.Crawlers.Sites.Yahoo;
using AoTracker.Crawlers.Surugaya;
using Microsoft.Extensions.Logging;

namespace AoTracker.Crawlers.Infrastructure
{
    public class CrawlerManager : ICrawlerManager
    {
        protected Dictionary<CrawlerDomain, ICrawler> _crawlers;

        private readonly Dictionary<Type, CrawlerDomain> _typeMapping = new Dictionary<Type, CrawlerDomain>
        {
            {typeof(SurugayaItem), CrawlerDomain.Surugaya}
        };

        public virtual void InitializeCrawlers(IHttpClientProvider httpClientProvider, ILoggerFactory loggerFactory)
        {
            _crawlers = new Dictionary<CrawlerDomain, ICrawler>
            {
                {CrawlerDomain.Surugaya, new SurugayaCrawler(httpClientProvider, loggerFactory)},
                {CrawlerDomain.Mandarake, new MandarakeCrawler(httpClientProvider, loggerFactory)},
                {CrawlerDomain.Mercari, new MercariCrawler(httpClientProvider, loggerFactory)},
                {CrawlerDomain.Yahoo, new YahooCrawler(httpClientProvider, loggerFactory)},
                {CrawlerDomain.Lashinbang, new LashinbangCrawler(httpClientProvider, loggerFactory)},
            };
        }

        public ICrawler<T> GetCrawler<T>() where T : ICrawlerResultItem
        {
            return _crawlers[_typeMapping[typeof(T)]] as ICrawler<T>;
        }

        public ICrawler GetCrawler(CrawlerDomain crawlerDomain)
        {
            return _crawlers[crawlerDomain];
        }
    }
}
