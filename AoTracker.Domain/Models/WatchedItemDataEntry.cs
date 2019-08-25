using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Mandarake;
using AoTracker.Crawlers.Sites.Lashinbang;
using AoTracker.Crawlers.Sites.Mercari;
using AoTracker.Crawlers.Sites.Yahoo;
using AoTracker.Crawlers.Surugaya;
using AoTracker.Domain.Models;

namespace AoTracker.Infrastructure.Models
{
    public class WatchedItemDataEntry
    {
        public WatchedItemDataEntry(WatchedItemEntry watchedItemEntry)
        {
            WatchedItemEntry = watchedItemEntry;

            switch (WatchedItemEntry.Domain)
            {
                case CrawlerDomain.Surugaya:
                    DataProxy = new SurugayaItem();
                    break;
                case CrawlerDomain.Mandarake:
                    DataProxy = new MandarakeItem();
                    break;
                case CrawlerDomain.Yahoo:
                    DataProxy = new YahooItem();
                    break;
                case CrawlerDomain.Mercari:
                    DataProxy = new MercariItem();
                    break;
                case CrawlerDomain.Lashinbang:
                    DataProxy = new LashinbangItem();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            DataProxy.Id = WatchedItemEntry.Id;
            DataProxy.ImageUrl = WatchedItemEntry.ImageUrl;
            DataProxy.Price = CrawlerConstants.InvalidPrice;
            DataProxy.Name = WatchedItemEntry.Name;
        }

        public WatchedItemEntry WatchedItemEntry { get; }
        public ICrawlerResultItem Data { get; set; }

        public CrawlerDomain Domain => WatchedItemEntry.Domain;
        public ICrawlerResultItem DataProxy { get; set; }
    }
}
