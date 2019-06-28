using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;

namespace AoTracker.Crawlers.Interfaces
{
    public interface ICrawler
    {
        CrawlerDomain Domain { get; }
        ICrawlerSource Source { get; }
    }

    public interface ICrawler<T> : ICrawler where T : ICrawlerResultItem
    {
        ICrawlerParser<T> Parser { get; }
    }
}
