using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Enums;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.LinkHandlers
{
    public class ZenMarketLinkHandler : IDomainLinkHandler
    {
        public ProxyDomain HandlingDomain { get; } = ProxyDomain.ZenMarket;

        public string GenerateWebsiteLink(ICrawlerResultItem item)
        {
            switch (item.Domain)
            {
                case CrawlerDomain.Surugaya:
                    return
                        $"https://zenmarket.jp/en/othershopproduct.aspx?u=https://www.suruga-ya.jp/product/detail/{item.Id}";
                case CrawlerDomain.Yahoo:
                    return $"https://zenmarket.jp/en/auction.aspx?itemCode={item.Id}";
                default:
                    return null;
            }
        }
    }
}
