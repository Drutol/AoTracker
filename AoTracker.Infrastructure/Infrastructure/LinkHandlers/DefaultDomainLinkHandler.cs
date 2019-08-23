using System;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Enums;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.LinkHandlers
{
    public class DefaultDomainLinkHandler : IDomainLinkHandler
    {
        public ProxyDomain HandlingDomain { get; } = ProxyDomain.None;

        public string GenerateWebsiteLink(ICrawlerResultItem item)
        {
            switch (item.Domain)
            {
                case CrawlerDomain.Surugaya:
                    return $"https://www.suruga-ya.jp/product/detail/{item.Id}";
                case CrawlerDomain.Mandarake:
                    return $"https://order.mandarake.co.jp/order/detailPage/item?itemCode={item.Id}";
                case CrawlerDomain.Yahoo:
                    return $"https://page.auctions.yahoo.co.jp/jp/auction/{item.Id}";
                case CrawlerDomain.Mercari:
                    return $"https://item.mercari.com/jp/{item.Id}";
                case CrawlerDomain.Lashinbang:
                    return $"https://shop.lashinbang.com/products/detail/{item.Id}";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
