using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.Crawling
{
    public class CrawlerManagerProvider : ICrawlerManagerProvider
    {
        public ICrawlerManager Manager { get; } = new CrawlerManager();
    }
}
