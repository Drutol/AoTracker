using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Infrastructure.Models;

namespace AoTracker.Interfaces
{
    public interface IIgnoredItemsManager : IInitializable
    {
        List<IgnoredItemEntry> IgnoredEntries { get; }

        bool IsItemIgnored(ICrawlerResultItem item);
        void AddIgnoredItem(ICrawlerResultItem item);
        void RemoveIgnoredItem(IgnoredItemEntry entry);
    }
}
