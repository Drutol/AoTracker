using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Models;

namespace AoTracker.Infrastructure.Models.NavArgs
{
    public class ConfigureCrawlerPageNavArgs
    {
        public bool ConfigureNew { get; set; }

        public CrawlerDescriptor DescriptorToEdit { get; set; }

        public ICrawlerSourceParameters CrawlerSourceParameters { get; set; }
        public CrawlerDomain Domain { get; set; }
    }
}
