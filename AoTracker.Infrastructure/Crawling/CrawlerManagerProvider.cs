using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.Crawling
{
    public class CrawlerManagerProvider : ICrawlerManagerProvider
    {
        private bool _initialized;

        private readonly ICrawlerManager _manager = new CrawlerManager();

        public ICrawlerManager Manager
        {
            get
            {
                if (!_initialized)
                {
                    _manager.InitializeCrawlers(new HttpProvider());
                }
                return _manager;
            }
        }

        class HttpProvider : IHttpClientProvider
        {
            public HttpClient HttpClient { get; } = new HttpClient();
        }
    }
}