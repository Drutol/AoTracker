using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Models;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.ViewModels.Feed
{
    public class FeedViewModel : ViewModelBase
    {
        private readonly IUserDataProvider _userDataProvider;
        private List<FeedTabEntry> _feedTabEntries;

        public List<FeedTabEntry> FeedTabEntries
        {
            get => _feedTabEntries;
            set => Set(ref _feedTabEntries, value);
        }

        public FeedViewModel(IUserDataProvider userDataProvider)
        {
            _userDataProvider = userDataProvider;
        }

        public void NavigatedTo()
        {
            var entries = new List<FeedTabEntry>
            {
                new FeedTabEntry(_userDataProvider.CrawlingSets.Where(set => set.Descriptors.Any()).ToList())
                {
                    Name = "All"
                }
            };

            foreach (var crawlerSet in _userDataProvider.CrawlingSets.Where(set => set.Descriptors.Any()).Take(5))
            {
                entries.Add(new FeedTabEntry(new List<CrawlerSet> {crawlerSet})
                {
                    Name = crawlerSet.Name
                });
            }

            FeedTabEntries = entries;
        }
    }
}
