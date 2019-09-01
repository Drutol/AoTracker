using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Enums;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.LinkHandlers
{
    class FromJapanLinkHandler : IDomainLinkHandler
    {
        public ProxyDomain HandlingDomain { get; } = ProxyDomain.FromJapan;

        public string GenerateWebsiteLink(ICrawlerResultItem item)
        {
            switch (item.Domain)
            {
                case CrawlerDomain.Surugaya:
                    return
                        $"https://www.fromjapan.co.jp/en/special/order/confirm/https://www.suruga-ya.jp/product/detail/{item.Id}/11_1";
                case CrawlerDomain.Yahoo:
                    return $"https://www.fromjapan.co.jp/en/auction/yahoo/input/{item.Id}";
                default:
                    return null;
            }
        }
    }
}
