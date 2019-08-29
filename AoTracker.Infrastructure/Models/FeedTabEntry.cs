using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoTracker.Domain.Models;

namespace AoTracker.Infrastructure.Models
{
    public class FeedTabEntry : IDisposable
    {
        public event EventHandler CrawlerSetsChanged;
        public event EventHandler Disposed;

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

        public void ResetEventSubscriptions()
        {
            CrawlerSetsChanged = null;
        }

        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }
    }
}
