using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Interfaces;
using Microsoft.Extensions.Logging;

namespace AoTracker.Infrastructure.Crawling
{
    public class CrawlerManagerProvider : ICrawlerManagerProvider
    {
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly ILoggerFactory _loggerFactory;
        private bool _initialized;

        private readonly ICrawlerManager _manager = new CrawlerManager();

        public CrawlerManagerProvider(IHttpClientProvider httpClientProvider,
            ILoggerFactory loggerFactory)
        {
            _httpClientProvider = httpClientProvider;
            _loggerFactory = loggerFactory;
        }

        public ICrawlerManager Manager
        {
            get
            {
                if (!_initialized)
                {
                    _manager.InitializeCrawlers(_httpClientProvider, _loggerFactory);
                    _initialized = true;
                }
                return _manager;
            }
        }
    }
}