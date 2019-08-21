using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Models;

namespace AoTracker.Infrastructure.Models.Messages
{
    public class CrawlerSetModifiedMessage
    {
        public bool FavouriteChanged { get; }
        public CrawlerSet ModifiedCrawlerSet { get; }

        public CrawlerSetModifiedMessage(CrawlerSet set)
        {
            ModifiedCrawlerSet = set;
        }

        public CrawlerSetModifiedMessage(bool favouriteChanged)
        {
            FavouriteChanged = favouriteChanged;
        }
    }
}
