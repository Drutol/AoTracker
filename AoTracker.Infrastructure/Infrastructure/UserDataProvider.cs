using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AoLibs.Adapters.Core;
using AoLibs.Adapters.Core.Interfaces;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Surugaya;
using AoTracker.Domain.Models;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.Infrastructure
{
    public class UserDataProvider : IUserDataProvider
    {
        private readonly AppVariables _appVariables;
        private readonly IDataCache _cache;

        private List<CrawlerSet> _sets;

        public UserDataProvider(AppVariables appVariables, IDataCache cache)
        {
            _appVariables = appVariables;
            _cache = cache;
        }

        public IReadOnlyList<CrawlerSet> CrawlingSets => _sets.AsReadOnly();

        public async Task Initialize()
        {
            _sets = await _appVariables.CrawlerSets.GetAsync() ?? new List<CrawlerSet>();
        }

        public async Task AddNewSet(CrawlerSet set)
        {
            _sets.Add(set);
            await _appVariables.CrawlerSets.SetAsync(_sets);

            await _cache.SaveDataAsync("test", set);

            var data = await _cache.RetrieveData<CrawlerSet>("test");
        }

        public async Task RemoveSet(CrawlerSet set)
        {
            _sets.Remove(set);
            await _appVariables.CrawlerSets.SetAsync(_sets);
        }

        public async Task UpdateSet(CrawlerSet set)
        {
            await _appVariables.CrawlerSets.SetAsync(_sets);
        }
    }
}
