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

namespace AoTracker.Crawlers.Infrastructure
{
    public class CrawlerManager : ICrawlerManager
    {
        protected Dictionary<CrawlerDomain, ICrawler> _crawlers;

        private readonly Dictionary<Type, CrawlerDomain> _typeMapping = new Dictionary<Type, CrawlerDomain>
        {
            {typeof(SurugayaItem), CrawlerDomain.Surugaya}
        };

        public virtual void InitializeCrawlers(IHttpClientProvider httpClientProvider)
        {
            _crawlers = new Dictionary<CrawlerDomain, ICrawler>
            {
                {CrawlerDomain.Surugaya, new SurugayaCrawler(httpClientProvider)},
                {CrawlerDomain.Mandarake, new MandarakeCrawler(httpClientProvider)},
                {CrawlerDomain.Mercari, new MercariCrawler(httpClientProvider)},
                {CrawlerDomain.Yahoo, new YahooCrawler(httpClientProvider)},
                {CrawlerDomain.Lashinbang, new LashinbangCrawler(httpClientProvider)},
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
