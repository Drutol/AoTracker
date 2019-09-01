using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Enums;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.LinkHandlers
{
    class DeJapanLinkHandler : IDomainLinkHandler
    {
        public ProxyDomain HandlingDomain { get; } = ProxyDomain.DeJapan;

        public string GenerateWebsiteLink(ICrawlerResultItem item)
        {
            switch (item.Domain)
            {
                case CrawlerDomain.Yahoo:
                    return "https://www.dejapan.com/en/top/mall/asp/detail.asp?code=v659181765";
                default:
                    return null;
            }
        }
    }
}
