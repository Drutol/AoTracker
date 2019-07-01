using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Util
{
    public class DesignTimeCrawlerDescriptor<T> where T : ICrawlerSourceParameters
    {
        public CrawlerDomain CrawlerDomain { get; set; }
        public T CrawlerSourceParameters { get; set; }
    };
}
