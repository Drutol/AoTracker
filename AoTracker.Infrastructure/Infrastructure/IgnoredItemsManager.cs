using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Models;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.Infrastructure
{
    public class IgnoredItemsManager : IIgnoredItemsManager
    {
        private readonly AppVariables _appVariables;
        private HashSet<string> _ignoredItems;

        public List<IgnoredItemEntry> IgnoredEntries { get; set; }

        public IgnoredItemsManager(AppVariables appVariables)
        {
            _appVariables = appVariables;
        }

        public async Task Initialize()
        {
            IgnoredEntries = await _appVariables.IgnoredItems.GetAsync() ?? new List<IgnoredItemEntry>();
            _ignoredItems = new HashSet<string>(IgnoredEntries.Select(entry => entry.InternalId));
        }

        public bool IsItemIgnored(ICrawlerResultItem item)
        {
            return _ignoredItems.Contains(item.InternalId);
        }

        public async void AddIgnoredItem(ICrawlerResultItem item)
        {
            _ignoredItems.Add(item.InternalId);
            IgnoredEntries.Add(new IgnoredItemEntry
            {
                Domain = item.Domain,
                InternalId = item.InternalId,
                Name = item.Name,
                ImageUrl = item.ImageUrl,
                IgnoredAt = DateTime.UtcNow
            });
            await _appVariables.IgnoredItems.SetAsync(IgnoredEntries);
        }

        public async void RemoveIgnoredItem(IgnoredItemEntry entry)
        {
            _ignoredItems.Remove(entry.InternalId);
            IgnoredEntries.Remove(entry);
            await _appVariables.IgnoredItems.SetAsync(IgnoredEntries);
        }
    }
}
