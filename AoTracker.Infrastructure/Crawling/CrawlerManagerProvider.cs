using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.Crawling
{
    public class CrawlerManagerProvider : ICrawlerManagerProvider
    {
        private readonly IHttpClientProvider _httpClientProvider;
        private bool _initialized;

        private readonly ICrawlerManager _manager = new CrawlerManager();

        public CrawlerManagerProvider(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }

        public ICrawlerManager Manager
        {
            get
            {
                if (!_initialized)
                {
                    _manager.InitializeCrawlers(_httpClientProvider);
                }
                return _manager;
            }
        }
    }
}