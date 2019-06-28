using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Domain.Models
{
    public class CrawlerDescriptor
    {
        public CrawlerDomain CrawlerDomain { get; set; }
        public ICrawlerSourceParameters CrawlerSourceParameters { get; set; }
    }
}
