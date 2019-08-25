using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Models;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.Infrastructure
{
    public class WatchedItemsManager : IWatchedItemsManager
    {
        private readonly AppVariables _appVariables;
        private readonly ICrawlerManagerProvider _crawlerManager;
        public event EventHandler<WatchedItemDataEntry> ItemDetailsFetched;
        public List<WatchedItemDataEntry> Entries { get; private set; }
        private HashSet<string> _watchedItems = new HashSet<string>();

        private readonly ConcurrentDictionary<CrawlerDomain, SemaphoreSlim> _domainSemaphores =
            new ConcurrentDictionary<CrawlerDomain, SemaphoreSlim>();

        public WatchedItemsManager(
            AppVariables appVariables,
            ICrawlerManagerProvider crawlerManager)
        {
            _appVariables = appVariables;
            _crawlerManager = crawlerManager;
        }

        public async Task Initialize()
        {
            Entries = (await _appVariables.WatchedItems.GetAsync() ?? Enumerable.Empty<WatchedItemEntry>())
                .Select(entry => new WatchedItemDataEntry(entry))
                .ToList();
            _watchedItems = new HashSet<string>(Entries.Select(entry => entry.WatchedItemEntry.Id));

            foreach (CrawlerDomain domain in Enum.GetValues(typeof(CrawlerDomain)))
            {
                _domainSemaphores[domain] = new SemaphoreSlim(2);
            }
        }

        public async void AddWatchedEntry(ICrawlerResultItem entry)
        {
            _watchedItems.Add(entry.Id);
            Entries.Add(new WatchedItemDataEntry(new WatchedItemEntry
            {
                Domain = entry.Domain,
                Id = entry.Id,
                ImageUrl = entry.ImageUrl,
                Name = entry.Name
            })
            {
                Data = entry
            });
            await _appVariables.WatchedItems.SetAsync(Entries.Select(dataEntry => dataEntry.WatchedItemEntry).ToList());
        }

        public async void RemoveWatchedEntry(WatchedItemDataEntry entry)
        {
            _watchedItems.Remove(entry.WatchedItemEntry.Id);
            Entries.Remove(entry);
            await _appVariables.WatchedItems.SetAsync(Entries.Select(dataEntry => dataEntry.WatchedItemEntry).ToList());
        }

        public void RemoveWatchedEntry(ICrawlerResultItem item)
        {
            var entry = Entries.First(dataEntry => dataEntry.WatchedItemEntry.Id.Equals(item.Id));
            RemoveWatchedEntry(entry);
        }

        public async void StartAggregatingWatchedItemsData()
        {
            var tasks = new List<Task>();

            foreach (var watchedItemDataEntry in Entries)
            {
                tasks.Add(FetchData(watchedItemDataEntry));
            }

            await Task.WhenAll(tasks);
        }

        public async void RequestSingleItemUpdate(WatchedItemDataEntry entry)
        {
            await FetchData(entry);
        }

        public bool IsWatched(ICrawlerResultItem item)
        {
            return _watchedItems.Contains(item.Id);
        }

        private async Task FetchData(WatchedItemDataEntry entry)
        {
            var semaphore = _domainSemaphores[entry.WatchedItemEntry.Domain];
            try
            {
                await semaphore.WaitAsync();
                var crawler = _crawlerManager.Manager.GetCrawler(entry.WatchedItemEntry.Domain);
                var result = await crawler.CrawlById(entry.WatchedItemEntry.Id);
                entry.Data = result.Result;

                ItemDetailsFetched?.Invoke(this, entry);
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
