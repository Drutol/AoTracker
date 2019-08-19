using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        private ObservableCollection<CrawlerSet> _sets;

        public UserDataProvider(AppVariables appVariables)
        {
            _appVariables = appVariables;
        }

        public ObservableCollection<CrawlerSet> CrawlingSets => _sets;

        public async Task Initialize()
        {
            _sets = new ObservableCollection<CrawlerSet>(await _appVariables.CrawlerSets.GetAsync() ?? new List<CrawlerSet>());
        }

        public async Task AddNewSet(CrawlerSet set)
        {
            _sets.Add(set);
            await _appVariables.CrawlerSets.SetAsync(_sets.ToList());
        }

        public async Task RemoveSet(CrawlerSet set)
        {
            _sets.Remove(set);
            await _appVariables.CrawlerSets.SetAsync(_sets.ToList());
        }

        public async Task UpdateSet(CrawlerSet set)
        {
            await _appVariables.CrawlerSets.SetAsync(_sets.ToList());
        }

        public async Task MoveSet(int movedPosition, int targetPosition)
        {
            _sets.Move(movedPosition, targetPosition);
            await _appVariables.CrawlerSets.SetAsync(_sets.ToList());
        }
    }
}
