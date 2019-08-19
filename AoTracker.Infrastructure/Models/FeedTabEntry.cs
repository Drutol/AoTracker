using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoTracker.Domain.Models;

namespace AoTracker.Infrastructure.Models
{
    public class FeedTabEntry
    {
        public event EventHandler CrawlerSetsChanged;

        private List<CrawlerSet> _crawlerSets;

        public string Name { get; set; }

        public List<CrawlerSet> CrawlerSets
        {
            get => _crawlerSets;
            set
            {
                _crawlerSets = value;
                CrawlerSetsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public FeedTabEntry(List<CrawlerSet> crawlerSets)
        {
            CrawlerSets = crawlerSets;
        }
    }
}
