using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Enums;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.LinkHandlers
{
    public class NeokyoLinkHandler : IDomainLinkHandler
    {
        public ProxyDomain HandlingDomain { get; } = ProxyDomain.Neokyo;

        public string GenerateWebsiteLink(ICrawlerResultItem item)
        {
            switch (item.Domain)
            {
                case CrawlerDomain.Surugaya:
                    return $"https://neokyo.com/product/surugaya/{item.Id}";
                default:
                    return null;
            }
        }
    }
}
