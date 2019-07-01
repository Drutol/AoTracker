using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Infrastructure.Models.NavArgs
{
    public class ConfigureCrawlerPageNavArgs
    {
        public ICrawlerSourceParameters CrawlerSourceParameters { get; set; }
        public CrawlerDomain Domain { get; set; }
    }
}
