using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Infrastructure
{
    public class CrawlerParameters
    {
        public ICrawlerSourceParameters Parameters { get; set; }
        public ICrawlerVolatileParameters VolatileParameters { get; set; }
    }
}
