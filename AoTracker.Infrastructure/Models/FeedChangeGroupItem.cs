using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Infrastructure.ViewModels.Item;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.Models
{
    public class FeedChangeGroupItem : IMerchItem
    {
        public FeedChangeGroupItem(DateTime lastChanged, IEnumerable<FeedItemViewModel> group)
        {
            LastChanged = lastChanged;
            GroupedItems = new HashSet<string>(group.Select(model => model.BackingModel.InternalId));
        }

        public DateTime LastChanged { get; }
        public HashSet<string> GroupedItems { get; }

        public ICrawlerResultItem Item { get; }
    }
}
